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

    public class PaymentController : Controller
    {
        private readonly UserDbContext context;
        public PaymentController(UserDbContext userdbcontext)
        {
            context = userdbcontext;
        }

        [HttpPost("PaymentDetails")]
        public IActionResult AddProductCustometDetails([FromBody] PaymentModel pay)
        {
            pay.PaymentDate = DateTime.UtcNow.Date;
            if (pay != null && context.ProductCustomerModels.Any(a => a.ProductCustomerId == pay.ProductCustomerId))
            {
                try
                {
                    var slotno = (from a in context.PaymentModels where a.ProductCustomerId == pay.ProductCustomerId select a.SubscriberList).Max();
                    pay.SubscriberList = slotno + 1;
                    if (context.ProductCustomerModels.Any(a => a.ProductCustomerId == pay.ProductCustomerId && a.ProductCustomerId >= pay.SubscriberList) &&
               context.PaymentModels.Any(a => a.ProductCustomerId == pay.ProductCustomerId))
                    {
                       
                        context.PaymentModels.Add(pay);
                        context.SaveChanges();
                        return Ok(pay);
                    }

                }
                catch (InvalidOperationException)
                {
                    pay.SubscriberList = 1;
                    context.PaymentModels.Add(pay);
                    context.SaveChanges();
                    return Ok(pay); ;
                }
            }
            return BadRequest();
        }

        [HttpPost("FilteredItems")]
        public IActionResult GetFilterDetails([FromBody] RequestModel requestModel)
        {

            var data = from c1 in context.ProductCustomerModels
                       join c in context.CustomerModels on c1.CustomerId equals c.CustomerId
                       join p in context.ProductModels on c1.ProductId equals p.ProductId
                       join p1 in context.PaymentModels on c1.ProductCustomerId equals p1.ProductCustomerId into groupcls
                       from gc in groupcls.DefaultIfEmpty()
                       where gc.PaymentDate.Date >= requestModel.FromDate.Date && gc.PaymentDate.Date <= requestModel.ToDate.Date
                       group gc by new
                       {
                           p.ProductName,
                           c.CustomerName,
                           gc.PaymentId,
                           gc.PaymentDate,
                           gc.PaidAmount,
                           c1.ProductCustomerId,
                           gc.CollectedBy,
                           gc.PaymentType

                       } into g
                       select new
                       {
                           PaymentId = g.Key.PaymentId,
                           ProductName = g.Key.ProductName,
                           CustomerName = g.Key.CustomerName,
                           PaymentDate = g.Key.PaymentDate,
                           PaidAmount = g.Key.PaidAmount,
                           CollectedBy = g.Key.CollectedBy,
                           ProductCustomerId = g.Key.ProductCustomerId,
                           PaymentType = g.Key.PaymentType


                       };

            return Ok(data);
        }

        [HttpPost("TotalAmount")]
        public IActionResult GetTotalAmount([FromBody] RequestModel requestModel)
        {

            var TotalAmount = (from g in context.PaymentModels
                               where g.PaymentDate.Date >= requestModel.FromDate.Date && g.PaymentDate.Date <= requestModel.ToDate.Date
                               select g.PaidAmount).Sum();

            return Ok(TotalAmount);

        }

        [HttpGet("getPaymentDetails")]
        public IActionResult GetPaymentDetails(int id)
        {
            var products = context.PaymentModels.Where(a => a.ProductCustomerId == id);
            return Ok(products);

        }
        [HttpGet("CustomerPayHistory")]
        public IActionResult CustomerDetailsForPay(int id)
        {
            var data = from c1 in context.ProductCustomerModels
                       join c in context.CustomerModels on c1.CustomerId equals c.CustomerId
                       join p in context.ProductModels on c1.ProductId equals p.ProductId
                       join p1 in context.PaymentModels on c1.ProductCustomerId equals p1.ProductCustomerId
                       where c1.ProductCustomerId == id
                       select new
                       {
                           p.ProductName,
                           c.CustomerName,
                           p1.PaymentId,
                           p1.PaymentDate,
                           p1.PaidAmount,
                           c1.ProductCustomerId,
                           p1.CollectedBy,
                           p1.PaymentType
                       };
            return Ok(data);
        }
    }
}