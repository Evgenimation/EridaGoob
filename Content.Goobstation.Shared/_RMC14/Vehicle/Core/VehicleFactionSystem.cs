using Content.Shared.NPC.Components;
using Content.Shared.NPC.Systems;
using Robust.Shared.Network;

namespace Content.Shared._RMC14.Vehicle;

public sealed partial class VehicleFactionSystem : EntitySystem
{
    [Dependency] private INetManager _net = default!;
    [Dependency] private NpcFactionSystem _faction = default!;

    public void OnOperatorEntered(EntityUid operatorUid, EntityUid vehicle)
    {
        if (_net.IsClient)
            return;

        if (HasComp<NpcFactionMemberComponent>(vehicle))
            return;

        if (!TryComp(operatorUid, out NpcFactionMemberComponent? opFactions) || opFactions.Factions.Count == 0)
            return;

        var vehFactions = EnsureComp<NpcFactionMemberComponent>(vehicle);
        _faction.AddFactions((vehicle, vehFactions), opFactions.Factions);
        EnsureComp<VehicleInheritedFactionComponent>(vehicle);
    }

    public void OnOperatorExited(EntityUid vehicle)
    {
        if (_net.IsClient)
            return;

        if (!HasComp<VehicleInheritedFactionComponent>(vehicle))
            return;

        RemComp<VehicleInheritedFactionComponent>(vehicle);
        RemComp<NpcFactionMemberComponent>(vehicle);
    }
}
