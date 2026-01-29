using ABPCourse.Demo1.Localization;
using FluentValidation;
using Microsoft.Extensions.Localization;
namespace ABPCourse.Demo1.Products
{
    public class CreateUpdateProductValidation : AbstractValidator<CreateUpdateProductDto>
    {
        private readonly IStringLocalizerFactory stringLocalizerFactory;
        private IStringLocalizer PrdLoc => stringLocalizerFactory.Create(typeof(ProductsResource));
        private IStringLocalizer Loc => stringLocalizerFactory.Create(typeof(Demo1Resource));

        public CreateUpdateProductValidation(IStringLocalizerFactory stringLocalizerFactory)
        {
            // stringLocalizer
            this.stringLocalizerFactory = stringLocalizerFactory;

            RuleFor(x => x.NameAr)
                .NotNull()  // ليس فراغ
                .NotEmpty() // غير موجود
                .MaximumLength(Demo1Consts.GeneralTextMaxLength)
                .WithErrorCode(Demo1DomainErrorCodes.INVAIL_PRODUCT_NAME_ARABIC)
                .WithMessage(PrdLoc["Products:InvalidProductNameAr"]);
            RuleFor(x => x.NameEn)
                .NotNull()
                .NotEmpty()
                .MaximumLength(Demo1Consts.GeneralTextMaxLength)
                .WithErrorCode(Demo1DomainErrorCodes.INVAIL_PRODUCT_NAME_ENGLISH)
                .WithMessage(PrdLoc["Products:InvalidProductNameEn"]);
            RuleFor(x => x.DescriptionAr)
                .NotEmpty()
                .MaximumLength(Demo1Consts.DescriptionTextMaxLength)
                .WithErrorCode(Demo1DomainErrorCodes.INVAIL_PRODUCT_Description_ARABIC)
                .WithMessage(PrdLoc["Products:InvalidProductDescriptionAr"]);
            RuleFor(x => x.DescriptionAr)
                .NotEmpty()
                .MaximumLength(Demo1Consts.DescriptionTextMaxLength)
                .WithErrorCode(Demo1DomainErrorCodes.INVAIL_PRODUCT_Description_ENGLISH)
                .WithMessage(PrdLoc["Products:InvalidProductDescriptionEn"]);
            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithErrorCode(Demo1DomainErrorCodes.INVAIL_PRODUCT_CATEGORY)
                .WithMessage(PrdLoc["Products:InvalidProductCategory"]);
        }
    }
}
