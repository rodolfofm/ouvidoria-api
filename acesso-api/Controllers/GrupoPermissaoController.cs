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
    [Authorize]
    public class GrupoPermissaoController : ControllerBase
    {
        private readonly IGrupoPermissaoService _service;

        public GrupoPermissaoController(IGrupoPermissaoService service)
        {
            _service = service;
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _service.BuscarPorId(id));
        }
        [HttpPost("pesquisar")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Pesquisar([FromBody] FiltroGrupoPermissaoDto filtro, [FromQuery] int pagina, [FromQuery] int quantidade)
        {
            filtro.Pagina = pagina;
            filtro.Quantidade = quantidade;
            return Ok(await _service.Pesquisar(filtro));
        }
        [HttpPut()]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Put([FromBody] GrupoPermissaoRequest dto)
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
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AtivarInativar(int id, bool ativo)
        {

            return Ok(await _service.AtivarInativar(id, ativo));
        }
    }
}
