using AutoMapper;
using CustomerService.Domain.CustomerAggregate;

namespace CustomerService.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Customer, DTO.Write.Customer>();
        CreateMap<Customer, DTO.Write.Customer>().ReverseMap();
        CreateMap<Address, DTO.Write.Address>();
        CreateMap<Address, DTO.Write.Address>().ReverseMap();

        CreateMap<Customer, DTO.Read.CustomerView>().IncludeMembers(c => c.ShippingAddress);
        CreateMap<Customer, DTO.Read.CustomerView>().IncludeMembers(c => c.ShippingAddress).ReverseMap();
        CreateMap<Address, DTO.Read.CustomerView>();
        CreateMap<Address, DTO.Read.CustomerView>().ReverseMap();
    }
}