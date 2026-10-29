using Robust.Shared.GameStates;

namespace Content.Shared._RMC14.Vehicle;

/// <summary>
/// so the inherited faction is cleared again when the driver leaves. This lets hostile turrets
/// treat a crewed vehicle as an enemy (and friendly turrets leave it alone), while parked/empty
/// vehicles carry no faction and are ignored.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class VehicleInheritedFactionComponent : Component;
