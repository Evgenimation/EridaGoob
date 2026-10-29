using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Movement.Pulling.Systems;
using Robust.Shared.Map;

namespace Content.Shared._RMC14.Teleporter;

public sealed partial class SharedRMCTeleporterSystem : EntitySystem
{
    [Dependency] private PullingSystem _pulling = default!;
    [Dependency] private SharedTransformSystem _transform = default!;

    public void HandlePulling(EntityUid user, MapCoordinates teleport)
    {
        if (TryComp(user, out PullableComponent? otherPullable) &&
            otherPullable.Puller != null)
        {
            _pulling.TryStopPull(user, otherPullable, otherPullable.Puller.Value);
        }

        if (TryComp(user, out PullerComponent? puller) &&
            TryComp(puller.Pulling, out PullableComponent? pullable))
        {
            if (TryComp(puller.Pulling, out PullerComponent? otherPullingPuller) &&
                TryComp(otherPullingPuller.Pulling, out PullableComponent? otherPullingPullable))
            {
                _pulling.TryStopPull(otherPullingPuller.Pulling.Value, otherPullingPullable, puller.Pulling);
            }

            var pulling = puller.Pulling.Value;
            _pulling.TryStopPull(pulling, pullable, user);
            _transform.SetMapCoordinates(user, teleport);
            _transform.SetMapCoordinates(pulling, teleport);
            _pulling.TryStartPull(user, pulling);
        }
        else
        {
            _transform.SetMapCoordinates(user, teleport);
        }
    }
}
