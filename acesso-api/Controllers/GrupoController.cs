using AutenticacaoLib.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using acesso.Domain.Filtro;
using acesso.Domain.Interfaces.Acesso;
using acesso.Domain.ViewModels.Acesso;

namespace acesso_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrupoController : ControllerBase
    {
        private readonly IGrupoService _service;

        public GrupoController(IGrupoService service)
        {
            _service = service;
        }
        [Authorize(Roles = "ADMIN, ROLE_ACESSO_LISTAR")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _service.BuscarPorId(id));
        }
        [Authorize(Roles = "ADMIN, ROLE_ACESSO_GRUPO, ROLE_ACESSO_LISTAR")]
        [HttpPost("pesquisar")]
        public async Task<IActionResult> Pesquisar([FromBody] FiltroGrupoDto filtro, [FromQuery] int pagina, [FromQuery] int quantidade)
        {
            filtro.Pagina = pagina;
            filtro.Quantidade = quantidade;
            return Ok(await _service.Pesquisar(filtro));
        }
        [Authorize(Roles = "ADMIN, ROLE_ACESSO_GRUPO, ROLE_ACESSO_EDITAR")]
        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] GrupoDto dto)
        {
            var validacoes = await _service.ValidarCadastrarEditar(dto);
            foreach (var item in validacoes)
            {
                ModelState.AddModelError(item.Key, item.Value);
            }

            if (!ModelState.IsValid)
            {
                return StatusCode(412, new StatusPreconditionFailed(validacoes));
            }


            return Ok(await _service.CadastrarEditar(dto));
        }
        [Authorize(Roles = "ADMIN, ROLE_ACESSO_GRUPO, ROLE_ACESSO_EDITAR")]
        [HttpPut("{id}/{ativo}")]
        public async Task<IActionResult> AtivarInativar(int id, bool ativo)
        {

            return Ok(await _service.AtivarInativar(id, ativo));
        }
    }
}
