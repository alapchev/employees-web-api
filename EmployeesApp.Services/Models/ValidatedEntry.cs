using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesApp.Services.Models
{
    public class ValidatedEntry
    {
        public ValidatedEntry(int employeeId, int projectId, DateOnly dateFrom, DateOnly dateTo)
        {
            EmployeeId = employeeId;
            ProjectId = projectId;
            DateFrom = dateFrom;
            DateTo = dateTo;
        }

        public int EmployeeId { get; set; }

        public int ProjectId { get; set; }

        public DateOnly DateFrom { get; set; }

        public DateOnly DateTo { get; set; }
    }
}
