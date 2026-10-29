using System.Numerics;

namespace Content.Shared._RMC14.Weapons.Ranged;

[ByRefEvent]
public record struct RMCBeforeMuzzleFlashEvent(EntityUid Weapon, Vector2 Offset = default);

public record struct BeforeAttemptShootEvent(Robust.Shared.Map.EntityCoordinates Origin, System.Numerics.Vector2 Offset, bool Handled = false);
