namespace EmployeesApp.WebApi.Dtos.Output
{
    public class EmployeePairDto
    {
        public int Employee1Id { get; set; }
        public int Employee2Id { get; set; }
        public int DaysWorkedTogether { get; set; }
        public int[]? CommonProjectsIds { get; set; }
    }
}
