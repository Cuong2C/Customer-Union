using AutoMapper;
using Customer_Union.Models;

namespace Customer_Union.Configuration;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<CustomerRequest, Customer>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedClientSourceCode, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedClientSourceCode, opt => opt.Ignore());

        CreateMap<Customer, CustomerResponse>();
        CreateMap<Customer, CustomerHistory>();
    }
}
