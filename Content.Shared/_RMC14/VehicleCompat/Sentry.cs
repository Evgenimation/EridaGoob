namespace Content.Shared._RMC14.Sentry;

public sealed class SharedSentryTargetingSystem : EntitySystem
{
    public void ApplyDeployerFactions(EntityUid sentry, EntityUid deployer)
    {
    }

    public bool IsValidTarget(Entity<SentryTargetingComponent> sentry, EntityUid target)
    {
        return false;
    }

    public IEnumerable<EntityUid> GetNearbyIffHostiles(Entity<SentryTargetingComponent> ent, float range)
    {
        yield break;
    }
}
