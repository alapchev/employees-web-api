using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesApp.Services.Models
{
    public class EmployeePair : IEquatable<EmployeePair>
    {
        public EmployeePair(int employee1Id, int employee2Id)
        {
            if (employee1Id == employee2Id)
            {
                throw new ArgumentException("The employee IDs must be different");
            }

            if (employee1Id > employee2Id)
            {
                int temp = employee1Id;
                employee1Id = employee2Id;
                employee2Id = temp;
            }

            Employee1Id = employee1Id;
            Employee2Id = employee2Id;
            CommonProjectsIds = new();
            DatesWorkedTogether = new();
        }

        public int Employee1Id { get; }

        public int Employee2Id { get; }

        public int DaysWorkedTogether => DatesWorkedTogether.Count;

        public HashSet<int> CommonProjectsIds { get; }

        public HashSet<DateOnly> DatesWorkedTogether { get; }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is EmployeePair other) return Equals(other);

            return false;
        }

        public bool Equals(EmployeePair? other)
        {
            if (other is null) return false;

            return Employee1Id == other.Employee1Id
                && Employee2Id == other.Employee2Id;
        }

        public override int GetHashCode()
            => HashCode.Combine(Employee1Id, Employee2Id);
    }
}
