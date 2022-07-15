using EmployeesApp.Services.Interfaces;
using EmployeesApp.Services.Models;

namespace EmployeesApp.Services
{
    public class EmployeesService : IEmployeesService
    {
        public IEnumerable<EmployeePair> FindPairs(IEnumerable<ValidatedEntry> entries)
        {
            HashSet<WorkLog> workLogs = CreateWorkLogs(entries);
            HashSet<EmployeePair> allEmployeePairs = new();
            List<EmployeePair> employeePairsMaxDays = new();
            int maxDaysWorkedTogether = -1;

            var groups = workLogs.GroupBy(
                wl => (wl.WorkDate, wl.ProjectId),
                wl => wl.EmployeeId);

            foreach (var group in groups)
            {
                if (group.Count() < 2)
                {
                    continue;
                }

                List<int> employeeIdsInGroup = group.ToList();

                for (int i = 0; i < employeeIdsInGroup.Count - 1; i++)
                {
                    for (int j = i + 1; j < employeeIdsInGroup.Count; j++)
                    {
                        EmployeePair employeePair = new(employeeIdsInGroup[i], employeeIdsInGroup[j]);
                        EmployeePair? actualPair;

                        if (!allEmployeePairs.TryGetValue(employeePair, out actualPair))
                        {
                            allEmployeePairs.Add(employeePair);
                            actualPair = employeePair;
                        }

                        actualPair.CommonProjectsIds.Add(group.Key.ProjectId);
                        actualPair.DatesWorkedTogether.Add(group.Key.WorkDate);

                        if (actualPair.DaysWorkedTogether > maxDaysWorkedTogether)
                        {
                            maxDaysWorkedTogether = actualPair.DaysWorkedTogether;
                            employeePairsMaxDays.Clear();
                            employeePairsMaxDays.Add(actualPair);
                        }
                        else if (actualPair.DaysWorkedTogether == maxDaysWorkedTogether)
                        {
                            employeePairsMaxDays.Add(actualPair);
                        }

                    }
                }
            }

            return employeePairsMaxDays;
        }

        private HashSet<WorkLog> CreateWorkLogs(IEnumerable<ValidatedEntry> entries)
        {
            HashSet<WorkLog> workLogs = new();

            foreach (ValidatedEntry entry in entries)
            {
                for (DateOnly currDate = entry.DateFrom; currDate <= entry.DateTo; currDate = currDate.AddDays(1))
                {
                    WorkLog newWorkLog = new(entry.EmployeeId, entry.ProjectId, currDate);
                    workLogs.Add(newWorkLog);
                }
            }

            return workLogs;
        }
    }
}
