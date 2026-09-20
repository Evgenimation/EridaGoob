// SPDX-FileCopyrightText: 2026 Lytheriia
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server._Erida.Administration;
using Content.Shared._Erida.Discord.Linking;
using Robust.Shared.Network;
using Robust.Shared.Timing;

namespace Content.Server._Erida.Discord.Linking;

public sealed partial class DiscordLinkingManager : IPostInjectInit
{
    [Dependency] private readonly INetManager _net = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly EridaServerApi _serverApi = default!;

    private readonly Dictionary<NetUserId, (Guid Code, TimeSpan Expires)> _activeCodes = new();
    private readonly HashSet<NetUserId> _pending = new();
    private static readonly TimeSpan CodeLifetime = TimeSpan.FromMinutes(3);

    void IPostInjectInit.PostInject()
    {
        _net.RegisterNetMessage<DiscordLinkingRequestMsg>(OnRequest);
        _net.RegisterNetMessage<DiscordLinkingCodeMsg>();
    }

    private async void OnRequest(DiscordLinkingRequestMsg message)
    {
        var channel = message.MsgChannel;
        var user = channel.UserId;
        var now = _timing.RealTime;

        if (_activeCodes.TryGetValue(user, out var active) && active.Expires > now)
        {
            _net.ServerSendMessage(new DiscordLinkingCodeMsg { Code = active.Code }, channel);
            return;
        }

        if (!_pending.Add(user))
            return;

        try
        {
            var code = Guid.NewGuid();

            if (!await _serverApi.RegisterLinkingCode(user.UserId, code))
            {
                _activeCodes.Remove(user);
                return;
            }

            _activeCodes[user] = (code, now + CodeLifetime);

            if (channel.IsConnected)
                _net.ServerSendMessage(new DiscordLinkingCodeMsg { Code = code }, channel);
        }
        finally
        {
            _pending.Remove(user);
        }
    }
}
