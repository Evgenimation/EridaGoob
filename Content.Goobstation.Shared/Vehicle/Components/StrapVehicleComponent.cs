using Robust.Shared.GameStates;

namespace Content.Shared.Vehicle.Components;

// erida edit
[RegisterComponent, NetworkedComponent]
[Access(typeof(VehicleSystem))]
public sealed partial class StrapVehicleComponent : Component;
