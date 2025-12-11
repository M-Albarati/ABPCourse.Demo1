using ABPCourse.Demo1.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace ABPCourse.Demo1.Data.Categories
{
    public class CategoryDataSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Category, int> categoryrepository;

        public CategoryDataSeeder(IRepository<Category, int> categoryrepository)
        {
            this.categoryrepository = categoryrepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if(!await categoryrepository.AnyAsync())
            {
                var categories = new List<Category>()
            {
                new Category(id: 1,
                            nameAr:"أطعمة ومشروبات",
                            nameEn: "Food & Drinks",
                            descriptionAr:"جميع أنواع الأطعمة والمشروبات",
                            descriptionEn: "All food and drink categories"),
                new Category(id: 2,
                             nameAr:"مواد تنظيف",
                             nameEn: "Detergents",
                             descriptionAr:"المنظفات بأنواعها",
                             descriptionEn: "all materials used for cleaning"),
                new Category(id: 3,
                             nameAr:"عطور",
                             nameEn: "Fragrances",
                             descriptionAr:"العطور بأنواعها",
                             descriptionEn: "all perfumes and its sub-categories"),
                new Category(id: 4,
                             nameAr:"بلاستيك",
                             nameEn: "Plastic",
                             descriptionAr:"البلاستيك القابل للتدوير والغير قابل للتدوير",
                             descriptionEn: "all reusable and non-reusable plastic materials"),
            };

                await this.categoryrepository.InsertManyAsync(categories);
            }
            
        }
    }
}
