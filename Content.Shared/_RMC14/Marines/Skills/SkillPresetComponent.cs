using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._RMC14.Marines.Skills;

// erida edit
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SkillsSystem))]
public sealed partial class SkillPresetComponent : Component
{
    [DataField, AutoNetworkedField, AlwaysPushInheritance]
    public Dictionary<EntProtoId<SkillDefinitionComponent>, int> Skills = new();
}
