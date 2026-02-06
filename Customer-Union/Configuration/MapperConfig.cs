namespace Customer_Union.Configuration;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<ClientSourceRequest, ClientSource>();
        CreateMap<ClientSource, ClientSourceResponse>();

        CreateMap<CustomerRequest, Customer>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedClientSourceCode, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedClientSourceCode, opt => opt.Ignore());

        CreateMap<Customer, CustomerResponse>();
        CreateMap<Customer, CustomerHistory>();
    }
}
