using System.Numerics;
using Content.Shared.Interaction;
using Content.Shared.DoAfter;
using RmcDrawDepth = Content.Shared.DrawDepth.DrawDepth;
using Robust.Shared.GameStates;
using Robust.Shared.Maths;
using Robust.Shared.Serialization;

namespace Content.Shared.Vehicle.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(Content.Shared.Vehicle.VehicleRideSurfaceSystem), Other = AccessPermissions.ReadWrite)]
public sealed partial class VehicleRideSurfaceComponent : Component
{
    [DataField(required: true), AutoNetworkedField]
    public List<Box2> Bounds = new();

    [DataField, AutoNetworkedField]
    public List<Box2> ClimbBounds = new();

    [DataField, AutoNetworkedField]
    public float ClimbRange = SharedInteractionSystem.InteractionRange;

    [DataField, AutoNetworkedField]
    public TimeSpan ClimbDelay = TimeSpan.FromSeconds(1);

    [DataField, AutoNetworkedField]
    public TimeSpan ClimbDownDelay = TimeSpan.FromSeconds(1);

    [DataField, AutoNetworkedField]
    public int RiderDrawDepth = (int) RmcDrawDepth.OverMobs + 2;

    [DataField, AutoNetworkedField]
    public float SoftBorderPadding = 0.75f;

    [DataField, AutoNetworkedField]
    public TimeSpan EdgeClimbDownGrace = TimeSpan.FromSeconds(0.15);

    [DataField, AutoNetworkedField]
    public TimeSpan FallOffKnockdown = TimeSpan.FromSeconds(2.5);

    [DataField, AutoNetworkedField]
    public TimeSpan FallOffStun = TimeSpan.FromSeconds(2.5);

    [DataField, AutoNetworkedField]
    public float MaxCarryDistance = 3f;

    [DataField, AutoNetworkedField]
    public bool CarryBuckled;

}

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
[Access(typeof(Content.Shared.Vehicle.VehicleRideSurfaceSystem), Other = AccessPermissions.ReadWrite)]
public sealed partial class VehicleRideSurfaceRiderComponent : Component
{
    [AutoNetworkedField]
    public EntityUid Vehicle = EntityUid.Invalid;

    [AutoNetworkedField]
    public Vector2 LocalPosition;

    public DoAfterId? ClimbDownDoAfter;
    public bool ClimbDownFromEdge;
    public bool ClimbDownCompleting;
    public TimeSpan? EdgeClimbDownAt;
}

[Serializable, NetSerializable]
public sealed partial class VehicleRideSurfaceClimbDoAfterEvent : SimpleDoAfterEvent;

[Serializable, NetSerializable]
public sealed partial class VehicleRideSurfaceClimbDownDoAfterEvent : DoAfterEvent
{
    [DataField]
    public bool FromEdge;

    public VehicleRideSurfaceClimbDownDoAfterEvent()
    {
    }

    public VehicleRideSurfaceClimbDownDoAfterEvent(bool fromEdge)
    {
        FromEdge = fromEdge;
    }

    public override DoAfterEvent Clone()
    {
        return new VehicleRideSurfaceClimbDownDoAfterEvent(FromEdge);
    }
}
