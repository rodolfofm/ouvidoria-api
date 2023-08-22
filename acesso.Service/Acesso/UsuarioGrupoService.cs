using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RepositoryLib;
using RepositoryLib.DataManagement;
using acesso.Domain.Interfaces.Acesso;
using acesso.Domain.ViewModels.Acesso;
using acesso.Domain.Entities.Acesso;
using acesso.Domain.Filtro;
using AutenticacaoLib.Core;
using DomainLib.Base.ViewModel;

namespace acesso.Service.Acesso
{
    public class UsuarioGrupoService : IUsuarioGrupoService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAuthorizationSession _authorizationSession;

        public UsuarioGrupoService(IUnitOfWork uow, IMapper mapper, IAuthorizationSession authorizationSession)
        {
            _uow = uow;
            _mapper = mapper;
            _authorizationSession = authorizationSession;
        }

        public async Task<UsuarioGrupoDto> AtivarInativar(int id, bool ativo)
        {
            var objeto = await _uow.GetRepositoryAsync<UsuarioGrupo>().SingleOrDefaultAsync(x => x.Id == id);
            objeto.UsuarioAlteracao = _authorizationSession.GetUserId();
            objeto.DataAlteracao = DateTime.Now;
            objeto.Ativo = ativo;
            await _uow.CommitAsync();
            return _mapper.Map<UsuarioGrupoDto>(objeto);
        }

        public async Task<UsuarioGrupoDto> BuscarPorId(int id)
        {
            return _mapper.Map<UsuarioGrupoDto>(await _uow.GetRepositoryAsync<UsuarioGrupo>().FindAsync(id));
        }
        public async Task<PesquisarResponse<List<UsuarioGrupoResponse>>> Pesquisar(FiltroUsuarioGrupoDto filtro)
        {
            var filtros = new Filter<UsuarioGrupo>();

            if (filtro.Ativo.HasValue)
                filtros.And(p => p.Ativo == filtro.Ativo.Value);

            if (filtro.GrupoId.HasValue)
                filtros.And(p => p.GrupoId == filtro.GrupoId.Value);

            if (filtro.UsuarioId.HasValue)
                filtros.And(p => p.UsuarioId == filtro.UsuarioId.Value);

            if (!string.IsNullOrWhiteSpace(filtro.UsuarioNome))
                filtros.And(p => EF.Functions.ILike(p.Usuario.Nome, $"%{filtro.UsuarioNome}%"));

            if (!string.IsNullOrWhiteSpace(filtro.GrupoNome))
                filtros.And(p => EF.Functions.ILike(p.Grupo.Nome, $"%{filtro.GrupoNome}%"));

            var orderby = new OrderBy<UsuarioGrupo>(filtro.Campo, !filtro.Crescente).Expression;

            var consulta = await _uow.GetRepositoryAsync<UsuarioGrupo>().GetListAsync(predicate: filtros.FullExpression,
                                                                                           orderBy: orderby,
                                                                                           include: x => x.Include(a => a.Usuario).Include(a => a.Grupo),
                                                                                           index: filtro.Pagina,
                                                                                           size: filtro.Quantidade);



            var resultado = consulta.Items
               .Select(p =>
               new UsuarioGrupoResponse
               {
                   Id = p.Id,
                   UsuarioId = p.Usuario.Id,
                   UsuarioNome = p.Usuario.Nome,
                   GrupoId = p.Grupo.Id,
                   GrupoNome = p.Grupo.Nome,
                   DataAlteracao = p.DataAlteracao,
                   Ativo = p.Ativo
               }).ToList();
            return new PesquisarResponse<List<UsuarioGrupoResponse>>
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
        public async Task<UsuarioGrupoDto> CadastrarEditar(UsuarioGrupoRequest dto)
        {
            dto.UsuarioAlteracao = _authorizationSession.GetUserId();
            dto.DataAlteracao = DateTime.Now;
            UsuarioGrupo objeto = null;
            if (dto.Id == 0)
            {
                objeto = _mapper.Map<UsuarioGrupo>(dto);
                await _uow.GetRepositoryAsync<UsuarioGrupo>().InsertAsync(objeto);
            }
            else
            {
                objeto = await _uow.GetRepositoryAsync<UsuarioGrupo>().SingleOrDefaultAsync(x => x.Id == dto.Id);
                objeto = _mapper.Map(dto, objeto);
            }
            await _uow.CommitAsync();
            return _mapper.Map<UsuarioGrupoDto>(objeto);
        }

        public async Task<ICollection<KeyValuePair<string, string>>> ValidarCadastrarEditar(UsuarioGrupoRequest dto)
        {
            ICollection<KeyValuePair<string, string>> retorno = new List<KeyValuePair<string, string>>();
            var consulta = await _uow.GetRepositoryAsync<UsuarioGrupo>().SingleOrDefaultAsync(x => dto.Id == 0 && x.UsuarioId == dto.UsuarioId && x.GrupoId == dto.GrupoId);
            if (consulta != null)
            {
                retorno.Add(new KeyValuePair<string, string>("GrupoId", $"O Grupo {dto.GrupoId} já está cadastrada para o respectivo usuário"));
            }
            return retorno;
        }
    }
}
