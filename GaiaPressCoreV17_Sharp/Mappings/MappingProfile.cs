using AutoMapper;
using GaiaPressCoreV17.Models;
using GaiaPressCoreV17.Models.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GaiaPressCoreV17.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Chantier
        CreateMap<Chantier, ChantierDto>()
            .ForMember(dest => dest.NombreForages, opt => opt.MapFrom(src => src.Forages != null ? src.Forages.Count : 0));
        CreateMap<ChantierCreateDto, Chantier>();
        CreateMap<ChantierUpdateDto, Chantier>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Forage
        CreateMap<Forage, ForageDto>()
            .ForMember(dest => dest.NomChantier, opt => opt.MapFrom(src => src.Chantier != null ? src.Chantier.Nom : null))
            .ForMember(dest => dest.CodeSol, opt => opt.MapFrom(src => src.Sol != null ? src.Sol.Code : null));
        CreateMap<ForageCreateDto, Forage>();
        CreateMap<ForageUpdateDto, Forage>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Sol
        CreateMap<Sol, SolDto>();
        CreateMap<SolCreateDto, Sol>();
        CreateMap<SolUpdateDto, Sol>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // EssaiForage
        CreateMap<EssaiForage, EssaiForageDto>()
            .ForMember(dest => dest.NomForage, opt => opt.MapFrom(src => src.Forage != null ? src.Forage.Nom : null));
        CreateMap<EssaiForageCreateDto, EssaiForage>();
        CreateMap<EssaiForageUpdateDto, EssaiForage>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // MesureEssaiForage
        CreateMap<MesureEssaiForage, MesureEssaiForageDto>();
        CreateMap<MesureEssaiForageCreateDto, MesureEssaiForage>();

        // EssaiExpansion
        CreateMap<EssaiExpansion, EssaiExpansionDto>()
            .ForMember(dest => dest.NomForage, opt => opt.MapFrom(src => src.Forage != null ? src.Forage.Nom : null))
            .ForMember(dest => dest.NumeroSerieProbe, opt => opt.MapFrom(src => src.Probe != null ? src.Probe.NumeroSerie : null));
        CreateMap<EssaiExpansionCreateDto, EssaiExpansion>();
        CreateMap<EssaiExpansionUpdateDto, EssaiExpansion>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // MesureExpansion
        CreateMap<MesureExpansion, MesureExpansionDto>();
        CreateMap<MesureExpansionCreateDto, MesureExpansion>();

        // EssaiResistance
        CreateMap<EssaiResistance, EssaiResistanceDto>()
            .ForMember(dest => dest.NomForage, opt => opt.MapFrom(src => src.Forage != null ? src.Forage.Nom : null));
        CreateMap<EssaiResistanceCreateDto, EssaiResistance>();
        CreateMap<EssaiResistanceUpdateDto, EssaiResistance>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // MesureResistance
        CreateMap<MesureResistance, MesureResistanceDto>();
        CreateMap<MesureResistanceCreateDto, MesureResistance>();

        // Probe
        CreateMap<Probe, ProbeDto>()
            .ForMember(dest => dest.DernierEtalonnage, opt => opt.MapFrom(src =>
                src.MesuresEtalonnage != null && src.MesuresEtalonnage.Any()
                    ? src.MesuresEtalonnage.Max(m => m.DateEtalonnage)
                    : (DateTime?)null));
        CreateMap<ProbeCreateDto, Probe>();
        CreateMap<ProbeUpdateDto, Probe>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // MesuresEtalonnage
        CreateMap<MesuresEtalonnage, MesuresEtalonnageDto>()
            .ForMember(dest => dest.NumeroSerieProbe, opt => opt.MapFrom(src => src.Probe != null ? src.Probe.NumeroSerie : null));
        CreateMap<MesuresEtalonnageCreateDto, MesuresEtalonnage>();
    }
}