using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class AccountInformation
    {
        public DateTime? Created { get; private set; }

        public string Email { get; private set; }

        public string Name { get; private set; }

        public string Role { get; private set; }

        internal AccountInformation(DateTime? created, string email, string name, string role)
        {
            Created = created;
            Email = email;
            Name = name;
            Role = role;
        }
    }
}
