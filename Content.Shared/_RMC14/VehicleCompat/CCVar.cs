using Robust.Shared;
using Robust.Shared.Configuration;

namespace Content.Shared._RMC14.CCVar;

// erida edit
public sealed partial class RMCCVars : CVars
{
    public static readonly CVarDef<bool> VehicleDebugOverlay =
        CVarDef.Create("rmc.vehicle.debug_overlay", false, CVar.CLIENT | CVar.ARCHIVE);

    public static readonly CVarDef<bool> VehicleHardpointOverlay =
        CVarDef.Create("rmc.vehicle.hardpoint_overlay", false, CVar.CLIENT | CVar.ARCHIVE);

    public static readonly CVarDef<bool> VehicleCollisionOverlay =
        CVarDef.Create("rmc.vehicle.collision_overlay", false, CVar.CLIENT | CVar.ARCHIVE);

    public static readonly CVarDef<bool> VehicleMovementOverlay =
        CVarDef.Create("rmc.vehicle.movement_overlay", false, CVar.CLIENT | CVar.ARCHIVE);

    public static readonly CVarDef<int> RMCVehicleRoundstartThresholdPlayers =
        CVarDef.Create("rmc.vehicle_roundstart_threshold_players", 200, CVar.REPLICATED | CVar.SERVER);
}
