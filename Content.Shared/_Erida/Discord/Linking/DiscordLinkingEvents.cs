// SPDX-FileCopyrightText: 2026 Lytheriia
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Shared._Erida.Discord.Linking;

[Serializable, NetSerializable]
public sealed class VerifyStatusRequest : EntityEventArgs
{
}

[Serializable, NetSerializable]
public sealed class VerifyStatusResponse : EntityEventArgs
{
    public bool Verified;
}
