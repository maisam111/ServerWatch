using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerWatch.Data;
using ServerWatch.Models;

namespace ServerWatch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class staticInfoesController : ControllerBase
    {
        private readonly ServerWatchContext _context;

        public staticInfoesController(ServerWatchContext context)
        {
            _context = context;
        }

        // GET: api/staticInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<staticInfo>>> GetstaticInfo()
        {
            return await _context.staticInfo.ToListAsync();
        }

        // GET: api/staticInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<staticInfo>> GetstaticInfo(int id)
        {
            var staticInfo = await _context.staticInfo.FindAsync(id);

            if (staticInfo == null)
            {
                return NotFound();
            }

            return staticInfo;
        }

        // PUT: api/staticInfoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutstaticInfo(int id, staticInfo staticInfo)
        {
            if (id != staticInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(staticInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!staticInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/staticInfoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<staticInfo>> PoststaticInfo(staticInfo staticInfo)
        {
            _context.staticInfo.Add(staticInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetstaticInfo", new { id = staticInfo.Id }, staticInfo);
        }

        // DELETE: api/staticInfoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletestaticInfo(int id)
        {
            var staticInfo = await _context.staticInfo.FindAsync(id);
            if (staticInfo == null)
            {
                return NotFound();
            }

            _context.staticInfo.Remove(staticInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool staticInfoExists(int id)
        {
            return _context.staticInfo.Any(e => e.Id == id);
        }
    }
}
