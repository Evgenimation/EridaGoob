using Content.Shared.Access;
using Content.Shared.Access.Systems;
using Content.Shared.CriminalRecords;
using Content.Shared.Humanoid;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Mindshield.Components;
using Content.Shared.Security;
using Content.Shared.SSDIndicator;
using Content.Shared.StationRecords;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Shared.Turrets;

/// <summary>
/// This system is used for validating potential targets for NPCs with a <see cref="TurretTargetSettingsComponent"/> (i.e., turrets).
/// A turret will consider an entity a valid target if the entity does not possess any access tags which appear on the
/// turret's <see cref="TurretTargetSettingsComponent.ExemptAccessLevels"/> list.
/// </summary>
public sealed partial class TurretTargetSettingsSystem : EntitySystem
{
    [Dependency] private readonly AccessReaderSystem _accessReader = default!;
    [Dependency] private readonly ItemToggleSystem _toggle = default!; // goob edit dont target disabled borgs
    // erida edit start
    [Dependency] private readonly SharedIdCardSystem _idCard = default!;
    [Dependency] private readonly SharedStationRecordsSystem _records = default!;
    // erida edit end
    
    private ProtoId<AccessLevelPrototype> _accessLevelBorg = "Borg";
    private ProtoId<AccessLevelPrototype> _accessLevelBasicSilicon = "BasicSilicon";

    /// <summary>
    /// Adds or removes access levels from a <see cref="TurretTargetSettingsComponent.ExemptAccessLevels"/> list.
    /// </summary>
    /// <param name="ent">The entity and its <see cref="TurretTargetSettingsComponent"/></param>
    /// <param name="exemption">The proto ID for the access level</param>
    /// <param name="enabled">Set 'true' to add the exemption, or 'false' to remove it</param>
    /// <param name="dirty">Set 'true' to dirty the component</param>
    [PublicAPI]
    public void SetAccessLevelExemption(Entity<TurretTargetSettingsComponent> ent, ProtoId<AccessLevelPrototype> exemption, bool enabled, bool dirty = true)
    {
        if (enabled)
            ent.Comp.ExemptAccessLevels.Add(exemption);
        else
            ent.Comp.ExemptAccessLevels.Remove(exemption);

        if (dirty)
            Dirty(ent);
    }

    /// <summary>
    /// Adds or removes a collection of access levels from a <see cref="TurretTargetSettingsComponent.ExemptAccessLevels"/> list.
    /// </summary>
    /// <param name="ent">The entity and its <see cref="TurretTargetSettingsComponent"/></param>
    /// <param name="exemption">The collection of access level proto IDs to add or remove</param>
    /// <param name="enabled">Set 'true' to add the collection as exemptions, or 'false' to remove them</param>
    [PublicAPI]
    public void SetAccessLevelExemptions(Entity<TurretTargetSettingsComponent> ent, ICollection<ProtoId<AccessLevelPrototype>> exemptions, bool enabled)
    {
        foreach (var exemption in exemptions)
            SetAccessLevelExemption(ent, exemption, enabled, false);

        Dirty(ent);
    }

    /// <summary>
    /// Sets a <see cref="TurretTargetSettingsComponent.ExemptAccessLevels"/> list to contain only a supplied collection of access levels.
    /// </summary>
    /// <param name="ent">The entity and its <see cref="TurretTargetSettingsComponent"/></param>
    /// <param name="exemptions">The supplied collection of access level proto IDs</param>
    [PublicAPI]
    public void SyncAccessLevelExemptions(Entity<TurretTargetSettingsComponent> ent, ICollection<ProtoId<AccessLevelPrototype>> exemptions)
    {
        ent.Comp.ExemptAccessLevels.Clear();
        SetAccessLevelExemptions(ent, exemptions, true);
    }

    /// <summary>
    /// Sets a <see cref="TurretTargetSettingsComponent.ExemptAccessLevels"/> list to match that of another.
    /// </summary>
    /// <param name="target">The entity this is having its exemption list updated <see cref="TurretTargetSettingsComponent"/></param>
    /// <param name="source">The entity that is being used as a template for the target</param>
    [PublicAPI]
    public void SyncAccessLevelExemptions(Entity<TurretTargetSettingsComponent> target, Entity<TurretTargetSettingsComponent> source)
    {
        SyncAccessLevelExemptions(target, source.Comp.ExemptAccessLevels);
    }

    /// <summary>
    /// Returns whether a <see cref="TurretTargetSettingsComponent.ExemptAccessLevels"/> list contains a specific access level.
    /// </summary>
    /// <param name="ent">The entity and its <see cref="TurretTargetSettingsComponent"/></param>
    /// <param name="exemption">The access level proto ID being checked</param>
    [PublicAPI]
    public bool HasAccessLevelExemption(Entity<TurretTargetSettingsComponent> ent, ProtoId<AccessLevelPrototype> exemption)
    {
        if (ent.Comp.ExemptAccessLevels.Count == 0)
            return false;

        return ent.Comp.ExemptAccessLevels.Contains(exemption);
    }

    /// <summary>
    /// Returns whether a <see cref="TurretTargetSettingsComponent.ExemptAccessLevels"/> list contains one or more access levels from another collection.
    /// </summary>
    /// <param name="ent">The entity and its <see cref="TurretTargetSettingsComponent"/></param>
    /// <param name="exemptions"></param>
    [PublicAPI]
    public bool HasAnyAccessLevelExemption(Entity<TurretTargetSettingsComponent> ent, ICollection<ProtoId<AccessLevelPrototype>> exemptions)
    {
        if (ent.Comp.ExemptAccessLevels.Count == 0)
            return false;

        foreach (var exemption in exemptions)
        {
            if (HasAccessLevelExemption(ent, exemption))
                return true;
        }

        return false;
    }

    // erida edit start
    [PublicAPI]
    public void SetTargetingMode(Entity<TurretTargetSettingsComponent> ent, TurretTargetingMode mode)
    {
        ent.Comp.Mode = mode;
        Dirty(ent);
    }

    [PublicAPI]
    public bool EntityIsTargetForTurret(Entity<TurretTargetSettingsComponent> ent, EntityUid target)
    {
        var accessLevels = _accessReader.FindAccessTags(target);

        // Borgs and silicons are only targeted if not exempt.
        if (accessLevels.Contains(_accessLevelBorg))
            return !HasAccessLevelExemption(ent, _accessLevelBorg);

        if (accessLevels.Contains(_accessLevelBasicSilicon))
            return !HasAccessLevelExemption(ent, _accessLevelBasicSilicon);

        if (!_toggle.IsActivated(target)) // goob edit dont target disabled borgs
            return !HasAccessLevelExemption(ent, _accessLevelBorg); // goob edit dont target disabled borgs

        if (ent.Comp.Mode.HasFlag(TurretTargetingMode.IgnoreAccess))
            return true;

        var isHumanoid = HasComp<HumanoidAppearanceComponent>(target);

        // Only apply status/identity-based targeting modes to humanoids (all playable races, not animals).
        if (isHumanoid)
        {
            // SSD entities are always targeted.
            if (TryComp<SSDIndicatorComponent>(target, out var ssd) && ssd.IsSSD)
                return true;

            if (ent.Comp.Mode.HasFlag(TurretTargetingMode.NoMindshield) &&
                !HasComp<MindShieldComponent>(target) &&
                !HasComp<FakeMindShieldComponent>(target))
            {
                return true;
            }

            if (ent.Comp.Mode.HasFlag(TurretTargetingMode.Wanted) || ent.Comp.Mode.HasFlag(TurretTargetingMode.Detained))
            {
                var status = GetTargetSecurityStatus(target);
                if (status != null)
                {
                    if (ent.Comp.Mode.HasFlag(TurretTargetingMode.Wanted) && status.Value == SecurityStatus.Wanted)
                        return true;
                    if (ent.Comp.Mode.HasFlag(TurretTargetingMode.Detained) && status.Value == SecurityStatus.Detained)
                        return true;
                }
            }

            if (ent.Comp.Mode.HasFlag(TurretTargetingMode.NotInManifest) && !HasCrewRecord(target))
                return true;
        }

        // Animals and other non-humanoids are never valid targets unless IgnoreAccess is set.
        if (!isHumanoid)
            return false;

        return !HasAnyAccessLevelExemption(ent, accessLevels);
    }

    private SecurityStatus? GetTargetSecurityStatus(EntityUid target)
    {
        if (!_idCard.TryFindIdCard(target, out var idCard))
            return null;

        if (!TryComp<StationRecordKeyStorageComponent>(idCard, out var keyStorage) || keyStorage.Key == null)
            return null;

        if (!_records.TryGetRecord<CriminalRecord>(keyStorage.Key.Value, out var criminalRecord))
            return null;

        return criminalRecord.Status;
    }

    private bool HasCrewRecord(EntityUid target)
    {
        if (!_idCard.TryFindIdCard(target, out var idCard))
            return false;

        if (!TryComp<StationRecordKeyStorageComponent>(idCard, out var keyStorage) || keyStorage.Key == null)
            return false;

        return _records.TryGetRecord<GeneralStationRecord>(keyStorage.Key.Value, out _);
    }
    // erida edit end
}
