using AutoMapper;
using DataAbstraction;

namespace AutoMapperLib
{
    public class BankCardMappingProfile : Profile
    {
        public  BankCardMappingProfile()            
        {
            // with auto field equals set = when all name are the same
            CreateMap<CardEntityToPost, CardEntity>();

            CreateMap<CardEntityToPostAutoField, CardEntity>();

            /*
            // with manual field equals set = in case of different names
            CreateMap<CardEntityToPost, CardEntity>()// map from CardEntityToPost to CardEntity
                .ForMember(d => d.HolderName, source => source.MapFrom(s => s.HolderName))
                .ForMember(d => d.Number, source => source.MapFrom(s => s.Number))
                .ForMember(d => d.CVVCode, source => source.MapFrom(s => s.CVVCode))
                .ForMember(d => d.Type, source => source.MapFrom(s => s.Type))
                .ForMember(d => d.System, source => source.MapFrom(s => s.System))
                .ForMember(d => d.IsBlocked, source => source.MapFrom(s => s.IsBlocked));
            */
        }
    }
}
