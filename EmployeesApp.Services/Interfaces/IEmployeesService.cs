using EmployeesApp.Services.Models;

namespace EmployeesApp.Services.Interfaces
{
    public interface IEmployeesService
    {
        IEnumerable<EmployeePair> FindPairs(IEnumerable<ValidatedEntry> data);
    }
}
