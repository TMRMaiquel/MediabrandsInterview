using INT.Application.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace INT.Distributed.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/admin/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        #region Miembros

        public IUsuarioService UsuarioService { get; }

        #endregion

        #region Constructor

        public UsuarioController(IUsuarioService usuarioService)
        {
            this.UsuarioService = usuarioService;
        }

        #endregion

        #region Métodos

        [HttpGet]
        [Route("getDependencies")]
        public async Task<IActionResult> GetDependencies([FromQuery] int id)
        {
            try
            {
                var result = await this.UsuarioService.GetDependencies(id);
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
                var result = await this.UsuarioService.Insert(objectJSON);
                return Ok(new { usuario = result });
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
                var result = await this.UsuarioService.Update(objectJSON);
                return Ok(new { usuario = result });
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
                var result = await this.UsuarioService.Delete(objectJSON);
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
                var result = await this.UsuarioService.GetByParameters(objectJSON);
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
