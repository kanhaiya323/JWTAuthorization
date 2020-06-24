using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain
{
    public class UserDomain
    {
        public long Id { get; set; }
        public long Suffix { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public long Mobile { get; set; }
        public long Phone { get; set; }
        public string Password { get; set; }
        public DateTime EffectiveDt { get; set; }
        public DateTime ExpiryDt { get; set; }
        public bool IsActive { get; set; }

    }
}
