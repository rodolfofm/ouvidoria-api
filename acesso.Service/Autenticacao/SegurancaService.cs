using acesso.Domain.Entities.Acesso;
using acesso.Domain.ViewModels;
using AutenticacaoLib.Core;
using AutenticacaoLib.Model;
using UtilsLib;
using UtilsLib.EmailService.Configuration;
using UtilsLib.EmailService.Interface;
using Microsoft.EntityFrameworkCore;
using RepositoryLib;
using RepositoryLib.DataManagement;

namespace acesso.Service.Autenticacao
{
    public class SegurancaService : ISegurancaService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsuarioAcessoService usuarioAcessoService;
        private readonly IEmailSender emailService;
        private readonly ParametrosAplicacao parametrosAplicacao;
        public SegurancaService(IUnitOfWork uow, IUsuarioAcessoService usuarioAcessoService, IEmailSender emailService, ParametrosAplicacao parametrosAplicacao)
        {
            _uow = uow;
            this.usuarioAcessoService = usuarioAcessoService;
            this.emailService = emailService;
            this.parametrosAplicacao = parametrosAplicacao;
        }
        public async Task<UsuarioResponseDto?> BuscarResponsePorId(int id)
        {
            var query = _uow.GetRepositoryAsync<Usuario>().GetQueryable();
            var usuario = await query.SingleOrDefaultAsync(x => x.Id == id && x.Ativo);
            if (usuario == null)
                return null;

            return MapUsuarioDto(usuario);
        }

        public async Task<UsuarioResponseDto?> ValidarUsuario(string login, string senha)
        {
            if (string.IsNullOrEmpty(senha))
                return null;
            var senhaMd5 = senha.MD5Hash().ToLower();

            var query = _uow.GetRepositoryAsync<Usuario>().GetQueryable();
            var usuario = await query.SingleOrDefaultAsync(x => x.Login == login && x.Senha == senhaMd5 && x.Ativo);

            if (usuario == null)
                return null;

            return MapUsuarioDto(usuario);
        }


        public async Task EsqueciSenha(EsqueciSenhaDto esqueciSenha)
        {
            var usuario = await _uow.GetRepositoryAsync<Usuario>().GetQueryable().Where(x => x.Email == esqueciSenha.Email && x.Ativo).SingleOrDefaultAsync();

            string novaSenha = StringExtensions.GeraSenha();
            string novaSenhaMd5 = novaSenha.MD5Hash().ToLower();
            usuario.Senha = novaSenhaMd5;
           

            string urlAplicacaoWeb = $"<a href='{parametrosAplicacao.UrlFront}'>AQUI</a>";

            string mensagem = $"Olá, {usuario.Nome} <br>";
            mensagem += $"Sua senha foi alterada para : <strong>{novaSenha}</strong> <br>";
            mensagem += $"Clique {urlAplicacaoWeb} e informe seu usuário e senha para acessar sua conta. <br>";
            mensagem += $"Atenciosamente. <br>";
            mensagem += $"Equipe ONESEC.";
            Message message = new(new List<string>() { usuario.Email }.AsEnumerable(), "Esqueci minha senha - ONESEC", mensagem);

            await emailService.SendEmailAsync(message);

            await _uow.CommitAsync();

        }
        public async Task<ICollection<KeyValuePair<string, string>>> ValidarEsqueciSenha(EsqueciSenhaDto dto)
        {
            ICollection<KeyValuePair<string, string>> retorno = new List<KeyValuePair<string, string>>();

            var usuario = await _uow.GetRepositoryAsync<Usuario>().GetQueryable().Where(x => x.Email == dto.Email).SingleOrDefaultAsync();
            if (usuario == null)
            {
                retorno.Add(new KeyValuePair<string, string>("Email", $"Email não encontrado em nossa base de dados"));
                return retorno;
            }
            if (!usuario.Ativo)
                retorno.Add(new KeyValuePair<string, string>("Ativo", $"Usuário com restrição, favor entrar em contato com administradores."));

            return retorno;
        }

        private UsuarioResponseDto MapUsuarioDto(Usuario usuario)
        {
            var dto = new UsuarioResponseDto()
            {
                Id = usuario.Id,
                Cep = usuario.Cep,
                Complemento = usuario.Complemento,
                Cpf = usuario.Cpf,
                Email = usuario.Email,
                Logradouro = usuario.Logradouro,
                Login = usuario.Login,
                CidadeId = usuario.CidadeId,
                Nome = usuario.Nome,
                Telefone = usuario.Telefone,
                Administrador = usuario.Administrador
            };
            dto.DataUltimoAcesso = usuarioAcessoService.BuscarUltimoAcessoPorId(usuario.Id);
            if (dto.Administrador)
                dto.Roles.Add("ADMIN");
            var roles = ObterRolesPorUsuarioId(usuario.Id);
            foreach (var item in roles)
            {
                dto.Roles.Add(item);
            }

            return dto;
        }
        public List<string> ObterRolesPorUsuarioId(int id)
        {
            string sql = @"select distinct p.*
                            from acesso.permissao p 
                                    inner join acesso.grupo_permissao gp on p.id = gp.permissao_id
                                    inner join acesso.grupo g on gp.grupo_id = g.id
                                    inner join acesso.usuario_grupo ug on g.id = ug.grupo_id
                            where p.ativo
                            and gp.ativo
                            and g.ativo
                            and ug.ativo
                            and ug.usuario_id = @p0";
            var consulta = _uow.GetRepositoryAsync<Permissao>().FromSqlRaw(sql, id);
            return consulta.Select(x => x.Role).ToList();
        }

        public List<UltimosAcessoDto> UltimosAcessos()
        {
            string sql = @"select * from 
                            (
                            select 
                                u.id, 
                                u.nome, 
                                u.login,
                                max(ua.data_ulitmo_acesso) as data_ultimo_acesso 
                            from acesso.usuario u inner join acesso.usuario_acesso ua on u.id  = ua.usuario_id
                            group by 
                                u.id, 
                                u.nome, 
                                u.login
                            ) t
                            order by t.data_ultimo_acesso desc
                            limit 10";

            return _uow.GetRepositoryAsync<UltimosAcessoDto>().RawSqlQuery(sql,
                x => new UltimosAcessoDto()
                {
                    Id = (int)x[0],
                    Nome = (string)x[1],
                    Login = (string)x[2],
                    //EquipeId = (int)x[3],
                    //EquipeNome = (string)x[4],
                    DataUltimoAcesso = (DateTime?)x[3]
                });
        }

        public int QuantidadeUsuarios(bool? ativo)
        {
            var filtros = new Filter<Usuario>();

            if (ativo.HasValue)
                filtros.And(x => x.Ativo == ativo.Value);

            return _uow.GetRepositoryAsync<Usuario>().GetQueryable().Where(filtros).Count();
        }
    }
}
