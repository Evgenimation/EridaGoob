// erida edit start
using Content.Shared.Actions.Components;

namespace Content.Shared.Actions;

public abstract partial class SharedActionsSystem
{
    public void SetItemIconStyle(Entity<ActionComponent?> ent, ItemActionIconStyle style)
    {
        if (!_actionQuery.Resolve(ent, ref ent.Comp) || ent.Comp.ItemIconStyle == style)
            return;

        ent.Comp.ItemIconStyle = style;
        DirtyField(ent, ent.Comp, nameof(ActionComponent.ItemIconStyle));
    }
}
// erida edit end
