using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            
        }
        public ApplicationUser(string userName):base()
        {
            base.UserName = userName;
        }

        [StringLength(256)]
        public string IpAddress { get; set; }
    }
}
