using dbworker.Data;
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
        private readonly UserValidator _validator;
        private readonly UnitOfWork _unitOfWork;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, DBworkerContext db)
        {
            _unitOfWork = new UnitOfWork(db);
            _validator = new UserValidator();
            _logger = logger;
        }
        
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _unitOfWork.Users.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("org/{orgid?}")]
        public ActionResult<IEnumerable<User>> GetAll(int? orgid)
        {
            IEnumerable<User> user;

            if ((orgid ?? 0) != 0)
            {
                user = _unitOfWork.Users.Get(p => p.Org == orgid);
            }
            else
            {
                user = _unitOfWork.Users.Get();
            }

            if (user == null)
            {
                return NotFound();
            }

            return user.ToList();
        }

        [HttpPost("Add/{name},{surname},{patronymic},{email}")]
        public async Task<ActionResult<User>> Add(string name, string surname, string patronymic, string email)
        {
            var u = new User { Name = name, Surname = surname, Patronymic = patronymic, Email = email };
            if (IsValid(u))
            {
                _unitOfWork.Users.Add(u);
                try
                {
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    return BadRequest($"Error: {e.Message}");
                }
            }
            else
            {
                return BadRequest("Incorect params");
            }
            return u;
        }


        [HttpPut("LinkUserOrg/user={id}/org={orgid}")]
        public async Task<ActionResult<User>> LinkUserOrg(int id, int orgid)
        {
            var u = await _unitOfWork.Users.GetAsync(id);

            if (u == null)
            {
                return BadRequest($"user id = ({id}) not found");
            }

            if (_unitOfWork.Orgs.Get(orgid) == null)
            {
                return BadRequest($"org id = ({orgid}) not found");
            }

            u.Org = orgid;
            _unitOfWork.Users.Update(u);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                return BadRequest($"Error LinkUserOrg [{e.Message}]");
            }
            return Ok();
        }
        
        private bool IsValid(User user)
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


    }
}
