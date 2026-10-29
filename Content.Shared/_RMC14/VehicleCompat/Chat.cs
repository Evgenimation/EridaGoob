using Content.Shared.Chat;
using Robust.Shared.Network;

namespace Content.Shared._RMC14.Chat;

public sealed partial class SharedCMChatSystem : EntitySystem
{
    public void ChatMessageToOne(
        ChatChannel channel,
        string message,
        string wrappedMessage,
        EntityUid source,
        bool hideChat,
        INetChannel client,
        Color? colorOverride = null,
        bool recordReplay = false,
        string? audioPath = null,
        float audioVolume = 0,
        NetUserId? author = null)
    {
    }
}
