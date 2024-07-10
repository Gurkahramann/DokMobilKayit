using DockMobile.Api.Data;
using DockMobile.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DockMobile.Api.Controllers
{
    // Define route and set controller to handle HTTP requests
    [Route("api/[controller]")]
    [ApiController]
    public class DokPartiDetayController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        // Constructor to inject the AppDbContext
        public DokPartiDetayController(AppDbContext appDbContext) => _appDbContext = appDbContext;

        // GET: api/DokPartiDetay
        [HttpGet]
        public ActionResult<IEnumerable<DokPartiDetay>> Get()
        {
            // Retrieve all DokPartiDetay records
            return _appDbContext.DokPartiDetay;
        }

        // GET: api/DokPartiDetay/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DokPartiDetay?>> GetById(int id)
        {
            // Retrieve a single DokPartiDetay by its ID
            return await _appDbContext.DokPartiDetay.Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        // POST: api/DokPartiDetay
        [HttpPost]
        public async Task<ActionResult> Create(DokPartiDetay DockPartiAnadetail)
        {
            // Add a new DokPartiDetay record
            await _appDbContext.DokPartiDetay.AddAsync(DockPartiAnadetail);
            await _appDbContext.SaveChangesAsync();

            // Return the newly created DokPartiDetay record
            return CreatedAtAction(nameof(GetById), new { id = DockPartiAnadetail.Id }, DockPartiAnadetail);
        }

        // PUT: api/DokPartiDetay
        [HttpPut]
        public async Task<ActionResult> Update(DokPartiDetay DockPartiAnaDetail)
        {
            // Update an existing DokPartiDetay record
            _appDbContext.DokPartiDetay.Update(DockPartiAnaDetail);
            await _appDbContext.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/DokPartiDetay/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            // Attempt to retrieve the DokPartiDetay to delete
            var DockPartiAnaGetByIdResult = await GetById(id);
            if (DockPartiAnaGetByIdResult.Value is null)
            {
                // If not found, return NotFound
                return NotFound();
            }

            // If found, remove the DokPartiDetay record
            _appDbContext.Remove(DockPartiAnaGetByIdResult.Value);
            await _appDbContext.SaveChangesAsync();
            return Ok();
        }

        // GET: api/DokPartiDetay/number/{partiNumara}
        [HttpGet("number/{partiNumara}")]
        public async Task<ActionResult<DokPartiDetay>> GetPartyByNumber(string partiNumara)
        {
            // Retrieve a DokPartiDetay by its PartiNo
            var partyDetail = await _appDbContext.DokPartiDetay
                .FirstOrDefaultAsync(p => p.PartiNo == partiNumara);

            if (partyDetail == null)
            {
                // If not found, return NotFound
                return NotFound();
            }

            // If found, return the DokPartiDetay record
            return Ok(partyDetail);
        }
    }
}
