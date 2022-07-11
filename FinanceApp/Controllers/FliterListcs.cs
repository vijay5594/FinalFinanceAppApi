using FinanceApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FinanceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FliterListcs : ControllerBase
    {
        private readonly UserDbContext context;
        public FliterListcs(UserDbContext userdbcontext)
        {
            context = userdbcontext;
        }
       

    }
}
