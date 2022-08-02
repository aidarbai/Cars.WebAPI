using System;
using System.Collections.Generic;

namespace Cars.COMMON.ViewModels.Users
{
    public class UserVm
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? BannedTime { get; set; }
        public bool IsBanned { get; set; }
        public int[] Cars { get; set; }
        public List<string> Roles { get; set; }
    }
}
