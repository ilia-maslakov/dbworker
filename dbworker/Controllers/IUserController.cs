using dbworker.Data.EF;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dbworker.Controllers
{
    public interface IUserController
    {
        Task<ActionResult<User>> Add(string name, string surname, string patronymic);
//        Task<ActionResult<User>> Add(User user);
        Task<IActionResult> LinkUserOrg(int id, int orgid);
        IList<User> UserList(int? OrgId);
    }
}