using Content.Server.Atmos.EntitySystems;
using Content.Shared._RMC14.Vehicle;
using Content.Shared.Atmos;
using Robust.Shared.Map.Components;

namespace Content.Server._RMC14.Vehicle;

// erida edit fix
public sealed partial class VehicleInteriorAtmosSystem : EntitySystem
{
    [Dependency] private AtmosphereSystem _atmos = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<VehicleInteriorLinkComponent, ComponentStartup>(OnInteriorLinked);
    }

    private void OnInteriorLinked(Entity<VehicleInteriorLinkComponent> ent, ref ComponentStartup args)
    {
        if (!HasComp<MapComponent>(ent.Owner))
            return;

        var air = new GasMixture(Atmospherics.CellVolume)
        {
            Temperature = Atmospherics.T20C,
        };
        air.AdjustMoles(Gas.Oxygen, Atmospherics.OxygenMolesStandard);
        air.AdjustMoles(Gas.Nitrogen, Atmospherics.NitrogenMolesStandard);

        _atmos.SetMapAtmosphere(ent.Owner, false, air);
    }
}
