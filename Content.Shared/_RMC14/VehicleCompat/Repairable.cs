// erida edit 
using Content.Goobstation.Maths.FixedPoint;

namespace Content.Shared._RMC14.Repairable;

public sealed class RMCRepairableSystem : EntitySystem
{
    public bool UseFuel(EntityUid tool, EntityUid user, FixedPoint2 fuelUsed, bool attempt = false)
    {
        return true;
    }
}
