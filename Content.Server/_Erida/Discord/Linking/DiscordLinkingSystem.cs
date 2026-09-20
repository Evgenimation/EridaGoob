// SPDX-FileCopyrightText: 2026 Lytheriia
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Threading.Tasks;
using Content.Server._Erida.Administration;
using Content.Shared._Erida.Discord.Linking;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Server._Erida.Discord.Linking;

public sealed class DiscordLinkingSystem : EntitySystem
{
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly EridaServerApi _eridaServerApi = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    private Dictionary<NetUserId, LinkStatus> _cachedPlayers = new();
    private readonly Dictionary<NetUserId, TimeSpan> _lastCheck = new();
    private static readonly TimeSpan RecheckDelay = TimeSpan.FromSeconds(10);

    public override void Initialize()
    {
        SubscribeNetworkEvent<VerifyStatusRequest>(OnVerifyStatusRequest);
        _playerManager.PlayerStatusChanged += OnPlayerStatusChanged;

        base.Initialize();
    }

    private async void OnVerifyStatusRequest(VerifyStatusRequest msg, EntitySessionEventArgs args)
    {
        var session = args.SenderSession;

        if (!_cachedPlayers.TryGetValue(session.UserId, out var status) || !status.Verified)
        {
            var now = _timing.RealTime;
            if (!_lastCheck.TryGetValue(session.UserId, out var last) || last + RecheckDelay <= now)
            {
                _lastCheck[session.UserId] = now;
                await RefreshPlayer(session);
                _cachedPlayers.TryGetValue(session.UserId, out status);
            }
        }

        RaiseNetworkEvent(new VerifyStatusResponse { Verified = status?.Verified ?? false }, session);
    }

    private async Task RefreshPlayer(ICommonSession session)
    {
        var status = await _eridaServerApi.GetLinkedUserData(session.UserId);
        if (status != null)
            _cachedPlayers[session.UserId] = status;
    }

    private async void OnPlayerStatusChanged(object? sender, SessionStatusEventArgs args)
    {
        switch (args.NewStatus)
        {
            case SessionStatus.Connected:
                await RefreshPlayer(args.Session);
                break;
            case SessionStatus.Disconnected:
                _cachedPlayers.Remove(args.Session.UserId);
                _lastCheck.Remove(args.Session.UserId);
                break;
        }
    }

    public void ForceUpdateUserData(NetUserId userId)
    {
        _cachedPlayers.Remove(userId);
        if (_playerManager.TryGetSessionById(userId, out var session))
            _ = RefreshPlayer(session);
    }
}
