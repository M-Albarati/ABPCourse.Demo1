using ABPCourse.Demo1.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ABPCourse.Demo1.Configuration
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ConfigureByConvention();
            builder.Property(x => x.Id).ValueGeneratedNever(); //للتحكم في رقم الفئة وادخالة بشكل يدوي عند اضافة الفئات

            builder.Property(x => x.NameAr).HasMaxLength(Demo1Consts.GeneralTextMaxLength);
            builder.Property(x => x.NameEn).HasMaxLength(Demo1Consts.GeneralTextMaxLength);
            builder.Property(x => x.DescriptionAr).HasMaxLength(Demo1Consts.DescriptionTextMaxLength);
            builder.Property(x => x.DescriptionEn).HasMaxLength(Demo1Consts.DescriptionTextMaxLength);

            builder.ToTable("Categories");
        }
    }
}
