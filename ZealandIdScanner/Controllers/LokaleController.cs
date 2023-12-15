using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZealandIdScanner.Models.Sensors;
using ZealandIdScanner.Models;
using ZealandIdScanner;
using Microsoft.EntityFrameworkCore;
using ZealandIdScanner.EBbContext;

namespace ZealandIdScanner.Controllers
    //hej
{
    [ApiController]
    [Route("api/[controller]")]
    public class LokaleController : ControllerBase
    {
        private readonly ZealandIdContext _dbContext;

        public LokaleController(ZealandIdContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Lokale
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lokaler>>> GetLokaler()
        {
            if (_dbContext.Lokaler == null)
            {
                return NotFound();
            }
            return await _dbContext.Lokaler.ToListAsync();
        }

        [HttpGet("Id/{id}")]
        public async Task<ActionResult<Lokaler>> GetLokale(int id)
        {
            if (_dbContext.Set<Lokaler>() == null)
            {
                return NotFound("DbContext can't be null");
            }
            var lokaler = await _dbContext.Lokaler.FindAsync(id);

            if (lokaler == null)
            {
                return NotFound("No Such lokale exists");
            }
            return Ok(lokaler);
        }

        [HttpPost]
        public ActionResult PostNewLokaler(Lokaler lokaler)
        {
            if (lokaler == null)
            {
                return BadRequest(lokaler);
            }
            if (lokaler.LokaleId == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            _dbContext.SaveChangesAsync();
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            // Assuming you have configured DbContextOptions in your application's startup
            var optionsBuilder = new DbContextOptionsBuilder<ZealandIdContext>();
            optionsBuilder.UseSqlServer("Data Source=mssql11.unoeuro.com;Initial Catalog=zealandid_dk_db_test;User ID=zealandid_dk;Password=4tn2gwfADdeRB5EGzm6b;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"); // Replace with your actual connection string

            using (var ctx = new ZealandIdContext(optionsBuilder.Options))
            {
                ctx.Lokaler.Add(new Lokaler()
                {
                    LokaleId = lokaler.LokaleId,
                    Navn = lokaler.Navn,
                    SensorId = lokaler.SensorId
                });

                ctx.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Lokaler>> PutLokaler(int id, Lokaler updatedLokaler)
        {
            if (updatedLokaler == null || id != updatedLokaler.LokaleId)
            {
                return BadRequest("Invalid data or mismatched IDs");
            }

            try
            {
                // Check if the Lokaler with the given ID exists
                var existingLokaler = await _dbContext.Lokaler.FindAsync(id);

                if (existingLokaler == null)
                {
                    return NotFound("Lokaler not found");
                }

                // Update the existing Lokaler properties
                existingLokaler.Navn = updatedLokaler.Navn;

                // Save changes to the database
                await _dbContext.SaveChangesAsync();

                return Ok(existingLokaler);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLokale(int id)
        {
            try
            {
                var lokaler = await _dbContext.Lokaler.FindAsync(id);
                if (lokaler == null)
                {
                    return NotFound();
                }

                // Find and delete associated Lokaler records
                var lokalerRecords = _dbContext.Lokaler.Where(l => l.LokaleId == id).ToList();
                _dbContext.Lokaler.RemoveRange(lokalerRecords);

                // Delete the sensor
                _dbContext.Lokaler.Remove(lokaler);
                await _dbContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception details
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}


