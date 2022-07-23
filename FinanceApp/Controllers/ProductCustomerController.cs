using System;
using System.Collections.Generic;
using System.Linq;
using FinanceApp.Data;
using FinanceApp.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    public class ProductCustomerController : Controller
    {
        private readonly UserDbContext context;
        public ProductCustomerController(UserDbContext userdbcontext)
        {
            context = userdbcontext;
        }

        [HttpPost("AddProductCustomerdetails")]
        public IActionResult AddProductCustometDetails([FromBody] ProductCustomerModel userObj)
        {
            try
            {
                var slotno = (from a in context.ProductCustomerModels where a.ProductId == userObj.ProductId select a.SlotNo).Max();
                userObj.SlotNo = slotno + 1;
                if (context.ProductModels.Any(a => a.ProductId == userObj.ProductId && a.NoOfCustomers >= userObj.SlotNo) &&
            context.CustomerModels.Any(a => a.CustomerId == userObj.CustomerId) &&
           !context.ProductCustomerModels.Any(a => a.ProductId == userObj.ProductId && a.SlotNo == userObj.SlotNo))
                {
                    context.ProductCustomerModels.Add(userObj);
                    context.SaveChanges();
                    return Ok(userObj);
                }
            }
            catch (InvalidOperationException)
            {
                userObj.SlotNo = 1;
                context.ProductCustomerModels.Add(userObj);
                context.SaveChanges();
                return Ok(userObj); ;
            }
            return BadRequest();
        }

        [HttpGet("CustomerDetailsForProduct")]
        public IActionResult CustomerDetailsForProduct(int id)
        {
            var data = from c1 in context.CustomerModels
                       join c in context.ProductCustomerModels on c1.CustomerId equals c.CustomerId
                       join c2 in context.ProductModels on c.ProductId equals c2.ProductId
                       join p in context.PaymentModels on c.ProductCustomerId equals p.ProductCustomerId into groupcls
                       from gc in groupcls.DefaultIfEmpty()
                       where c2.ProductId == id
                       group gc by new
                       {
                           product = c.ProductId == null ? 0 : c.ProductId,
                           ProductCustomerId = c.ProductCustomerId == null ? 0 : c.ProductCustomerId,
                           ProductName = c2.ProductName == null ? "no value" : c2.ProductName,
                           ProductTenure = c2.ProductTenure == null ? 0 : c2.ProductTenure,
                           CustomerName = c1.CustomerName == null ? "no value" : c1.CustomerName,
                           SlotNo = c.SlotNo == null ? 0 : c.SlotNo,
                       }
                       into g
                       select new
                       {
                           name = g.Key.product,
                           ProductName = g.Key.ProductName,
                           CustomerName = g.Key.CustomerName,
                           ProductTenure = g.Key.ProductTenure,
                           ProductCustomerId = g.Key.ProductCustomerId,
                           SubcriberList = g.Max(q => q == null ? 0 : q.SubscriberList),
                           SlotNo = g.Key.SlotNo
                       };
            return Ok(data);
        }

        [HttpGet("ProductDetailsForCustomer")]
        public IActionResult FoProductDetailsForCustomer(int id)
        {
            var data = from c1 in context.CustomerModels
                       join c in context.ProductCustomerModels on c1.CustomerId equals c.CustomerId
                       join c2 in context.ProductModels on c.ProductId equals c2.ProductId
                       join p in context.PaymentModels on c.ProductCustomerId equals p.ProductCustomerId into groupcls
                       from gc in groupcls.DefaultIfEmpty()
                       where c1.CustomerId == id
                       group gc by new
                       {
                           product = c.ProductId == null ? 0 : c.ProductId,
                           ProductCustomerId = c.ProductCustomerId == null ? 0 : c.ProductCustomerId,
                           ProductName = c2.ProductName == null ? "no value" : c2.ProductName,
                           ProductTenure = c2.ProductTenure == null ? 0 : c2.ProductTenure,
                           CustomerName = c1.CustomerName == null ? "no value" : c1.CustomerName,
                           SlotNo = c.SlotNo == null ? 0 : c.SlotNo,
                           Status = c1.Status == null ? "no value" : c1.Status,
                       } into g
                       select new
                       {
                           name = g.Key.product,
                           ProductName = g.Key.ProductName,
                           CustomerName = g.Key.CustomerName,
                           ProductTenure = g.Key.ProductTenure,
                           ProductCustomerId = g.Key.ProductCustomerId,
                           SlotNo = g.Key.SlotNo,
                           Status = g.Key.Status
                       };

            return Ok(data);
        }
    }
}

