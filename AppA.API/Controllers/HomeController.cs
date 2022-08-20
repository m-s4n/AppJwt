using AppA.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppA.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public ICollection<Info> GetInfo()
        {
            ICollection<Info> infos = new List<Info>();
            infos.Add(new() { Id = 1, Name = "Dua Lipa", Age = 24 });
            infos.Add(new() { Id = 2, Name = "Bengü", Age = 29 });
            infos.Add(new() { Id = 3, Name = "Simge", Age = 30 });

            return infos;
        }
    }
}
