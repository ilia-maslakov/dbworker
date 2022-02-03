using dbworker.Data;
using dbworker.Data.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace dbworker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrgController : ControllerBase
    {
        private readonly ILogger<UnitOfWork> _logger;
        private readonly UnitOfWork _unitOfWork;

        public OrgController(ILogger<UnitOfWork> logger, UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<Org>> Get(int id)
        {
            var o = await _unitOfWork.Orgs.GetAsync(id);

            if (o == null)
            {
                return NotFound();
            }

            return o;
        }

        [HttpPost("Add/{name}")]
        public async Task<ActionResult<Org>> Add(string name)
        {


            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Add Org({name})");

            var o = new Org { Name = name };
            await _unitOfWork.Orgs.AddAsync(o);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Error Add Org: {e.Message}");
            }

            return o;
        }

        // DELETE: api/Org/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrg(int id)
        {
            var org = await _unitOfWork.Orgs.GetAsync(id);
            if (org == null)
            {
                return NotFound();
            }

            _unitOfWork.Orgs.Remove(org);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
