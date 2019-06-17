using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRaySDK
{
    public class CompanyInformation
    {
        public DateTime? Created { get; private set; }

        public List<string> Employees { get; private set; }

        public CompanyInformation(DateTime? created, IEnumerable<string> employees)
        {
            Created = created;
            Employees = employees.ToList();
        }

        public CompanyInformation(DateTime? created, params string[] employees)
            : this(created, employees.ToList()) { }
    }
}
