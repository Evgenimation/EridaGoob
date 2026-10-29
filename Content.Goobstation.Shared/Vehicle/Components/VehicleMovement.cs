// erida edit start
using System;
using Content.Goobstation.Shared.Vehicles;
using JetBrains.Annotations;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;

namespace Content.Shared.Vehicle.Components;

[Serializable, NetSerializable]
public enum VehicleVisuals : byte
{
    HasOperator,
    CanRun
}

[Serializable, NetSerializable]
public enum VehicleMovementKind : byte
{
    Standard,
    Grid
}

[ByRefEvent, UsedImplicitly]
public readonly record struct OnVehicleEnteredEvent(Entity<VehicleComponent> Vehicle, EntityUid Operator);

[ByRefEvent, UsedImplicitly]
public readonly record struct OnVehicleExitedEvent(Entity<VehicleComponent> Vehicle, EntityUid Operator);

[ByRefEvent, UsedImplicitly]
public readonly record struct VehicleOperatorSetEvent(EntityUid? NewOperator, EntityUid? OldOperator);

[ByRefEvent, UsedImplicitly]
public record struct VehicleCanRunEvent(Entity<VehicleComponent> Vehicle, bool CanRun = true);
// erida edit end
