using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerAuth.Models;
using ServerAuth.Database;
using ServerAuth.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ServerAuth.Controllers
{
    [Authorize]
    [ApiController]
    public class WatchController : ControllerBase
    {
        private WatchService _watchService;
        public WatchController(WatchService watchService)
        {
            _watchService = watchService;
        }

        [HttpGet]
        [Route("api/watch")]
        public async Task<IActionResult> Get()
        {
            var result = await _watchService.GetAllAsync();
            List<WatchDTO> objs = new List<WatchDTO>();
            foreach (var item in  result)
            {
                WatchDTO o = new WatchDTO();
                o.Id = item.Id.ToString();
                o.Brand = item.Brand;
                o.Price = item.Price;
                objs.Add(o);
            }
            return Ok(objs);
        }

        [HttpGet]
        [Route("api/watch/{watchId}")]
        public async Task<IActionResult> GetById(string watchId)
        {
            var watch = await _watchService.GetByIdAsync(watchId);
            if (watch == null)
            {
                return BadRequest("User not found!");
            }
            return Ok(watch);
        }

        [HttpPost]
        [Route("api/watch")]
        public async Task<IActionResult> AddWatch(Watch iWatch)
        {
            if (iWatch == null)
            {
                return BadRequest("Please send a valid data firstname, lastname, age!");
            }

            if (iWatch.Price == 0 ||
                string.IsNullOrWhiteSpace(iWatch.Brand))
            {
                return BadRequest("firstname and lastname cant be empty");
            }

            await _watchService.AddAsync(iWatch);

            return Ok();
        }

        [HttpPut]
        [Route("api/watch")]
        public async Task<IActionResult> UpdateWatch(Watch iWatch)
        {
            if (iWatch == null)
            {
                return BadRequest("Please send a valid data firstname, lastname, age!");
            }
            if (string.IsNullOrWhiteSpace(iWatch.Id.ToString()))
            {
                return BadRequest("User id is empty");
            }
            var success = await _watchService.UpdateAsync(iWatch);
            if (success)
                return Ok();
            return StatusCode(StatusCodes.Status500InternalServerError, "database error");
        }

        [HttpDelete]
        [Route("api/watch/{watchId}")]
        public async Task<IActionResult> DeleteWatch(string WatchId)
        {
            var deleteSuccess = await _watchService.DeleteAsync(WatchId);
            if (deleteSuccess)
            {
                return Ok();

            }
            else
            {
                return NotFound();
            }
        }
    }
}
