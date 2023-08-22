using AutoMapper;
using AutenticacaoLib.Core;
using DomainLib.Base.ViewModel;
using UtilsLib;
using Microsoft.EntityFrameworkCore;
using acesso.Domain.Entities.Acesso;
using acesso.Domain.Filtro;
using acesso.Domain.Interfaces.Acesso;
using acesso.Domain.ViewModels.Acesso;
using RepositoryLib;
using RepositoryLib.DataManagement;
using System.Text.RegularExpressions;
using UtilsLib.EmailService.Interface;
using UtilsLib.EmailService.Configuration;
using acesso.Domain.ViewModels;

namespace acesso.Service.Acesso
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAuthorizationSession authorizationSession;
        private readonly IEmailSender emailSender;
        private readonly ParametrosAplicacao parametrosAplicacao;
        public UsuarioService(IUnitOfWork uow,
            IMapper mapper,
            IAuthorizationSession authorizationSession,
            IEmailSender emailSender,
            ParametrosAplicacao parametrosAplicacao)
        {
            _uow = uow;
            _mapper = mapper;
            this.authorizationSession = authorizationSession;
            this.emailSender = emailSender;
            this.parametrosAplicacao = parametrosAplicacao;
        }
        public async Task<UsuarioDto> BuscarPorId(int id)
        {
            var query = _uow.GetRepositoryAsync<Usuario>().GetQueryable(include: p => p.Include(p => p.Cidade).ThenInclude(x => x.Estado));
            return _mapper.Map<UsuarioDto>(await query.SingleOrDefaultAsync(x => x.Id == id));
        }

        public async Task<UsuarioDto> CadastrarEditar(UsuarioDto dto)
        {
            dto.UsuarioAlteracao = authorizationSession.GetUserId();
            dto.DataAlteracao = DateTime.Now;
            Usuario objeto = null;
            string Senha = string.Empty;
            if (dto.Id == 0)
            {
                objeto = _mapper.Map<Usuario>(dto);
                ///todo: Enviar senha por email
                Senha = StringExtensions.GeraSenha();
                objeto.Senha = Senha.MD5Hash();
                objeto.EmailValidado = false;
                await _uow.GetRepositoryAsync<Usuario>().InsertAsync(objeto);
            }
            else
            {
                objeto = await _uow.GetRepositoryAsync<Usuario>().SingleOrDefaultAsync(x => x.Id == dto.Id);
                objeto = _mapper.Map(dto, objeto);
            }
            await _uow.CommitAsync();
            if(dto.Id == 0)
                await EnviarEmailNovoUsuario(dto, Senha);

            return _mapper.Map<UsuarioDto>(objeto);
        }

        private async Task EnviarEmailNovoUsuario(UsuarioDto dto, string? Senha)
        {
            string assunto = $"Novo Usuário OneSec";

            string urlAplicacaoWeb = $"<a href='{parametrosAplicacao.UrlFront}'>AQUI</a>";

            string mensagem = $"Olá, {dto.Nome} <br>";
            mensagem += $"Seu usuário foi cadastrada com sucesso!! <br>";
            mensagem += $"Usuario: <strong>{dto.Login}</strong> <br>";
            mensagem += $"Senha: <strong>{Senha}</strong> <br>";
            mensagem += $"Clique {urlAplicacaoWeb} e informe seu usuário e senha para acessar sua conta. <br>";
            mensagem += $"Atenciosamente. <br>";
            mensagem += $"Equipe OneSec.";

#if !DEBUG
            Message message = new(new List<string> { dto.Email }, assunto, mensagem);
#else
            Message message = new(new List<string> { "rodolfofm@gmail.com" }, assunto, mensagem);
#endif

            await this.emailSender.SendEmailAsync(message);
        }

        public async Task<PesquisarResponse<List<UsuarioDto>>> Pesquisar(FiltroUsuarioDto filtro)
        {
            #region filtros da checkbox

            var filtros = new Filter<Usuario>();

            //if (!serviceAuthentication.GetAuthentication().Admin)
            //{
            //    filtros.And(x => x.EquipeId == serviceAuthentication.GetEquipeId() || x.Id == serviceAuthentication.GetUserId());
            //}

            //if (filtro.EquipeId.HasValue)
            //    filtros.And(x => x.EquipeId == filtro.EquipeId);

            if (filtro.GrupoId.HasValue)
            {
                var filtroGrupo = new Filter<UsuarioGrupo>(x => x.Ativo && x.GrupoId == filtro.GrupoId);

                //todo: resolver filtro por grupo
                //filtros.And(x => x.Any(x => x.GrupoId == filtro.GrupoId));
            }

            if (filtro.Id.HasValue)
                filtros.And(p => p.Id == filtro.Id);

            if (!string.IsNullOrWhiteSpace(filtro.Nome))
                filtros.And(p => EF.Functions.ILike(p.Nome, $"%{filtro.Nome}%"));

            if (!string.IsNullOrWhiteSpace(filtro.Cpf.RemoveNaoNumericos()))
                filtros.And(p => p.Cpf.Contains(filtro.Cpf.RemoveNaoNumericos()));

            if (!string.IsNullOrWhiteSpace(filtro.Telefone))
                filtros.And(p => p.Telefone.Contains(filtro.Telefone));

            if (!string.IsNullOrWhiteSpace(filtro.Login))
                filtros.And(p => EF.Functions.ILike(p.Login, $"%{filtro.Login}%"));

            if (!string.IsNullOrWhiteSpace(filtro.Email))
                filtros.And(p => EF.Functions.ILike(p.Email, $"%{filtro.Email}%"));

            #endregion
            var orderby = new OrderBy<Usuario>(filtro.Campo, !filtro.Crescente).Expression;

            var consulta = await _uow.GetRepositoryAsync<Usuario>().GetListAsync(predicate: filtros.FullExpression,
                                                                                orderBy: orderby,
                                                                                include: x => x.Include(x=>x.Cidade),
                                                                                index: filtro.Pagina,
                                                                                size: filtro.Quantidade);

            var resultado = _mapper.Map<List<UsuarioDto>>(consulta.Items);

            return new PesquisarResponse<List<UsuarioDto>>
            {
                Mensagem = "Consulta realizada com sucesso!",
                Registros = resultado,
                Total = consulta.Count,
                HasPrevious = consulta.HasPrevious,
                HasNext = consulta.HasNext,
                Pages = consulta.Pages,
                Size = consulta.Size,
                Index = consulta.Index
                //,From = consulta.From
            };
        }
        public async Task<ICollection<KeyValuePair<string, string>>> ValidarAtivarInativar(int id)
        {
            ICollection<KeyValuePair<string, string>> retorno = new List<KeyValuePair<string, string>>();

            ///TODO: get admin role
            if (!authorizationSession.GetAuthentication().Admin)
            {
                if (authorizationSession.GetAuthentication().Id == id)
                {
                    retorno.Add(new KeyValuePair<string, string>("UsuarioId", $"O Usuário atual não pode ativar ou inativar ele mesmo."));
                }
            }
            return retorno;
        }
        public async Task<ICollection<KeyValuePair<string, string>>> ValidarCadastrarEditar(UsuarioDto dto)
        {
            ICollection<KeyValuePair<string, string>> retorno = new List<KeyValuePair<string, string>>();

            //if (!serviceAuthentication.GetAuthentication().Admin && dto.Id != 0)
            //{
            //    var usuario = await _uow.GetRepositoryAsync<Usuario>().FindAsync(dto.Id);
            //    if (usuario.EquipeId != serviceAuthentication.GetEquipeId())
            //    {
            //        retorno.Add(new KeyValuePair<string, string>("UsuarioId", $"O Usuário só pode alterar os usuários criados por ele."));
            //    }
            //}
            if (!dto.Cpf.ValidaCPF())
            {
                retorno.Add(new KeyValuePair<string, string>("CPF", $"O Cpf {dto.Cpf} é inválido."));
            }
            if (dto.Id == 0)
            {
                if (await _uow.GetRepositoryAsync<Usuario>().AnyAsync(x => x.Cpf == dto.Cpf.RemoveNaoNumericos() && x.Ativo))
                {
                    retorno.Add(new KeyValuePair<string, string>("CPF", $"Já existe um usuário cadastrado para este CPF {dto.Cpf}."));
                }

                if (await _uow.GetRepositoryAsync<Usuario>().AnyAsync(x => x.Email == dto.Email))
                {
                    var str = $"Já existe um usuário cadastrado para este Email {dto.Email} ";
                    str += dto.EmailValidado ? " Validado." : " Não Validado.";
                    retorno.Add(new KeyValuePair<string, string>("Email", str));
                }
            }
            else
            {
                if (await _uow.GetRepositoryAsync<Usuario>().AnyAsync(x => x.Cpf == dto.Cpf.RemoveNaoNumericos() && x.Id != dto.Id && x.Ativo))
                {
                    retorno.Add(new KeyValuePair<string, string>("CPF", $"Já existe um usuário cadastrado para este CPF {dto.Cpf}."));
                }

                if (await _uow.GetRepositoryAsync<Usuario>().AnyAsync(x => x.Email == dto.Email && x.Id != dto.Id && x.Ativo))
                {
                    var str = $"Já existe um usuário cadastrado para este Email {dto.Email} ";
                    str += dto.EmailValidado ? " Validado." : " Não Validado.";
                    retorno.Add(new KeyValuePair<string, string>("Email", str));
                }
            }
            return retorno;
        }
        public async Task<UsuarioDto> AtivarInativar(int id, bool ativo)
        {
            var objeto = await _uow.GetRepositoryAsync<Usuario>().SingleOrDefaultAsync(x => x.Id == id);
            objeto.UsuarioAlteracao = authorizationSession.GetUserId();
            objeto.DataAlteracao = DateTime.Now;
            objeto.Ativo = ativo;
            await _uow.CommitAsync();
            return _mapper.Map<UsuarioDto>(objeto);
        }
        public async Task<ICollection<KeyValuePair<string, string>>> ValidarAlterarSenha(string senhaAtual, string novaSenha, string confirmacaoNovaSenha)
        {
            ICollection<KeyValuePair<string, string>> retorno = new List<KeyValuePair<string, string>>();

            var usuarioId = authorizationSession.GetUserId();

            var senhaMd5 = senhaAtual.MD5Hash();
            // Log.Information($"atual: {senhaAtual} md5: ${senhaMd5} id: {usuarioId}");
            var query = _uow.GetRepositoryAsync<Usuario>().GetQueryable();
            var usuario = await query.SingleOrDefaultAsync(x => x.Id == usuarioId && x.Senha == senhaMd5 && x.Ativo);

            if (usuario == null)
            {
                retorno.Add(new KeyValuePair<string, string>("senhaAtual", $"Senha atual inválida."));
                return retorno;
            }
            if (senhaAtual.Equals(novaSenha))
            {
                retorno.Add(new KeyValuePair<string, string>("novaSenha", $"A nova senha deve ser diferente da senha atual."));
            }
            if (!confirmacaoNovaSenha.Equals(novaSenha))
            {
                retorno.Add(new KeyValuePair<string, string>("confirmacaoNovaSenha", $"Confirmação de nova senha não confere."));
            }
            if (!ValidatePassword(novaSenha, out string ErrorMessage))
            {
                retorno.Add(new KeyValuePair<string, string>("novaSenha", ErrorMessage));
            }

            return retorno;
        }
        public async Task AlterarSenha(string novaSenha)
        {
            var usuarioId = authorizationSession.GetUserId();

            var query = _uow.GetRepositoryAsync<Usuario>().GetQueryable();

            var usuario = await query.SingleOrDefaultAsync(x => x.Id == usuarioId && x.Ativo);

            string senhaMd5 = novaSenha.MD5Hash();
            usuario.Senha = senhaMd5;
            await _uow.CommitAsync();
        }
        private static bool ValidatePassword(string password, out string ErrorMessage)
        {
            var input = password;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new Exception("Senha não pode ser em branco.");
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(input))
            {
                ErrorMessage = "Senha deve ter pelo menos 1 caracter minúsculo.";
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                ErrorMessage = "Senha deve ter pelo menos 1 caracter maiúsculo.";
                return false;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                ErrorMessage = "Senha deve ter no mínimo 8 caracteres e no máximo 15 caracteres.";
                return false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                ErrorMessage = "Senha deve ter pelo menos um número";
                return false;
            }

            else if (!hasSymbols.IsMatch(input))
            {
                ErrorMessage = "Senha deve ter pelo menos um caracter especial ex: @, #, $, %, &, * ...";
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
