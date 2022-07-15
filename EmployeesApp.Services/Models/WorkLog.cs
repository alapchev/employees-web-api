using System.Diagnostics.CodeAnalysis;

namespace EmployeesApp.Services.Models
{
    internal readonly struct WorkLog : IEquatable<WorkLog>
    {
        public WorkLog(int employeeId, int projectId, DateOnly workDate)
        {
            EmployeeId = employeeId;
            ProjectId = projectId;
            WorkDate = workDate;
        }

        public int EmployeeId { get; }

        public int ProjectId { get; }

        public DateOnly WorkDate { get; }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is WorkLog other)
            {
                return Equals(other);
            }

            return false;
        }

        public bool Equals(WorkLog other)
        {
            return EmployeeId == other.EmployeeId
                && ProjectId == other.ProjectId
                && WorkDate == other.WorkDate;
        }

        public override int GetHashCode()
            => HashCode.Combine(EmployeeId, ProjectId, WorkDate);
    }
}
