// SPDX-FileCopyrightText: 2026 Lytheriia
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Erida.Discord.Linking;

namespace Content.Client._Erida.Discord.Linking;

public sealed class DiscordLinkingSystem : EntitySystem
{
    public bool IsVerified { get; private set; }

    public override void Initialize()
    {
        base.Initialize();

        SubscribeNetworkEvent<VerifyStatusResponse>(OnVerifyStatusResponse);
    }

    private void OnVerifyStatusResponse(VerifyStatusResponse msg)
    {
        IsVerified = msg.Verified;
    }

    public void RequestVerificationStatus()
    {
        RaiseNetworkEvent(new VerifyStatusRequest());
    }
}
