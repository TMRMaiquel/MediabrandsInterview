using INT.Application.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace INT.Distributed.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/admin/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        #region Miembros

        public IEmployeeService EmployeeService { get; }

        #endregion

        #region Constructor

        public EmployeeController(IEmployeeService employeeService)
        {
            this.EmployeeService = employeeService;
        }

        #endregion

        #region Métodos

        [HttpGet]
        [Route("getDependencies")]
        public async Task<IActionResult> GetDependencies([FromQuery] int id)
        {
            try
            {
                var result = await this.EmployeeService.GetDependencies(id);
                return Ok(new { dependencies = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("insert")]
        public async Task<IActionResult> Insert([FromBody] JObject objectJSON)
        {
            try
            {
                var result = await this.EmployeeService.Insert(objectJSON);
                return Ok(new { employee = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] JObject objectJSON)
        {
            try
            {
                var result = await this.EmployeeService.Update(objectJSON);
                return Ok(new { employee = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] JObject objectJSON)
        {
            try
            {
                var result = await this.EmployeeService.Delete(objectJSON);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("getByParameters")]
        public async Task<IActionResult> GetByParameters([FromBody] JObject objectJSON)
        {
            try
            {
                var result = await this.EmployeeService.GetByParameters(objectJSON);
                return Ok(new { records = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
