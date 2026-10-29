using Content.Goobstation.Shared.Overlays; // erida edit
using Content.Shared.Actions;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Vehicle.Components;
using Content.Goobstation.Shared.Vehicles;
using Robust.Shared.Network;

namespace Content.Shared._RMC14.Vehicle;

public sealed partial class VehicleNightVisionModuleSystem : EntitySystem
{
    [Dependency] private ItemSlotsSystem _itemSlots = default!;
    [Dependency] private INetManager _net = default!;
    [Dependency] private SharedActionsSystem _actions = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<HardpointSlotsChangedEvent>(OnHardpointsChanged);
    }


    public void OnOperatorEntered(EntityUid operatorUid, EntityUid vehicle)
    {
        if (_net.IsClient)
            return;

        UpdateOperator(operatorUid, vehicle);
    }


    public void OnOperatorExited(EntityUid operatorUid)
    {
        if (_net.IsClient)
            return;

        RemComp<NightVisionComponent>(operatorUid);
    }

    private void OnHardpointsChanged(HardpointSlotsChangedEvent args)
    {
        if (_net.IsClient)
            return;

        if (!TryComp(args.Vehicle, out VehicleComponent? vehicle) || vehicle.Operator is not { } op)
            return;

        UpdateOperator(op, args.Vehicle);
    }

    private void UpdateOperator(EntityUid operatorUid, EntityUid vehicle)
    {
        if (VehicleHasNightVision(vehicle))
        {
            var nv = EnsureComp<NightVisionComponent>(operatorUid);
            nv.IsEquipment = false;
            if (nv.ToggleActionEntity == null && nv.ToggleAction != null)
            {
                _actions.AddAction(operatorUid, ref nv.ToggleActionEntity, nv.ToggleAction);
                Dirty(operatorUid, nv);
            }
        }
        else
        {
            RemComp<NightVisionComponent>(operatorUid);
        }
    }





    public bool TryToggleOccupantNightVision(EntityUid occupant, EntityUid vehicle)
    {
        if (_net.IsClient)
            return false;

        if (!VehicleHasNightVision(vehicle))
            return false;

        if (HasComp<NightVisionComponent>(occupant))
        {
            RemComp<NightVisionComponent>(occupant);
        }
        else
        {
            var nv = EnsureComp<NightVisionComponent>(occupant);
            nv.IsEquipment = false;
            nv.IsActive = true;
            Dirty(occupant, nv);
        }

        return true;
    }


    public void OnGunnerExited(EntityUid gunnerUid)
    {
        if (_net.IsClient)
            return;

        RemComp<NightVisionComponent>(gunnerUid);
    }

    private bool VehicleHasNightVision(EntityUid vehicle)
    {
        if (!TryComp(vehicle, out HardpointSlotsComponent? hardpoints) ||
            !TryComp(vehicle, out ItemSlotsComponent? itemSlots))
            return false;

        foreach (var slot in hardpoints.Slots)
        {
            if (string.IsNullOrWhiteSpace(slot.Id))
                continue;

            if (!_itemSlots.TryGetSlot(vehicle, slot.Id, out var itemSlot, itemSlots) || !itemSlot.HasItem)
                continue;

            if (HasComp<VehicleNightVisionModuleComponent>(itemSlot.Item!.Value))
                return true;
        }

        return false;
    }
}
