// erida edit start
using Content.Shared.Weapons.Ranged.Components;
using Robust.Shared.GameObjects;

namespace Content.Shared.Weapons.Ranged.Systems;

[ByRefEvent]
public struct VehicleGunRequest
{
    public readonly EntityUid Entity;
    public Entity<GunComponent> Gun;
    public bool Handled;

    public VehicleGunRequest(EntityUid entity)
    {
        Entity = entity;
        Gun = default;
        Handled = false;
    }
}

public abstract partial class SharedGunSystem
{
    /// <summary>
    /// Sets the current gun target, returning the previous value.
    /// </summary>
    public EntityUid? SwapTarget(Entity<GunComponent> gun, EntityUid? target)
    {
        var previous = gun.Comp.Target;
        gun.Comp.Target = target;
        return previous;
    }

    public bool TryGetVehicleGun(EntityUid entity, out Entity<GunComponent> gun)
    {
        var ev = new VehicleGunRequest(entity);
        RaiseLocalEvent(entity, ref ev);
        gun = ev.Gun;
        return ev.Handled;
    }
}
// erida edit end
