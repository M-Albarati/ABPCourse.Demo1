namespace ABPCourse.Demo1;

public static class Demo1DomainErrorCodes
{
    /* You can add your business exception error codes here, as constants */
    public const string PRODUCT_NOT_FOUND = "STORE:PRODUCTS:0001";
    public const string INVAIL_PRODUCT_NAME_ARABIC = "STORE:PRODUCTS:0002";
    public const string INVAIL_PRODUCT_NAME_ENGLISH = "STORE:PRODUCTS:0003";
    public const string INVAIL_PRODUCT_Description_ARABIC = "STORE:PRODUCTS:0004";
    public const string INVAIL_PRODUCT_Description_ENGLISH = "STORE:PRODUCTS:0005";
    public const string INVAIL_PRODUCT_CATEGORY = "STORE:PRODUCTS:0006";
}
