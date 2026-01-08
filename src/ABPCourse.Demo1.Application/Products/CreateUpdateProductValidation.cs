using FluentValidation;
namespace ABPCourse.Demo1.Products
{
    public class CreateUpdateProductValidation : AbstractValidator<CreateUpdateProductDto>
    {
        public CreateUpdateProductValidation()
        {
            RuleFor(x => x.NameAr)
                .NotNull()  // ليس فراغ
                .NotEmpty() // غير موجود
                .MaximumLength(Demo1Consts.GeneralTextMaxLength)
                .WithErrorCode(Demo1DomainErrorCodes.INVAIL_PRODUCT_NAME_ARABIC)
                .WithMessage("Product Name In Arabic Is Invalid");
            RuleFor(x => x.NameEn)
                .NotNull()
                .NotEmpty()
                .MaximumLength(Demo1Consts.GeneralTextMaxLength)
                .WithErrorCode(Demo1DomainErrorCodes.INVAIL_PRODUCT_NAME_ENGLISH)
                .WithMessage("Product Name In English Is Invalid");
            RuleFor(x => x.DescriptionAr)
                .NotEmpty()
                .MaximumLength(Demo1Consts.DescriptionTextMaxLength)
                .WithErrorCode(Demo1DomainErrorCodes.INVAIL_PRODUCT_Description_ARABIC)
                .WithMessage("Product Description In Arabic Is Invalid");
            RuleFor(x => x.DescriptionAr)
                .NotEmpty()
                .MaximumLength(Demo1Consts.DescriptionTextMaxLength)
                .WithErrorCode(Demo1DomainErrorCodes.INVAIL_PRODUCT_Description_ENGLISH)
                .WithMessage("Product Description In English Is Invalid");
            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithErrorCode(Demo1DomainErrorCodes.INVAIL_PRODUCT_CATEGORY)
                .WithMessage("Product Category Is Invalid");
        }
    }
}
