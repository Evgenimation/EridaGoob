using Content.Shared.Whitelist;
using Robust.Shared.GameStates;
using Content.Shared.Vehicle.Components;
using Content.Goobstation.Shared.Vehicles;

namespace Content.Shared.Vehicle.Components;

// erida edit
[RegisterComponent, NetworkedComponent]
[Access(typeof(VehicleSystem))]
public sealed partial class GenericKeyedVehicleComponent : Component
{

    [DataField(required: true)]
    public string ContainerId;

    [DataField(required: true)]
    public EntityWhitelist KeyWhitelist = new();

    [DataField]
    public bool PreventInvalidInsertion = true;
}
