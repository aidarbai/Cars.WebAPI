using Cars.COMMON.DTOs;
using Cars.COMMON.ViewModels.Cars;
using Cars.COMMON.ViewModels.Users;
using Cars.DAL.Models;
using System.Linq;

namespace Cars.BLL.Helpers
{
    public static class MappingHelper
    {
        public static void AutoMapperInit()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.ValidateInlineMaps = false;
                cfg.CreateMissingTypeMaps = true;
                cfg.CreateMap<CarImportDTO, Car>()
                    .ForMember(t => t.ExternalId, t => t.MapFrom(p => p.Id))
                    .ForMember(t => t.Id, t => t.Ignore())
                    .ForMember(t => t.BodyType, t => t.Ignore())
                    .ForMember(t => t.Color, t => t.Ignore())
                    .ForMember(t => t.Model, t => t.Ignore())
                    .ForMember(t => t.PhotoUrls, t => t.Ignore())
                    .ForMember(t => t.Mileage, t => t.MapFrom(p => p.MileageUnformatted))
                    .ForMember(t => t.Price, t => t.MapFrom(p => p.PriceUnformatted));
                cfg.CreateMap<Car, CarVm>()
                   .ForMember(t => t.Color, t => t.MapFrom(p => p.Color.Name))
                   .ForMember(t => t.Model, t => t.MapFrom(p => p.Model.Name))
                   .ForMember(t => t.Make, t => t.MapFrom(p => p.Model.Make.Name))
                   .ForMember(t => t.BodyType, t => t.MapFrom(p => p.BodyType.Name))
                   .ForMember(t => t.PhotoUrls, t => t.MapFrom(p => p.PhotoUrls.Select(o => o.Path).ToArray()));
                cfg.CreateMap<ApplicationUser, UserVm>()
                   .ForMember(t => t.Cars, t => t.MapFrom(p => p.Cars.Select(c => c.Id).ToArray()))
                   .ForMember(t => t.Roles, t => t.MapFrom(p => p.Roles.Select(c => c.Role.Name).ToArray()))
                   .ForMember(t => t.IsBanned, t => t.MapFrom(p => p.BannedTime != null));
                cfg.CreateMap<ApplicationUser, UserDTO>()
                   .ForMember(t => t.Roles, t => t.MapFrom(p => p.Roles.Select(c => c.Role.Name).ToArray()));
            });
        }
    }
}