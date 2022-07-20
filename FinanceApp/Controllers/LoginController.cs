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
            if (user != null && user.Role == "Admin")
            {

                return Ok(user);
            }
            if (user != null && user.Role == "operator")
            {
                return Ok(user);
            }
            return BadRequest();
        }
        [HttpGet("GetUser")]
        public IActionResult GetUser(int data)
        {
            var user = dataContext.LoginModels.Where(x => x.UserId == data).SingleOrDefault();
            if (user != null && user.Role == "Admin")
            {
                var AllUser = dataContext.LoginModels.AsQueryable();
                return Ok(AllUser);

            }
            if (user != null && user.Role == "operator")
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

        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser([FromBody] LoginModel Obj)
        {

            if (dataContext.LoginModels.Any(a => a.UserId == Obj.
            UserId && a.UserName == Obj.UserName))
            {
                dataContext.Entry(Obj).State = EntityState.Modified;
                dataContext.SaveChanges();
                return Ok(Obj);
            }
            else if (dataContext.LoginModels.Any(a => a.UserId == Obj.UserId && a.UserName != Obj.UserName))
            {
                if (dataContext.LoginModels.Any(a => a.UserName == Obj.UserName))
                {
                    return BadRequest(new
                    {
                        StatusCode = "400"
                    });
                }
                else
                {
                    dataContext.Entry(Obj).State = EntityState.Modified;
                    dataContext.SaveChanges();
                    return Ok(Obj);
                }
            }
            return BadRequest(new
            {
                StatusCode = "400"
            });
        }

        [HttpDelete("DeleteUser")]
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

        [HttpGet("CheckUser")]
        public IActionResult CheckUser(int id)
        {
            var checkuser = dataContext.LoginModels.AsNoTracking().FirstOrDefault(x => x.UserId == id);
            bool isActive = true;
            if (checkuser != null)
            {
                return Ok(isActive);
            }
            return Ok(!isActive);
        }
    }
}