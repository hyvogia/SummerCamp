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
    public class CampersController : ControllerBase
    {
        private readonly SummerCampContext _context;

        public CampersController(SummerCampContext context)
        {
            _context = context;
        }

        // GET: api/Campers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CamperDTO>>> GetCampers()
        {
            return await _context.Campers
                .Include(c => c.Compound)
                .Select(c => new CamperDTO
                {
                    ID = c.ID,
                    FirstName = c.FirstName,
                    MiddleName = c.MiddleName,
                    LastName = c.LastName,
                    DOB = c.DOB,
                    Gender = c.Gender,
                    EMail = c.EMail,
                    Phone = c.Phone,
                    RowVersion = c.RowVersion,
                    CompoundID = c.CompoundID,
                    Compound = null
                })
                .ToListAsync();
        }

        // GET: api/Campers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CamperDTO>> GetCamper(int id)
        {
            var camperDTO = await _context.Campers
                .Include(c => c.Compound)
                .Select(c => new CamperDTO
                {
                    ID = c.ID,
                    FirstName = c.FirstName,
                    MiddleName = c.MiddleName,
                    LastName = c.LastName,
                    DOB = c.DOB,
                    Gender = c.Gender,
                    EMail = c.EMail,
                    Phone = c.Phone,
                    RowVersion = c.RowVersion,
                    CompoundID = c.CompoundID,
                    Compound = null
                })
                .FirstOrDefaultAsync(c => c.ID == id);

            if (camperDTO == null)
            {
                return NotFound(new { message = "Error: Can't locate the camper." });
            }

            return camperDTO;
        }

        // GET: api/CampersByCompound
        [HttpGet("ByCompound/{id}")]
        public async Task<ActionResult<IEnumerable<CamperDTO>>> GetCampersByCompound(int id)
        {
            var camperDTO = await _context.Campers
                .Include(a => a.Compound)
                .Where(a => a.CompoundID == id)
                .Select(a => new CamperDTO
                {
                    ID = a.ID,
                    FirstName = a.FirstName,
                    MiddleName = a.MiddleName,
                    LastName = a.LastName,
                    DOB = a.DOB,
                    Gender = a.Gender,
                    EMail = a.EMail,
                    Phone = a.Phone,
                    RowVersion = a.RowVersion,
                    CompoundID = a.CompoundID,
                    Compound = new CompoundDTO
                    {
                        ID = a.Compound.ID,
                        Name = a.Compound.Name,
                        Campers = null
                    }
                })
                .ToListAsync();

            if (camperDTO.Count() == 0)
            {
                return NotFound(new { message = "Error: No camper of this compound." });
            }

            return camperDTO;
        }

        // PUT: api/Campers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCamper(int id, CamperDTO camperDTO)
        {
            if (id != camperDTO.ID)
            {
                return BadRequest(new { message = "Error: ID does not match camper." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var camperToUpdate = await _context.Campers.FindAsync(id);
            
            if (camperToUpdate == null)
            {
                return NotFound(new { message = "Error: Can't locate the camper." });
            }
            if (camperDTO.RowVersion != null)
            {
                if (!camperToUpdate.RowVersion.SequenceEqual(camperDTO.RowVersion))
                {
                    return Conflict(new { message = "Concurrency Error: Camper has been changed by another user.  Try editing the record again." });
                }
            }

            camperToUpdate.ID = camperDTO.ID;
            camperToUpdate.FirstName = camperDTO.FirstName;
            camperToUpdate.LastName = camperDTO.LastName;
            camperToUpdate.MiddleName = camperDTO.MiddleName;
            camperToUpdate.DOB = camperDTO.DOB;
            camperToUpdate.Gender = camperDTO.Gender;
            camperToUpdate.EMail = camperDTO.EMail;
            camperToUpdate.Phone = camperDTO.Phone;
            camperToUpdate.RowVersion = camperDTO.RowVersion;
            camperToUpdate.CompoundID = camperDTO.CompoundID;

            _context.Entry(camperToUpdate).Property("RowVersion").OriginalValue = camperDTO.RowVersion;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CamperExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: Camper has been Removed." });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Camper has been updated by another user.  Back out and try editing the record again." });
                }
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Camper." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // POST: api/Campers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CamperDTO>> PostCamper(CamperDTO camperDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Camper camper = new Camper
            {
                FirstName = camperDTO.FirstName,
                MiddleName = camperDTO.MiddleName,
                LastName = camperDTO.LastName,
                DOB = camperDTO.DOB,
                Gender = camperDTO.Gender,
                EMail = camperDTO.EMail,
                Phone = camperDTO.Phone,
                CompoundID = camperDTO.CompoundID
            };

            try
            {
                _context.Campers.Add(camper);
                await _context.SaveChangesAsync();

                camperDTO.ID = camper.ID;
                camperDTO.RowVersion = camper.RowVersion;

                return CreatedAtAction(nameof(GetCamper), new { id = camper.ID }, camperDTO);
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Camper." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // DELETE: api/Campers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCamper(int id)
        {
            var camper = await _context.Campers.FindAsync(id);
            if (camper == null)
            {
                return NotFound(new { message = "Delete Error: Camper has already been removed." });
            }

            try
            {
                _context.Campers.Remove(camper);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Delete Error: Unable to delete Camper." });
            }
        }

        private bool CamperExists(int id)
        {
            return _context.Campers.Any(e => e.ID == id);
        }
    }
}
