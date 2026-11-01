
using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared._RMC14.Vendors
{
    [RegisterComponent]
    public sealed partial class CMAutomatedVendorComponent : Component
    {
        [DataField]
        public List<CMVendorSection> Sections = new();

        [DataField]
        public bool Hackable;

        [DataField]
        public bool EjectContentsOnDestruction;

        [DataField]
        public Robust.Shared.Utility.SpriteSpecifier? BaseSprite;

        [DataField]
        public Robust.Shared.Utility.SpriteSpecifier? AnimationSprite;
    }

    [DataDefinition]
    public sealed partial class CMVendorSection
    {
        [DataField]
        public string Name = string.Empty;

        [DataField]
        public List<CMVendorEntry> Entries = new();
    }

    [DataDefinition]
    public sealed partial class CMVendorEntry
    {
        [DataField]
        public EntProtoId Id = string.Empty;

        [DataField]
        public string Name = string.Empty;

        [DataField]
        public int? Amount;

        [DataField]
        public int? Multiplier;

        [DataField]
        public int? Max;
    }

    /// <summary>
    /// Raised on a player after they vend an item from an automated vendor.
    /// Never raised on our build (vendor surface is stubbed).
    /// </summary>
    public readonly record struct RMCAutomatedVendedUserEvent(EntityUid Item);

    public sealed partial class SharedCMAutomatedVendorSystem : EntitySystem
    {
        public void SetSections(Entity<CMAutomatedVendorComponent> vendor, List<CMVendorSection> sections)
        {
        }
    }
}

namespace Content.Shared._RMC14.Intel
{
    [RegisterComponent]
    public sealed partial class IntelTechTreeComponent : Component
    {
    }

    public sealed partial class IntelSystem : EntitySystem
    {
        private EntityUid? _techTree;

        public Entity<IntelTechTreeComponent> EnsureTechTree()
        {
            if (_techTree is { } existing && Exists(existing) && TryComp(existing, out IntelTechTreeComponent? comp))
                return (existing, comp);

            var uid = Spawn(null, Robust.Shared.Map.MapCoordinates.Nullspace);
            var tree = EnsureComp<IntelTechTreeComponent>(uid);
            _techTree = uid;
            return (uid, tree);
        }
    }
}

namespace Content.Shared._RMC14.Intel.Tech
{
    /// <summary>
    /// from TechnologyPrototype.GenericUnlocks[].PurchaseEvent so NT vehicle techs unlock
    /// the supply console's gated entries (VehicleSupplySystem.OnTechUnlockVehicle).
    /// </summary>
    [Robust.Shared.Serialization.Manager.Attributes.DataDefinition]
    public sealed partial class TechUnlockVehicleEvent : Robust.Shared.GameObjects.EntityEventArgs
    {
        [Robust.Shared.Serialization.Manager.Attributes.DataField]
        public string Unlock = string.Empty;
    }

    public sealed partial class TechSystem : EntitySystem
    {
        public void SetVehicleUnlockOptionDisabled(EntProtoId unlock, bool disabled)
        {
        }
    }
}
