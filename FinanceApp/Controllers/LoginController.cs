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
    public class LoginController : ControllerBase
    {
        private readonly UserDbContext dataContext;
        public LoginController(UserDbContext userData)
        {
            dataContext = userData;
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody] LoginModel userData)
        {
            if (userData != null && !(dataContext.LoginModels.Any(a => a.UserName == userData.UserName)))
            {
                dataContext.LoginModels.Add(userData);
                dataContext.SaveChanges();
                return Ok(userData);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("GetLogin")]
        public IActionResult GetLogin([FromBody] LoginModel data)
        {
            var user = dataContext.LoginModels.Where(x => x.UserName == data.UserName && x.Password == data.Password).FirstOrDefault();
            if (dataContext.LoginModels.Any(x => x.UserName == data.UserName && x.Password == data.Password && x.Role == "Admin"))
            {
                return Ok(user);
            }
            if (dataContext.LoginModels.Any(x => x.UserName == data.UserName && x.Password == data.Password && x.Role == "operator"))
            {
                return Ok(user);
            }
            return BadRequest();
        }

        [HttpGet("UserExist")]
        public IActionResult GetUser(string obj)
        {
            var userDetails = dataContext.LoginModels.AsNoTracking().FirstOrDefault(x => x.UserName == obj);
            if (userDetails == null)
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

        [HttpPut("UpdateLogin")]
        public IActionResult UpdateLogin([FromBody] LoginModel obj)
        {
            if (obj == null)
            {
                return BadRequest();
            }
            var user = dataContext.LoginModels.AsNoTracking().FirstOrDefault(x => x.UserId == obj.UserId);
            if (!dataContext.LoginModels.Any(x => x.UserName == obj.UserName) && user != null)
            {
                dataContext.Entry(obj).State = EntityState.Modified;
                dataContext.SaveChanges();
                return Ok(obj);
            }
            return BadRequest();
        }

        [HttpDelete("DeletUser")]
        public IActionResult DeletUser(int id)
        {
            var deleteUser = dataContext.LoginModels.Find(id);
            if (deleteUser == null)
            {
                return NotFound();
            }
            else
            {
                dataContext.LoginModels.Remove(deleteUser);
                dataContext.SaveChanges();
                return Ok();
            }
        }
    }
}