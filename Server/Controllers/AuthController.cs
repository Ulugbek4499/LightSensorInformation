using Microsoft.AspNetCore.Mvc;
using Server.Entities.Identity;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user=new User();


    }
}
