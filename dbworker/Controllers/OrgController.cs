using dbworker.Data.EF;
using dbworker.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dbworker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrgController : ControllerBase
    {
        private readonly ILogger<OrgController> _logger;
        private readonly DBworkerContext _context;
        private readonly OrgValidator _validator;

        public OrgController(ILogger<OrgController> logger, DBworkerContext context)
        {
            _context = context;
            _logger = logger;
            _validator = new OrgValidator();
        }

        private bool IsValidData(Org org)
        {
            var result = _validator.Validate(org);
            if (result.IsValid)
            {
                return true;
            }

            foreach (var error in result.Errors)
            {
                _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Org validation errors: {error.ErrorMessage}");
            }
            return false;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Org>>> Get()
        {
            return await _context.Org.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Org>> Get(int id)
        {
            var org = await _context.Org.FindAsync(id);

            if (org == null)
            {
                return NotFound();
            }

            return org;
        }

        [HttpPost("Add/{name}")]
        public async Task<ActionResult<Org>> Add(string name)
        {

            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} PostOrg({name})");

            var o = new Org { Name = name };
            await _context.AddAsync(o);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Error: {e.Message}");
            }

            return o;
        }

        // DELETE: api/Org/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrg(int id)
        {
            var org = await _context.Org.FindAsync(id);
            if (org == null)
            {
                return NotFound();
            }

            _context.Org.Remove(org);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        /*
        private bool OrgExists(int id)
        {
            return _context.Org.Any(e => e.Id == id);
        }
        */
    }
}
