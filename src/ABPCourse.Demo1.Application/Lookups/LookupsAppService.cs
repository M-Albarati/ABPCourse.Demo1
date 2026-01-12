using ABPCourse.Demo1.Bases;
using ABPCourse.Demo1.Categories;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace ABPCourse.Demo1.Lookups
{
    public class LookupsAppService : BaseApplicationService
    {
        #region Fields
        private readonly IRepository<Category, int> _categoryRepository;
        private readonly IDistributedCache<List<CategoryDto>> _categoryCache;
        #endregion

        #region ctor
        public LookupsAppService(IRepository<Category, int> categoryRepository,
                                 IDistributedCache<List<CategoryDto>> categoryCache)
        {
            _categoryRepository = categoryRepository;
            _categoryCache = categoryCache;
        }
        #endregion

        #region Methods
        public async Task<List<CategoryDto>?> GetCategories()
        {
            //return await GetAllCategoriesFromDbAsync();
            //return await GetAllCategoriesFromCacheAsync();
            return await GetAllCategoriesFromRedisCacheAsync();
        }
        #endregion

        #region private Methods
        
        // get Data From DB
        private async Task<List<CategoryDto>> GetAllCategoriesFromDbAsync()
        {
            var categories = await _categoryRepository.GetListAsync();
            return ObjectMapper.Map<List<Category>, List<CategoryDto>>(categories);
        }

        // IN_Memory Caching
        private async Task<List<CategoryDto>?> GetAllCategoriesFromCacheAsync()
        {
            return await _categoryCache.GetOrAddAsync(
                $"ALL_CATEGORIES", //Cache Key
                async () => await GetAllCategoriesFromDbAsync(), //Cache Value
                () => new DistributedCacheEntryOptions //Cache Options
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddHours(1)
                }
            );
        }

        // Redis Caching
        private async Task<List<CategoryDto>?> GetAllCategoriesFromRedisCacheAsync()
        {
            return await _categoryCache.GetOrAddAsync(
                $"ALL_CATEGORIES", //Cache Key
                async () => await GetAllCategoriesFromDbAsync() //Cache Value
            );
        }
        #endregion
    }
}
