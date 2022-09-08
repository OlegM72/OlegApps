using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DictionaryApi.Models;

namespace DictionaryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryItemsController : ControllerBase
    {
        private readonly DictionaryContext _context; // to connect with the InMemory DB

        public DictionaryItemsController(DictionaryContext context)
        {
            _context = context;
        }
        
        /// <summary>HTTP GET (all) method</summary>
        /// <remarks>sample usage - GET: api/DictionaryItems</remarks>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DictionaryItem>>> GetDictionaryItems()
        {
          if (_context.DictionaryItems == null)
          {
              return NotFound(); // 404
          }
            return await _context.DictionaryItems.ToListAsync();
        }

        /// <summary>HTTP GET (id) method</summary>
        /// <remarks>sample usage - GET: api/DictionaryItems/5</remarks>
        [HttpGet("{id}")]
        public async Task<ActionResult<DictionaryItem>> GetDictionaryItem(long id)
        {
          if (_context.DictionaryItems == null)
          {
              return NotFound(); // 404
          }
            var dictionaryItem = await _context.DictionaryItems.FindAsync(id);

            if (dictionaryItem == null)
            {
                return NotFound(); // 404
            }

            return dictionaryItem; // 200
        }

        /// <summary>HTTP PUT (id) method</summary>
        /// <remarks>sample usage - PUT: api/DictionaryItems/5</remarks>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDictionaryItem(long id, DictionaryItem dictionaryItem)
        {
            if (id != dictionaryItem.Id)
            {
                return BadRequest(); // 400
            }

            _context.Entry(dictionaryItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DictionaryItemExists(id))
                {
                    return NotFound(); // 404
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // 204
        }

        /// <summary>HTTP POST method</summary>
        /// <remarks>sample usage - POST: api/DictionaryItems</remarks>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DictionaryItem>> PostDictionaryItem(DictionaryItem dictionaryItem)
        {
          if (_context.DictionaryItems == null)
          {
              return Problem("Entity set 'DictionaryContext.DictionaryItems' is null.");
          }
            _context.DictionaryItems.Add(dictionaryItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDictionaryItem), new { id = dictionaryItem.Id }, dictionaryItem); // 201
        }

        /// <summary>HTTP DELETE (id) method</summary>
        /// <remarks>sample usage - DELETE: api/DictionaryItems/5</remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDictionaryItem(long id)
        {
            if (_context.DictionaryItems == null)
            {
                return NotFound(); // 404
            }
            var dictionaryItem = await _context.DictionaryItems.FindAsync(id);
            if (dictionaryItem == null)
            {
                return NotFound(); // 404
            }

            _context.DictionaryItems.Remove(dictionaryItem);
            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }

        private bool DictionaryItemExists(long id)
        {
            return (_context.DictionaryItems?.Any(e => e.Id == id)).GetValueOrDefault(); // 200
        }
    }
}
