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
    [Authorize(Roles = "ADMIN")]
    public class UsuarioGrupoController : ControllerBase
    {
        private readonly IUsuarioGrupoService _service;

        public UsuarioGrupoController(IUsuarioGrupoService service)
        {
            _service = service;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _service.BuscarPorId(id));
        }
        [HttpPost("pesquisar")]
        public async Task<IActionResult> Pesquisar([FromBody] FiltroUsuarioGrupoDto filtro, [FromQuery] int pagina, [FromQuery] int quantidade)
        {
            filtro.Pagina = pagina;
            filtro.Quantidade = quantidade;
            return Ok(await _service.Pesquisar(filtro));
        }

        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] UsuarioGrupoRequest dto)
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
        [HttpPut("{id}/{ativo}")]
        public async Task<IActionResult> AtivarInativar(int id, bool ativo)
        {

            return Ok(await _service.AtivarInativar(id, ativo));
        }
    }
}
