// SPDX-FileCopyrightText: 2026 Lytheriia
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Client.Message;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controllers;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Timing;
using Content.Shared._Erida.Discord.Linking;
using Content.Shared._Erida.CCVar;

namespace Content.Client._Erida.Discord.Linking;

public sealed class LinkAccountUIController : UIController
{
    [Dependency] private readonly IClipboardManager _clipboard = default!;
    [Dependency] private readonly IConfigurationManager _config = default!;
    [Dependency] private readonly DiscordLinkingManager _linkingManager = default!;
    [Dependency] private readonly INetManager _net = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IUriOpener _uriOpener = default!;
    [Dependency] private readonly IEntitySystemManager _entitySystemManager = default!;

    private DiscordLinkingSystem LinkingSystem => _entitySystemManager.GetEntitySystem<DiscordLinkingSystem>();
    private DiscordLinkingWindow? _window;
    private TimeSpan _disableUntil;
    private Guid _code;
    private TimeSpan _codeExpires;
    private static readonly TimeSpan CodeLifetime = TimeSpan.FromMinutes(3);

    private bool HasValidCode => _code != default && _timing.RealTime < _codeExpires;

    public override void Initialize()
    {
        _linkingManager.CodeReceived += OnCode;
    }

    private void OnCode(Guid code)
    {
        _code = code;
        _codeExpires = _timing.RealTime + CodeLifetime;

        if (_window == null)
            return;

        _window.CopyButton.Disabled = false;
    }

    public void ToggleWindow()
    {
        if (_window == null)
        {
            _window = new DiscordLinkingWindow();
            _window.OnClose += () => _window = null;
            _window.Label.SetMarkupPermissive($"{Loc.GetString("rmc-ui-link-discord-account-text")}");

            if (LinkingSystem.IsVerified)
                _window.Label.SetMarkupPermissive($"{Loc.GetString("rmc-ui-link-discord-account-already-linked")}\n\n{Loc.GetString("rmc-ui-link-discord-account-text")}");

            _window.CopyButton.OnPressed += _ =>
            {
                _clipboard.SetText(_code.ToString());
                _window.CopyButton.Text = Loc.GetString("rmc-ui-link-discord-account-copied");
                _window.CopyButton.Disabled = true;
                _disableUntil = _timing.RealTime.Add(TimeSpan.FromSeconds(9));
            };

            var messageLink = _config.GetCVar(ECCVars.DiscordAccountLinkingMessageLink);
            if (string.IsNullOrEmpty(messageLink))
            {
                _window.LinkButton.Visible = false;
                _window.CopyButton.RemoveStyleClass("OpenRight");
            }
            else
            {
                _window.LinkButton.Visible = true;
                _window.LinkButton.OnPressed += _ => _uriOpener.OpenUri(messageLink);
                _window.CopyButton.AddStyleClass("OpenRight");
            }

            _window.OpenCentered();

            if (HasValidCode)
            {
                _window.CopyButton.Disabled = false;
            }
            else
            {
                _window.CopyButton.Disabled = true;
                _net.ClientSendMessage(new DiscordLinkingRequestMsg());
            }
            return;
        }

        _window.Close();
        _window = null;
    }

    public override void FrameUpdate(FrameEventArgs args)
    {
        if (_window == null)
            return;

        var time = _timing.RealTime;
        if (_disableUntil != default && time > _disableUntil)
        {
            _disableUntil = default;
            _window.CopyButton.Text = Loc.GetString("rmc-ui-link-discord-account-copy");
            _window.CopyButton.Disabled = false;
        }
    }
}
