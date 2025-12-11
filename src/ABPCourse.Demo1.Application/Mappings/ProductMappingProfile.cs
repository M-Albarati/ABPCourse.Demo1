using ABPCourse.Demo1.Products;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABPCourse.Demo1.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
        //Read
        CreateMap<Product, ProductDto>();
        // Create & Update
        CreateMap<CreateUpdateProductDto, Product>();
        }
    }
}
