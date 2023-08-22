using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RepositoryLib;
using RepositoryLib.DataManagement;
using acesso.Domain.Interfaces.Acesso;
using AutenticacaoLib.Core;
using acesso.Domain.ViewModels;
using acesso.Domain.ViewModels.Acesso;
using acesso.Domain.Entities.Acesso;
using acesso.Domain.Filtro;
using DomainLib.Base.ViewModel;

namespace acesso.Service.Acesso
{
    public class GrupoPermissaoService : IGrupoPermissaoService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAuthorizationSession _authorizationSession;

        public GrupoPermissaoService(IUnitOfWork uow, IMapper mapper, IAuthorizationSession authorizationSession)
        {
            _uow = uow;
            _mapper = mapper;
            _authorizationSession = authorizationSession;
        }

        public async Task<GrupoPermissaoDto> BuscarPorId(int id)
        {
            return _mapper.Map<GrupoPermissaoDto>(await _uow.GetRepositoryAsync<GrupoPermissao>().FindAsync(id));
        }
        public async Task<PesquisarResponse<List<GrupoPermissaoResponse>>> Pesquisar(FiltroGrupoPermissaoDto filtro)
        {
            #region filtros da checkbox

            var filtros = new Filter<GrupoPermissao>();

            if (filtro.Ativo.HasValue)
                filtros.And(p => p.Ativo == filtro.Ativo.Value);

            if (filtro.Id.HasValue)
                filtros.And(p => p.Id == filtro.Id);

            if (filtro.GrupoId.HasValue)
                filtros.And(p => p.GrupoId == filtro.GrupoId);

            if (!string.IsNullOrWhiteSpace(filtro.GrupoNome))
                filtros.And(p => p.Grupo.Nome.Contains(filtro.GrupoNome));

            if (filtro.PermissaoId.HasValue)
                filtros.And(p => p.PermissaoId == filtro.PermissaoId);

            if (!string.IsNullOrWhiteSpace(filtro.PermissaoNome))
                filtros.And(p => p.Permissao.Nome.Contains(filtro.PermissaoNome));

            #endregion
            var orderby = new OrderBy<GrupoPermissao>(filtro.Campo, !filtro.Crescente).Expression;

            var consulta = await _uow.GetRepositoryAsync<GrupoPermissao>().GetListAsync(predicate: filtros.FullExpression,
                                                                                orderBy: orderby,
                                                                                include: p => p.Include(p => p.Grupo).Include(p => p.Permissao),
                                                                                index: filtro.Pagina,
                                                                                size: filtro.Quantidade);

            var resultado = consulta.Items
                .Select(p =>
                new GrupoPermissaoResponse
                {
                    Id = p.Id,
                    GrupoId = p.GrupoId,
                    GrupoNome = p.Grupo.Nome,
                    PermissaoId = p.PermissaoId,
                    PermissaoNome = p.Permissao.Nome,
                    PermissaoRole = p.Permissao.Role,
                    DataAlteracao = p.DataAlteracao,
                    Ativo = p.Ativo
                }).ToList();
            return new PesquisarResponse<List<GrupoPermissaoResponse>>
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
        public async Task<GrupoPermissaoDto> CadastrarEditar(GrupoPermissaoRequest dto)
        {
            dto.UsuarioAlteracao = _authorizationSession.GetUserId();
            dto.DataAlteracao = DateTime.Now;
            GrupoPermissao objeto = null;
            if (dto.Id == 0)
            {
                objeto = _mapper.Map<GrupoPermissao>(dto);
                await _uow.GetRepositoryAsync<GrupoPermissao>().InsertAsync(objeto);
            }
            else
            {
                objeto = await _uow.GetRepositoryAsync<GrupoPermissao>().SingleOrDefaultAsync(x => x.Id == dto.Id);
                objeto = _mapper.Map(dto, objeto);
            }
            await _uow.CommitAsync();
            return _mapper.Map<GrupoPermissaoDto>(objeto);
        }
        public async Task<ICollection<KeyValuePair<string, string>>> ValidarCadastrarEditar(GrupoPermissaoRequest dto)
        {
            ICollection<KeyValuePair<string, string>> retorno = new List<KeyValuePair<string, string>>();
            var consulta = await _uow.GetRepositoryAsync<GrupoPermissao>().SingleOrDefaultAsync(x => x.PermissaoId == dto.PermissaoId && x.GrupoId == dto.GrupoId && dto.Id == 0);
            if (consulta != null)
            {
                retorno.Add(new KeyValuePair<string, string>("PermissaoId", $"A Permissão {consulta.Permissao.Nome} já está cadastrada."));
            }
            return retorno;
        }
        public async Task<GrupoPermissaoDto> AtivarInativar(int id, bool ativo)
        {
            var objeto = await _uow.GetRepositoryAsync<GrupoPermissao>().SingleOrDefaultAsync(x => x.Id == id);
            objeto.UsuarioAlteracao = _authorizationSession.GetUserId();
            objeto.DataAlteracao = DateTime.Now;
            objeto.Ativo = ativo;
            await _uow.CommitAsync();
            return _mapper.Map<GrupoPermissaoDto>(objeto);
        }
    }
}
