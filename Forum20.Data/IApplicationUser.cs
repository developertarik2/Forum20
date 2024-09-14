using Forum20.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Forum20.Data
{
   public interface IApplicationUser
    {
        ApplicationUser GetById(string id);
        IEnumerable<ApplicationUser> GetAll();
        Task SetProfileImage(string id,Uri uri);
        Task UpdateUserRating(string id, Type type);
        Task Deactivate(ApplicationUser user);
    }
}
