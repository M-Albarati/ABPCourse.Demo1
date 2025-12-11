using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace ABPCourse.Demo1.Products
{
    public class ProductNotFoundException : BusinessException
    {
        public ProductNotFoundException(int id) : base(Demo1DomainErrorCodes.PRODUCT_NOT_FOUND)
        {
            WithData("id", id);
        }
    }
}
