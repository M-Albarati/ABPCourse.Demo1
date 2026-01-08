using ABPCourse.Demo1.Bases;
using ABPCourse.Demo1.Permissions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;

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
        [Authorize(Demo1Permissions.CreateEditProductPermission)]
        public async Task<ProductDto> CreateProductAsync(CreateUpdateProductDto input)
        {
            //Validation
            var ValidateResult = new CreateUpdateProductValidation().Validate(input);
            if (!ValidateResult.IsValid)
            {
                var exception = GetValidationException(ValidateResult);
                throw exception;
            }

            var product = ObjectMapper.Map<CreateUpdateProductDto, Product>(input);
            var inserted = await productRepository.InsertAsync(product,autoSave:true);
            var response = ObjectMapper.Map<Product, ProductDto>(inserted);
            return response;
        }

        [Authorize(Demo1Permissions.DeleteProductPermission)]
        public async Task<bool> DeleteProductAsync(int id)
        {
            var existproduct = await productRepository.GetAsync(id);
            if (existproduct == null) { return false; }
            await productRepository.DeleteAsync(id);
            return true;
        }

        [Authorize(Demo1Permissions.ListProductPermission)]
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
                           .WhereIf(
                                !input.Filter.IsNullOrWhiteSpace(),
                                product => product.NameAr.Contains(input.Filter) ||
                                           product.NameEn.Contains(input.Filter)
                            )
                           .WhereIf(
                                input.CategoryId.HasValue,
                                product => product.CategoryId == input.CategoryId
                            )
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

        [Authorize(Demo1Permissions.GetProductPermission)]
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

        [Authorize(Demo1Permissions.CreateEditProductPermission)]
        public async Task<ProductDto> UpdateProductAsync(CreateUpdateProductDto input)
        {
            //Validation
            var ValidateResult = new CreateUpdateProductValidation().Validate(input);
            if (!ValidateResult.IsValid)
            {
                var exception = GetValidationException(ValidateResult);
                throw exception;
            }

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

        public async Task<bool> TestComplexPermissions()
        {
            var result = await AuthorizationService.AuthorizeAsync(Demo1Permissions.CreateEditProductPermission);
            if(result.Succeeded == false)
            {
                throw new AbpAuthorizationException("you don't have permission for this action");
            }
            return true;
        }
        #endregion IProductAppService
    }
}
