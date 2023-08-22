using AutoMapper;
using AutenticacaoLib.Core;
using AutenticacaoLib.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Serilog;

namespace acesso_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SegurancaController : ControllerBase
    {
        private readonly IAuthenticationSession authentication;
        private readonly ISegurancaService segurancaService;

        public SegurancaController(IAuthenticationSession authentication, ISegurancaService segurancaService)
        {
            this.authentication = authentication;
            this.segurancaService = segurancaService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                Log.Error($"Autenticacao invalida usuario {request.UserName} senha {request.Password}");
                return BadRequest();
            }

            var (result, loginResponse) = await authentication.Autenticar(request.UserName, request.Password);
            if (!result)
            {
                Log.Error($"Usuario nao autorizado {request.UserName} senha {request.Password}");
                return Unauthorized();
            }
            Log.Information($"Usuario {request.UserName} autenticado");
            var auth = new KeyValuePair<string, StringValues>("authorization", new StringValues($"Bearer {loginResponse.AccessToken}"));
            HttpContext.Response.Headers.Add(auth);
            auth = new KeyValuePair<string, StringValues>("access-control-expose-headers", new StringValues($"Authorization, Content-Disposition"));
            HttpContext.Response.Headers.Add(auth);
            return Ok();
        }

        [Authorize]
        [HttpGet("autenticacao")]
        public IActionResult GetAuthentication()
        {
            //string token = HttpContext.Request.Headers["Authorization"];
            //if (String.IsNullOrEmpty(token))
            //    return Unauthorized();
            //token = token.Split(' ')[1];
            //var (principal, jwt) = _jwtAuthManager.DecodeJwtToken(token);

            //var loginResponse = authentication.GetAuthentication();
            //var usuario = mapper.Map<UsuarioDto>(await usuarioService.BuscarPorLogin(loginResponse.UserName));

            return Ok(authentication.GetAuthentication());
        }
        [Authorize]
        [HttpGet("logado")]
        public async Task<IActionResult> GetUsuario()
        {
            var loginResponse = authentication.GetAuthentication();
            var usuario = await segurancaService.BuscarResponsePorId(loginResponse.Id);

            return Ok(usuario);

        }

        [Authorize]
        [HttpGet("quantidade/{ativo}")]
        public IActionResult Quantidade(bool ativo)
        {
            int quantidade = segurancaService.QuantidadeUsuarios(ativo);
            return Ok(quantidade);
        }

        [Authorize]
        [HttpGet("ultimosAcessos")]
        public IActionResult UltimosAcessos()
        {
            return Ok(segurancaService.UltimosAcessos());
        }
        [AllowAnonymous]
        [HttpPost("esqueciSenha")]
        public async Task<IActionResult> EsqueciSenha([FromBody] EsqueciSenhaDto request)
        {
            Log.Information($"Email {request.Email} esqueci minha senha");

            var validacoes = await segurancaService.ValidarEsqueciSenha(request);
            foreach (var item in validacoes)
            {
                ModelState.AddModelError(item.Key, item.Value);
            }

            if (!ModelState.IsValid)
            {
                return StatusCode(412, new StatusPreconditionFailed(validacoes));
            }

            await segurancaService.EsqueciSenha(request);

            return Ok();
        }

        //[AllowAnonymous]
        //[HttpPost("atualizar")]
        //public IActionResult Refresh([FromBody] RefreshCredentials refreshCred)
        //{
        //    var token = tokenRefresher.Refresh(refreshCred);

        //    if (token == null)
        //        return Unauthorized();

        //    return Ok(token);
        //}
    }
}
