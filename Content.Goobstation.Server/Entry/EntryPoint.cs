
using Content.Goobstation.Server.IoC;
using Content.Goobstation.Server.VoiceChat; // erida edit
using Content.Goobstation.Common.JoinQueue;
using Content.Goobstation.Common.ServerCurrency;
using Robust.Shared.ContentPack;
using Robust.Shared.Timing;

namespace Content.Goobstation.Server.Entry;

public sealed class EntryPoint : GameServer
{
    private VoiceChatManager _voiceManager = default!; // erida edit
    private VoiceLogManager _voiceLogs = default!; // erida edit
    private ICommonCurrencyManager _curr = default!;
    private IJoinQueueManager _joinQueue = default!;

    public override void Init()
    {
        base.Init();

        ServerGoobContentIoC.Register();

        IoCManager.BuildGraph();

        _voiceManager = IoCManager.Resolve<VoiceChatManager>(); // erida edit
        _voiceManager.Initialize(); // erida edit

        _voiceLogs = IoCManager.Resolve<VoiceLogManager>(); // erida edit
        _voiceLogs.Initialize(); // erida edit

        _joinQueue = IoCManager.Resolve<IJoinQueueManager>();
        _joinQueue.Initialize();

        _curr = IoCManager.Resolve<ICommonCurrencyManager>();
        _curr.Initialize();
    }

    public override void Update(ModUpdateLevel level, FrameEventArgs frameEventArgs)
    {
        base.Update(level, frameEventArgs);

        switch (level)
        {
            case ModUpdateLevel.PreEngine:
                _joinQueue.Update(frameEventArgs.DeltaSeconds);
                break;
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        _curr.Shutdown();
        _voiceManager.Shutdown();
        _voiceLogs.Shutdown(); // erida edit
    }
}
