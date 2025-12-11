using ABPCourse.Demo1.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace ABPCourse.Demo1.Products
{
    public class ProductAppService : BaseApplicationService,IProductAppService
    {
        #region fields
        private readonly IRepository<Product, int> productRepository;
        #endregion

        #region ctor
        public ProductAppService(IRepository<Product, int> productRepository)
        {
            this.productRepository = productRepository;
        }
        #endregion

        #region IProductAppService
        public async Task<ProductDto> CreateProductAsync(CreateUpdateProductDto input)
        {
            var product = ObjectMapper.Map<CreateUpdateProductDto, Product>(input);
            var inserted = await productRepository.InsertAsync(product,autoSave:true);
            var response = ObjectMapper.Map<Product, ProductDto>(inserted);
            return response;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var existproduct = await productRepository.GetAsync(id);
            if (existproduct == null) { return false; }
            await productRepository.DeleteAsync(id);
            return true;
        }

        public async Task<PagedResultDto<ProductDto>> GetListAsync(GetProductListDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(Product.Id);
            }
            var products = await productRepository
                           .WithDetailsAsync(product => product.Category)
                           .Result
                           .AsQueryable()
                           .WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                                product => product.NameAr.Contains(input.Filter) ||
                                           product.NameEn.Contains(input.Filter))
                           .Skip(input.SkipCount)
                           .Take(input.MaxResultCount)
                           .OrderBy(input.Sorting)  ////using System.Linq.Dynamic.Core;
                           .ToListAsync();          ////using Microsoft.EntityFrameworkCore;
            var totalcount = input.Filter == null
                ? await productRepository.CountAsync()
                : await productRepository.CountAsync(product => product.NameAr.Contains(input.Filter) ||
                                                                product.NameEn.Contains(input.Filter));
            return new PagedResultDto<ProductDto>
            {
                TotalCount = totalcount,
                Items = ObjectMapper.Map<List<Product>, List<ProductDto>>(products)
            };
        }

        public async Task<ProductDto> GetProductAsync(int id)
        {
            var product = await productRepository
                .WithDetailsAsync(x => x.Category)
                .Result
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (product == null)
            {
                //throw new Exception("Product Not Found");
                throw new ProductNotFoundException(id);  // new exception >> ProductNotFoundException
            }
            var response = ObjectMapper.Map<Product, ProductDto>(product);
            return response;
        }

        public async Task<ProductDto> UpdateProductAsync(CreateUpdateProductDto input)
        {
            var existed = await productRepository.GetAsync(input.Id);
            if (existed == null)
            {
                //throw new Exception("Product Not Found");
                throw new ProductNotFoundException(input.Id);  // new exception >> ProductNotFoundException
            }
            var mapped = ObjectMapper.Map<CreateUpdateProductDto, Product>(input, existed);
                var updated = await productRepository.UpdateAsync(mapped, autoSave:true);
                var response = ObjectMapper.Map<Product, ProductDto>(updated);
                return response;
        }
        #endregion
    }
}
