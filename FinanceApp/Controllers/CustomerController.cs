using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Data;
using FinanceApp.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase

    {
        private readonly UserDbContext context;
        public CustomerController(UserDbContext userdbcontext)
        {
            context = userdbcontext;
        }

        [HttpPost("AddNewCustomer")]
        public IActionResult AddCustomerDetails([FromBody] CustomerModel userObj)
        {
            userObj.Status = "open";
            if (!context.CustomerModels.Any(a => a.CustomerId == userObj.CustomerId && a.MobileNumber == userObj.MobileNumber && a.AadharNumber == userObj.AadharNumber))
                context.CustomerModels.Add(userObj);
            context.SaveChanges();
            return Ok(userObj);
        }

        [HttpGet("existMobileNumber")]

        public IActionResult GetMobileNumber(string obj)
        {
            var productName = context.CustomerModels.Where(a => a.MobileNumber == obj).FirstOrDefault();

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

        [HttpGet("existAadharNumber")]

        public IActionResult GetproductName(string obj)
        {
            var productName = context.CustomerModels.Where(a => a.AadharNumber == obj).FirstOrDefault();

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

        [HttpGet("customerId")]
        public IActionResult CustomerDetails(int id)
        {
            var details = context.CustomerModels.Where(a => a.CustomerId == id).ToList();
            return Ok(details);
        }

        [HttpPut("UpdateCustomer")]
        public IActionResult UpdateCustomerDetails([FromBody] CustomerModel userObj)
        {
            /* var customer = context.CustomerModels.AsNoTracking().FirstOrDefault(a => a.CustomerId == userObj.CustomerId);
             if (customer != null)
             {
                 context.Entry(userObj).State = EntityState.Modified;
                 context.SaveChanges();
                 return Ok(userObj);
             }
             return BadRequest();*/
            if (context.CustomerModels.Any(a => a.CustomerId == userObj.CustomerId && a.MobileNumber == userObj.MobileNumber))
            {
                context.Entry(userObj).State = EntityState.Modified;
                context.SaveChanges();
                return Ok(userObj);
            }
            else if(context.CustomerModels.Any(a => a.CustomerId == userObj.CustomerId && a.MobileNumber != userObj.MobileNumber))
            {
                if(context.CustomerModels.Any(a=>a.MobileNumber==userObj.MobileNumber))
                {
                    return BadRequest(new
                    {
                        StatusCode = "400"
                    });
                }
                else
                {
                    context.Entry(userObj).State = EntityState.Modified;
                    context.SaveChanges();
                    return Ok(userObj);

                }
            }
            return BadRequest(new
            {
                StatusCode = "400"
            });

        }


        [HttpDelete("DeleteCustomer")]
        public IActionResult DeleteCustomerDetails(int CustomerId)
        {
            var customer = context.CustomerModels.Where(a => a.CustomerId == CustomerId).SingleOrDefault();
            if (customer != null && customer.IsActive == true)
                customer.IsActive = false;
            context.SaveChanges();
            return Ok(customer);
        }

        [HttpGet("AllCustomers")]
        public IActionResult GetAllCustomers()
        {
            bool IsActive = true;
            if (IsActive == true)
            {
                var customers = context.CustomerModels.Where(a => a.IsActive == IsActive);
                return Ok(customers);
            }
            else
            {
                return NotFound();
            }
        }


    /*    [HttpGet("FliterCustomerDetails")]
        public IActionResult GetAllProductCustomer()
        {
            var allproductcustomer = (from a in context.CustomerModels
                                      join p in context.FileAttachment on a.AttachmentId equals p.AttachmentId
                                      select new
                                      {
                                          p.AttachmentName,
                                          p.AttachmentId,
                                          a.CustomerName,
                                          a.CustomerId,
                                          a.GuarantorName,
                                          a.ReferredBy,
                                          a.MobileNumber,
                                          a.AadharNumber,
                                          a.Address,
                                          a.Status


                                      }).ToList();
            var produts = allproductcustomer.ToList();
            return Ok(produts);

        }

        [HttpGet("Page")]
        public ActionResult<List<CustomerModel>> GetAll(int? page = 10, int? pageSize = 10)
        {
            if (!page.HasValue)
            {
                return context.CustomerModels.ToList();
            }
            return BadRequest();
        }
    }
}*/
[HttpGet("CustomerPagination")]
public IActionResult GetAllCustomer([FromQuery] PaginationModel pagination)
{

    var allcustomer = (from a in context.CustomerModels
                       join p in context.FileAttachment on a.AttachmentId equals p.AttachmentId
                       select new
                       {
                           p.AttachmentName,
                           p.AttachmentId,
                           a.CustomerName,
                           a.CustomerId,
                           a.GuarantorName,
                           a.ReferredBy,
                           a.MobileNumber,
                           a.Address,
                           a.AadharNumber,
                           a.AdditionalMobileNumber
                           

                       }).AsQueryable();
    if (!string.IsNullOrEmpty(pagination.QuerySearch))
    {
        allcustomer = allcustomer.Where(a => a.CustomerName.Contains(pagination.QuerySearch));
    }
    int count = allcustomer.Count();
    int CurrentPage = pagination.PageNumber;
    int PageSize = pagination.PageSize;
    int TotalCount = count;
    int TotalPages = (int)Math.Ceiling(count / (double)PageSize);
    var item = allcustomer.Skip((CurrentPage - 1) * PageSize).Take(pagination.PageSize);
    var previousPage = CurrentPage > 1 ? "Yes" : "No";
    var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

    return Ok(item);
}
    }
    

}

