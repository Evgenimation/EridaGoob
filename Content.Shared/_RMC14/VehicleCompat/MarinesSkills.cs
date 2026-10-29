using Robust.Shared.Prototypes;

namespace Content.Shared._RMC14.Marines.Skills;

public sealed class SkillsSystem : EntitySystem
{
    public bool HasSkills(Entity<SkillsComponent?> ent, SkillWhitelist whitelist)
    {
        return true;
    }

    public bool HasAllSkills(Entity<SkillsComponent?> ent, Dictionary<EntProtoId<SkillDefinitionComponent>, int> required)
    {
        return true;
    }

    public bool HasSkill(Entity<SkillsComponent?> ent, EntProtoId<SkillDefinitionComponent> skill, int required)
    {
        return true;
    }

    public float GetSkillDelayMultiplier(Entity<SkillsComponent?> user, EntProtoId<SkillDefinitionComponent> definition, float[]? multipliers = null)
    {
        return 1f;
    }
}
