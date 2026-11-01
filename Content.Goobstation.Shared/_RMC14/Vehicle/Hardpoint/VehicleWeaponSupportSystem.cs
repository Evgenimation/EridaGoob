using Content.Shared._RMC14.Weapons.Ranged;
using Content.Shared.Vehicle.Components;
using Content.Goobstation.Shared.Vehicles;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;

namespace Content.Shared._RMC14.Vehicle;

public sealed partial class VehicleWeaponSupportSystem : EntitySystem
{
    [Dependency] private VehicleTopologySystem _topology = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<HardpointItemComponent, GunRefreshModifiersEvent>(OnGunRefresh);
        SubscribeLocalEvent<HardpointItemComponent, GetWeaponAccuracyEvent>(OnGetAccuracy);
    }

    private void OnGunRefresh(Entity<HardpointItemComponent> ent, ref GunRefreshModifiersEvent args)
    {
        if (!_topology.TryGetVehicle(ent.Owner, out var vehicle))
            return;

        if (!TryComp(vehicle, out VehicleWeaponSupportModifierComponent? mods))
            return;

        args.FireRate *= mods.FireRateMultiplier;
    }

    private void OnGetAccuracy(Entity<HardpointItemComponent> ent, ref GetWeaponAccuracyEvent args)
    {
        if (!_topology.TryGetVehicle(ent.Owner, out var vehicle))
            return;

        if (!TryComp(vehicle, out VehicleWeaponSupportModifierComponent? mods))
            return;

        args.AccuracyMultiplier *= mods.AccuracyMultiplier;
    }
}
