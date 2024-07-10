using DockMobile.Api.Data;
using DockMobile.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DockMobile.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Controller for managing DokPartiAna entities
    public class DokPartiAnaController : ControllerBase
    {
        // Initializes the database context for dependency injection
        private readonly AppDbContext _appDbContext;
        public DokPartiAnaController(AppDbContext appDbContext) => _appDbContext = appDbContext;

        // Retrieves all DokPartiAna records
        [HttpGet]
        public ActionResult<IEnumerable<DokPartiAna>> Get()
        {
            return _appDbContext.DokPartiAna;
        }

        // Retrieves a list of PartiNo values for a given DokPartiAna by its ID
        [HttpGet("{dokId}/partinumaralari")]
        public async Task<ActionResult<IEnumerable<string>>> GetPartiNumaralari(int dockId)
        {
            var DockHareket = await _appDbContext.DokPartiAna
                .FirstOrDefaultAsync(d => d.Id == dockId);

            if (DockHareket == null)
            {
                return NotFound();
            }

            var partiNumaralari = await _appDbContext.DokPartiDetay
                .Where(p => p.DokPartiAnaId == dockId)
                .Select(p => p.PartiNo)
                .ToListAsync();

            return Ok(partiNumaralari);
        }

        // Retrieves a single DokPartiAna by its ID
        [HttpGet("{id}")]
        public async Task<ActionResult<DokPartiAna?>> GetById(int id)
        {
            var DokPartiAna = await _appDbContext.DokPartiAna.FindAsync(id);
            if (DokPartiAna == null)
            {
                return NotFound();
            }
            return DokPartiAna;
        }

        // Retrieves a DokPartiAna by its DokNo
        [HttpGet("byNumber/{dockNumara}")]
        public async Task<ActionResult<DokPartiDetay>> GetDockByNumber(string dockNumara)
        {
            var DockHareket = await _appDbContext.DokPartiAna
                .FirstOrDefaultAsync(d => d.DokNo.ToLower() == dockNumara.ToLower());

            if (DockHareket == null)
            {
                return NotFound();
            }

            return Ok(DockHareket);
        }

        // Creates a new DokPartiAna record
        [HttpPost]
        public async Task<ActionResult> Create(DokPartiAna dokPartiAna)
        {
            await _appDbContext.DokPartiAna.AddAsync(dokPartiAna);
            await _appDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = dokPartiAna.Id }, dokPartiAna);
        }

        // Updates an existing DokPartiDetay record
        [HttpPut]
        public async Task<ActionResult> Update(DokPartiDetay DockHareket)
        {
            _appDbContext.DokPartiDetay.Update(DockHareket);
            await _appDbContext.SaveChangesAsync();
            return Ok();
        }

        // Deletes a DokPartiAna record by its ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var DockHareketGetByIdResult = await GetById(id);
            if (DockHareketGetByIdResult.Value is null)
            {
                return NotFound();
            }
            _appDbContext.Remove(DockHareketGetByIdResult.Value);
            await _appDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
