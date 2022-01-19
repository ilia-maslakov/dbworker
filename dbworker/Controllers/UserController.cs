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
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase, IUserController
    {
        private readonly ILogger<UserController> _logger;
        private readonly DBworkerContext _context;
        private readonly UserValidator _validator;

        public UserController(ILogger<UserController> logger, DBworkerContext context)
        {
            _logger = logger;
            _context = context;
            _validator = new UserValidator();
        }

        private bool isValidData(User user)
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

        [HttpPost("Add/{name},{surname},{patronymic}")]
        public async Task<ActionResult<User>> Add(string name, string surname, string patronymic)
        {

            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Add({name})");

            var u = new User { Name = name, Surname = surname, Patronymic = patronymic };
            if (isValidData(u))
            {
                _context.Add(u);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Error: {e.Message}");
                    return BadRequest(e.Message);
                }

            }
            else
            {
                _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Adding aborted!!!");
                return BadRequest("Incorect params");
            }
            return u;
        }
        /*
        public Task<ActionResult<User>> Add(User user)
        {
            return Add(user.Name, user.Surname, user.Patronymic);
        }
        */
        [HttpPut("{id}/{orgid}")]
        public async Task<IActionResult> LinkUserOrg(int id, int orgid)
        {
            var u = await _context.User.FindAsync(id);

            if (u == null)
            {
                return NotFound($"user id = ({id}) not found");
            }
            if (!OrgExists(orgid))
            {
                return NotFound($"org id = ({orgid}) not found");
            }

            u.Org = orgid;
            _context.Entry(u).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                return BadRequest($"Error LinkUserOrg [{e.Message}]");
            }

            return Ok();
        }


        [HttpGet]
        public IList<User> UserList(int? OrgId)
        {

            int orgid = OrgId ?? 0;
            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} UserList({OrgId} -> {orgid})");
            IQueryable<User> l = _context.User;

            if (orgid > 0)
            {
                l = l.Where(p => p.Org == orgid);
            }
            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} UserList retern {l.Count()} records");
            return l.ToList();
        }
        private bool OrgExists(int id)
        {
            return _context.Org.Any(e => e.Id == id);
        }

    }
}
