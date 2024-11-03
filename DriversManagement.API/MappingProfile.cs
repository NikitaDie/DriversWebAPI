using AutoMapper;
using DriversManagement.API.DTOs;
using DriversManagement.API.Models;

namespace DriversManagement.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Driver, DriverDto>()
            .ForMember(x => x.Name, cnf => cnf.MapFrom(x => x.FirstName))
            .ForMember(x => x.CategoryId, cnf => cnf.MapFrom(x => x.Category.Id))
            .ReverseMap();

        CreateMap<Vehicle, VehicleDto>()
            // .ForMember(dest => dest.DriverId, opt => opt.MapFrom(src => src.Driver.Id))
            .ReverseMap();
    }
}