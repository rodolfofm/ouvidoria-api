using acesso_api.ViewModel;
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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN,ROLE_ACESSO_USUARIO_LISTAR")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _service.BuscarPorId(id));
        }

        [HttpPost("pesquisar")]
        [Authorize(Roles = "ADMIN,ROLE_ACESSO_USUARIO_LISTAR")]
        public async Task<IActionResult> Pesquisar([FromBody] FiltroUsuarioDto filtro, [FromQuery] int pagina, [FromQuery] int quantidade)
        {
            filtro.Pagina = pagina;
            filtro.Quantidade = quantidade;
            return Ok(await _service.Pesquisar(filtro));
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN,ROLE_ACESSO_USUARIO_EDITAR")]
        public async Task<IActionResult> Put([FromBody] UsuarioDto dto)
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
        [Authorize(Roles = "ADMIN,ROLE_ACESSO_USUARIO_EDITAR")]
        public async Task<IActionResult> AtivarInativar(int id, bool ativo)
        {
            var validacoes = await _service.ValidarAtivarInativar(id);
            foreach (var item in validacoes)
            {
                ModelState.AddModelError(item.Key, item.Value);
            }

            if (!ModelState.IsValid)
            {
                return StatusCode(412, new StatusPreconditionFailed(validacoes));
            }
            return Ok(await _service.AtivarInativar(id, ativo));
        }
        [Authorize]
        [HttpPut("alterarSenha")]
        public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaViewModel formSenha)
        {
            var validacoes = await _service.ValidarAlterarSenha(formSenha.SenhaAtual, formSenha.SenhaNova, formSenha.ConfirmacaoSenhaNova);
            foreach (var item in validacoes)
            {
                ModelState.AddModelError(item.Key, item.Value);
            }

            if (!ModelState.IsValid)
            {
                return StatusCode(412, new StatusPreconditionFailed(validacoes));
            }

            await _service.AlterarSenha(formSenha.SenhaNova);

            return Ok();
        }
    }
}
