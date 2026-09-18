using Content.Shared.Access;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Turrets;

/// <summary>
/// Attached to entities to provide them with turret target selection data.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(TurretTargetSettingsSystem))]
public sealed partial class TurretTargetSettingsComponent : Component
{
    /// <summary>
    /// Crew with one or more access levels from this list are exempt from being targeted by turrets.
    /// </summary>
    [DataField, AutoNetworkedField]
    public HashSet<ProtoId<AccessLevelPrototype>> ExemptAccessLevels = new();

    // erida edit start
    [DataField, AutoNetworkedField]
    public TurretTargetingMode Mode = TurretTargetingMode.AccessExempt;
    // erida edit end
}

[Flags]
public enum TurretTargetingMode : byte
{
    AccessExempt = 1 << 0,
    IgnoreAccess = 1 << 1,
    NoMindshield = 1 << 2,
    Wanted = 1 << 3,
    Detained = 1 << 4,
    NotInManifest = 1 << 5,
}
