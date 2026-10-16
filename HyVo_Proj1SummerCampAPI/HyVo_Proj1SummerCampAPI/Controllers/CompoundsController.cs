using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HyVo_Proj1SummerCampAPI.Data;
using HyVo_Proj1SummerCampAPI.Models;

namespace HyVo_Proj1SummerCampAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompoundsController : ControllerBase
    {
        private readonly SummerCampContext _context;

        public CompoundsController(SummerCampContext context)
        {
            _context = context;
        }

        // GET: api/Compounds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompoundDTO>>> GetCompounds()
        {
            return await _context.Compounds
                .Select(c => new CompoundDTO
                {
                    ID = c.ID,
                    Name = c.Name,
                    Campers = null
                })
                .ToListAsync();
        }

        // GET: api/Compounds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompoundDTO>> GetCompound(int id)
        {
            var compoundDTO = await _context.Compounds
                .Select(c => new CompoundDTO
                {
                    ID = c.ID,
                    Name = c.Name,
                    Campers = null
                })
                .FirstOrDefaultAsync(c => c.ID == id);

            if (compoundDTO == null)
            {
                return NotFound(new { message = "Error: Can't locate the compound." });
            }

            return compoundDTO;
        }

        // PUT: api/Compounds/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompound(int id, CompoundDTO compoundDTO)
        {
            if (id != compoundDTO.ID)
            {
                return BadRequest(new { message = "Error: ID does not match compound." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var compoundToUpdate = await _context.Compounds.FindAsync(id);

            if (compoundToUpdate == null)
            {
                return NotFound(new { message = "Error: Can't locate the compound." });
            }
            if (compoundDTO.RowVersion != null)
            {
                if (!compoundToUpdate.RowVersion.SequenceEqual(compoundDTO.RowVersion))
                {
                    return Conflict(new { message = "Concurrency Error: Compound has been changed by another user.  Try editing the record again." });
                }
            }

            compoundToUpdate.ID = compoundDTO.ID;
            compoundToUpdate.Name = compoundDTO.Name;

            _context.Entry(compoundToUpdate).Property("RowVersion").OriginalValue = compoundDTO.RowVersion;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompoundExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: Compound has been Removed." });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Compound has been updated by another user.  Back out and try editing the record again." });
                }
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Compound." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // POST: api/Compounds
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CompoundDTO>> PostCompound(CompoundDTO compoundDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Compound compound = new Compound
            {
                Name = compoundDTO.Name
            };

            try
            {
                _context.Compounds.Add(compound);
                await _context.SaveChangesAsync();

                compoundDTO.ID = compound.ID;
                compoundDTO.RowVersion = compound.RowVersion;

                return CreatedAtAction(nameof(GetCompound), new { id = compound.ID }, compoundDTO);
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Compound." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // DELETE: api/Compounds/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCompound(int id)
        {
            var compound = await _context.Compounds.FindAsync(id);
            if (compound == null)
            {
                return NotFound(new { message = "Delete Error: Compound has already been removed." });
            }

            try
            {
                _context.Compounds.Remove(compound);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    return BadRequest(new { message = "Delete Error: Can't delete Compound with Campers assigned to it." });
                }
                else
                {
                    return BadRequest(new { message = "Delete Error: Unable to delete Compound. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        private bool CompoundExists(int id)
        {
            return _context.Compounds.Any(e => e.ID == id);
        }
    }
}
