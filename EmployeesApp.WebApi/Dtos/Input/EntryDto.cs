namespace EmployeesApp.WebApi.Dtos.Input
{
    public class EntryDto
    {
        public string? EmployeeId { get; set; }
        public string? ProjectId { get; set; }
        public string? DateFrom { get; set; }
        public string? DateTo { get; set; }
    }
}
