using EmployeesApp.Services.Models;
using EmployeesApp.WebApi.Dtos.Input;
using EmployeesApp.WebApi.Dtos.Output;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace EmployeesApp.WebApi.Common
{
    internal static class Helpers
    {
        private static readonly string[] _dateFormats =
        {
            "yyyy-M-d",
            "d-M-yyyy",
            "d/M/yyyy",
            "d.M.yyyy",
            "yyyyMMdd"
        };

        public static IEnumerable<ValidatedEntry> ValidateInput(RecordsDto input)
        {
            List<ValidatedEntry> validatedEntries = new();

            if (input.Records is not null)
            {
                foreach (EntryDto entry in input.Records)
                {
                    if (!TryValidateEntry(entry, out ValidatedEntry? validated))
                    {
                        continue;
                    }

                    validatedEntries.Add(validated);
                }
            }

            return validatedEntries;
        }

        public static EmployeePairDto[] ToEmployeePairDtoArray(IEnumerable<EmployeePair> employeePairs)
        {
            EmployeePairDto[] dtoArray = new EmployeePairDto[employeePairs.Count()];
            IEnumerable<EmployeePair> sortedPairs = employeePairs
                .OrderBy(ep => ep.Employee1Id)
                .ThenBy(ep => ep.Employee2Id);

            int i = 0;
            foreach (EmployeePair pair in sortedPairs)
            {
                EmployeePairDto dto = new();
                dto.Employee1Id = pair.Employee1Id;
                dto.Employee2Id = pair.Employee2Id;
                dto.DaysWorkedTogether = pair.DaysWorkedTogether;
                dto.CommonProjectsIds = pair.CommonProjectsIds.OrderBy(x => x).ToArray();
                dtoArray[i++] = dto;
            }

            return dtoArray;
        }

        private static bool TryValidateEntry(EntryDto entry, [NotNullWhen(true)] out ValidatedEntry? validated)
        {
            int employeeId;
            int projectId;
            DateOnly dateFrom;
            DateOnly dateTo;
            validated = null;

            if (entry.EmployeeId is null
                || entry.ProjectId is null
                || entry.DateFrom is null
                || entry.DateTo is null)
            {
                return false;
            }

            if (!int.TryParse(entry.EmployeeId, out employeeId)
                || !int.TryParse(entry.ProjectId, out projectId)
                || !DateOnly.TryParseExact(entry.DateFrom, _dateFormats, CultureInfo.InvariantCulture,
                        DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite, out dateFrom))
            {
                return false;
            }

            if (entry.DateTo.Trim().ToUpperInvariant() == "NULL")
            {
                dateTo = DateOnly.FromDateTime(DateTime.Today);
            }
            else if (!DateOnly.TryParseExact(entry.DateTo, _dateFormats, CultureInfo.InvariantCulture,
                        DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite, out dateTo))
            {
                return false;
            }

            if (dateFrom > dateTo)
            {
                return false;
            }

            validated = new ValidatedEntry(employeeId, projectId, dateFrom, dateTo);
            
            return true;
        }
    }
}
