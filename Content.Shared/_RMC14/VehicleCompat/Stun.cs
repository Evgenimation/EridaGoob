namespace Content.Shared._RMC14.Stun;

public sealed class RMCSizeStunSystem : EntitySystem
{
    public bool TryGetSize(EntityUid ent, out RMCSizes size)
    {
        size = default;
        if (!TryComp(ent, out RMCSizeComponent? sizeComp))
            return false;

        size = sizeComp.Size;
        return true;
    }
}
