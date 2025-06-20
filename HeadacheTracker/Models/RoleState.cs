using HeadacheTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreMVC_SchoolSystem.Models {
    public class RoleState {
        public IdentityRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }

    }
}
