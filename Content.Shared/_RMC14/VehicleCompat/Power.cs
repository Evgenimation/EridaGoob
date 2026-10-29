using Robust.Shared.GameStates;

namespace Content.Shared._RMC14.Power;

[RegisterComponent, NetworkedComponent]
public sealed partial class RMCPowerReceiverComponent : Component;

public sealed class SharedRMCPowerSystem : EntitySystem
{
    public bool IsPowered(EntityUid ent)
    {
        return false;
    }
}
