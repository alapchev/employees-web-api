using Microsoft.AspNetCore.Mvc;
using EmployeesApp.WebApi.Dtos.Input;
using EmployeesApp.WebApi.Dtos.Output;
using EmployeesApp.Services.Interfaces;
using EmployeesApp.Services.Models;
using EmployeesApp.WebApi.Common;

namespace EmployeesApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [HttpPost]
        public ActionResult<EmployeePairDto[]> FindEmployeePairs(RecordsDto data)
        {
            IEnumerable<ValidatedEntry> validatedData = Helpers.ValidateInput(data);
            IEnumerable<EmployeePair> employeePairsMaxDays = _employeesService.FindPairs(validatedData);
            EmployeePairDto[] results = Helpers.ToEmployeePairDtoArray(employeePairsMaxDays);

            return results;
        }
    }
}
