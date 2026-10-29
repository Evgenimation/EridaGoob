using Robust.Shared.GameStates;

namespace Content.Shared.Vehicle.Components;

// erida edit fix
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(VehicleSystem))]
public sealed partial class VehicleOperatorComponent : Component
{
    [DataField, AutoNetworkedField]
    public EntityUid? Vehicle;
}
