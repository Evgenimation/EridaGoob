using Content.Shared._Erida.Language; // Erida edit
using Robust.Shared.Prototypes;

namespace Content.Shared._DV.Traits.Effects;

/// <summary>
/// Effect that adds components to the player entity.
/// Components are added without overwriting existing ones.
/// </summary>
public sealed partial class AddCompsEffect : BaseTraitEffect
{
    /// <summary>
    /// The components to add to the entity.
    /// </summary>
    [DataField(required: true)]
    public ComponentRegistry Components = new();

    // Erida start
    [DataField(required: false)]
    public bool RemoveExisting = false;
    // Erida end

    public override void Apply(TraitEffectContext ctx)
    {
        // Erida start - more languages
        foreach (var entry in Components.Values)
        {
            if (entry.Component is not LanguageOnSpawnComponent languages)
                continue;

            if (!ctx.EntMan.TryGetComponent<LanguageOnSpawnComponent>(ctx.Player, out var comp))
            {
                comp = new LanguageOnSpawnComponent();
                ctx.EntMan.AddComponent(ctx.Player, comp);
            }

            foreach (var language in languages.Languages)
            {
                if (!comp.Languages.Contains(language))
                    comp.Languages.Add(language);
            }
            ctx.EntMan.Dirty(ctx.Player, comp);
        }
        // Erida end

        ctx.EntMan.AddComponents(ctx.Player, Components, removeExisting: RemoveExisting); // Erida edit
    }
}
