using BulletinBoard.DB.Models;
using BulletinBoard.Web.CustomModels;
using BulletinBoard.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BulletinBoard.Web.Controllers
{
    [ApiController]
    public class BulletinsController : ControllerBase
    {
        private BulletinsService Service { get; }

        public BulletinsController(BulletinsService service)
        {
            Service = service;
        }

        [Route("GetBulletins")]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PageParams pageParams)
        {
            var result = await Service.GetAllBulletins(pageParams);
            return Ok(result);
        }

        [Route("GetBulletinByName")]
        [HttpGet]
        public async Task<IActionResult> GetAsync(string Name, [FromQuery] QueryParams queryParams)
        {
            var result = await Service.GetFirstBulletinOrDefault(queryParams, Name);
            return Ok(result);
        }

        [Route("CreateBulletin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Bulletin bulletin)
        {
            var result = await Service.CreateBulletin(bulletin);
            if (result != null) return Ok(result.ID);
            return Conflict();
        }
    }
}
