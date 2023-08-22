using acesso.Domain.Interfaces.Endereco;
using DomainLib.Base.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace acesso_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CidadeController : Controller
    {
        private readonly ICidadeService _service;

        public CidadeController(ICidadeService service)
        {
            _service = service;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _service.BuscarPorId(id));
        }
        [HttpPost("pesquisar")]
        public async Task<IActionResult> Pesquisar([FromBody] FiltroPadraoDto filtro, [FromQuery] int pagina, [FromQuery] int quantidade)
        {
            filtro.Pagina = pagina;
            filtro.Quantidade = quantidade;
            return Ok(await _service.Pesquisar(filtro));
        }
        [HttpGet("listar/{sigla}")]
        public async Task<IActionResult> Listar([FromRoute] string Sigla)
        {
            return Ok(await _service.ListarPorEstadoSigla(Sigla));
        }
    }
}
