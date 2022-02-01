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
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly DBworkerContext _db;
        private readonly UserValidator _validator;

        public UserController(ILogger<UserController> logger, DBworkerContext db)
        {
            _logger = logger;
            _db = db;
            _validator = new UserValidator();
        }
        
        private bool IsValidData(User user)
        {
            var result = _validator.Validate(user);
            if (result.IsValid)
            {
                return true;
            }

            foreach (var error in result.Errors)
            {
                _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} User validation errors: {error.ErrorMessage}");
            }
            return false;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var org = await _db.User.FindAsync(id);

            if (org == null)
            {
                return NotFound();
            }

            return org;
        }

        [HttpPost("Add/{name},{surname},{patronymic},{email}")]
        public async Task<ActionResult<User>> Add(string name, string surname, string patronymic, string email)
        {

            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Add({name})");

            var u = new User { Name = name, Surname = surname, Patronymic = patronymic, Email = email };
            if (IsValidData(u))
            {
                _db.Add(u);
                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Error: {e.Message}");
                    return BadRequest($"Error: {e.Message}");
                }
            }
            else
            {
                _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Adding aborted!!!");
                return BadRequest("Incorect params");
            }
            return u;
        }

        [HttpPut("LinkUserOrg/user={id}/org={orgid}")]
        public async Task<ActionResult<User>> LinkUserOrg(int id, int orgid)
        {
            var u = await _db.User.FindAsync(id);

            if (u == null)
            {
                return BadRequest($"user id = ({id}) not found"); 
            }

            if (!OrgExists(orgid))
            {
                return BadRequest($"org id = ({orgid}) not found");
            }

            u.Org = orgid;
            _db.Entry(u).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                return BadRequest($"Error LinkUserOrg [{e.Message}]");
            }
            return Ok();
        }

        [HttpGet("{orgid}")]
        public ActionResult<IEnumerable<User>> UserList(int? OrgId)
        {
            int org = OrgId ?? 0;

            IQueryable<User> l = _db.User;

            if (org > 0)
            {
                l = l.Where(p => p.Org == org);
            }
            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} UserList retern {l.Count()} records");

            return l.ToList();
        }

        private bool OrgExists(int orgid)
        {
            var u = _db.Org.Find(orgid);
            if (u != null)
            {
                return true;
            }
            return false;
        }

        private bool UserExists(int id)
        {
            var u = _db.User.Find(id);
            if (u != null)
            {
                return true;
            }
            return false;
        }

    

    }
}
