using System.Linq;
using FinanceApp.Data;
using FinanceApp.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly UserDbContext context;
        public ProductController(UserDbContext userdbcontext)
        {
            context = userdbcontext;
        }

        [HttpPost("AddNewProduct")]
        public IActionResult AddProductDetails([FromBody] ProductModel productObj)
        {
            productObj.IsStatus = "open";
            context.ProductModels.Add(productObj);
            context.SaveChanges();
            return Ok(productObj);
        }

        [HttpGet("GetProductName")]
        public IActionResult ProductDetails(int id)
        {
            var details = context.ProductModels.Where(a => a.ProductId == id).ToList();
            return Ok(details);
        }

        [HttpGet("productNameExist")]

        public IActionResult GetproductName(string obj)
        {
            var productName = context.ProductModels.Where(a => a.ProductName.ToLower() == obj).FirstOrDefault();

            if (productName == null)
            {
                return Ok(new
                {
                    message = "You Can Enter"
                }); ;
            }
            else
            {
                return Ok(new
                {
                    message = "already Exist"
                });
            }
        }

        [HttpGet("GetProductDetails")]
        public IActionResult ProductDetails()
        {
            var data = from s in context.ProductModels
                       where s.IsStatus == "open"
                       join i in context.ProductCustomerModels on s.ProductId equals i.ProductId into groupClasses
                       from gc in groupClasses.DefaultIfEmpty()
                       group gc by new
                       {
                           product = s.ProductId == null ? 0 : s.ProductId,
                           Name = s.ProductName == null ? "no value" : s.ProductName,
                           IsStatus = s.IsStatus == null ? "no value" : s.IsStatus,
                           Tenure = s.ProductTenure == null ? 0 : s.ProductTenure,
                           Type = s.ProductType == null ? "no value" : s.ProductType,
                           Customers = s.NoOfCustomers == null ? 0 : s.NoOfCustomers,
                           Price = s.Price == null ? 0 : s.Price,
                           ProductDescription = s.ProductDescription == null ? "no value" : s.ProductDescription
                       }
         into g
                       select new
                       {
                           productId = g.Key.product,
                           productName = g.Key.Name,
                           ProductTenure = g.Key.Tenure,
                           ProductType = g.Key.Type,
                           Price = g.Key.Price,
                           ProductDescription = g.Key.ProductDescription,
                           NoOfCustomers = g.Key.Customers,
                           IsStatus = g.Key.IsStatus,
                           Slot = g.Max(p => p == null ? 0 : p.SlotNo)
                       };
            return Ok(data);
        }

        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProductDetails([FromBody] ProductModel productObj)
        {
            if (context.ProductModels.Any(a => a.ProductId == productObj.
             ProductId && a.ProductName == productObj.ProductName))
            {
                context.Entry(productObj).State = EntityState.Modified;
                context.SaveChanges();
                return Ok(productObj);
            }
            else if (context.ProductModels.Any(a => a.ProductId == productObj.ProductId && a.ProductName != productObj.ProductName))
            {
                if (context.ProductModels.Any(a => a.ProductName == productObj.ProductName))
                {
                    return BadRequest(new
                    {
                        StatusCode = "400"
                    });
                }
                else
                {
                    context.Entry(productObj).State = EntityState.Modified;
                    context.SaveChanges();
                    return Ok(productObj);
                }
            }
            return BadRequest(new
            {
                StatusCode = "400"
            });
        }

        [HttpDelete("DeleteProduct")]
        public IActionResult DeleteProductDetails(int ProductId)
        {
            var products = context.ProductModels.Where(a => a.ProductId == ProductId).FirstOrDefault();
            if (products == null)
            {
                return BadRequest();
            }
            else
            {
                products.IsActive = false;
                context.SaveChanges();
                return Ok();
            }
        }
    }
}




