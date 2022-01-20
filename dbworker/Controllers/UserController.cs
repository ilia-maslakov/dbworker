using dbworker.Data.EF;
using dbworker.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dbworker.Connection;
using Microsoft.Extensions.DependencyInjection;

namespace dbworker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserValidator _validator;
        private readonly UserRepository _db;

        public UserController(ILogger<UserController> logger, DBworkerContext db)
        {
            _logger = logger;
            _validator = new UserValidator();
            _db = new UserRepository(logger, db, 1);
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
                await _db.Add(u);
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

        [HttpPut("LinkUserOrg/user={id}/org={orgid}")]
        public IActionResult LinkUserOrg(int id, int orgid)
        {
            var res = _db.LinkUserOrg(id, orgid);
            if (res != "OK")
            {
                return BadRequest(res);
            }
            return Ok();
        }


        [HttpGet]
        public IList<User> UserList(int? OrgId)
        {
            return _db.GetUsers(0, 0);
        }


    }
}
