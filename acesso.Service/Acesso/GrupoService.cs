using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RepositoryLib;
using RepositoryLib.DataManagement;
using acesso.Domain.Interfaces.Acesso;
using acesso.Domain.ViewModels.Acesso;
using acesso.Domain.Entities.Acesso;
using AutenticacaoLib.Core;
using acesso.Domain.Filtro;
using DomainLib.Base.ViewModel;

namespace acesso.Service.Acesso
{
    public class GrupoService : IGrupoService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAuthorizationSession _authorizationSession;

        public GrupoService(IUnitOfWork uow, IMapper mapper, IAuthorizationSession authorizationSession)
        {
            _uow = uow;
            _mapper = mapper;
            _authorizationSession = authorizationSession;
        }

        public async Task<GrupoDto> AtivarInativar(int id, bool ativo)
        {
            var objeto = await _uow.GetRepositoryAsync<Grupo>().SingleOrDefaultAsync(x => x.Id == id);
            objeto.UsuarioAlteracao = _authorizationSession.GetUserId();
            objeto.DataAlteracao = DateTime.Now;
            objeto.Ativo = ativo;
            await _uow.CommitAsync();
            return _mapper.Map<GrupoDto>(objeto);
        }

        public async Task<GrupoDto> BuscarPorId(int id)
        {
            return _mapper.Map<GrupoDto>(await _uow.GetRepositoryAsync<Grupo>().FindAsync(id));
        }

        public async Task<GrupoDto> CadastrarEditar(GrupoDto dto)
        {
            dto.UsuarioAlteracao = _authorizationSession.GetUserId();
            dto.DataAlteracao = DateTime.Now;
            Grupo objeto = null;
            if (dto.Id == 0)
            {
                objeto = _mapper.Map<Grupo>(dto);
                await _uow.GetRepositoryAsync<Grupo>().InsertAsync(objeto);
            }
            else
            {
                objeto = await _uow.GetRepositoryAsync<Grupo>().SingleOrDefaultAsync(x => x.Id == dto.Id);
                objeto = _mapper.Map(dto, objeto);
            }
            await _uow.CommitAsync();
            return _mapper.Map<GrupoDto>(objeto);
        }

        public async Task<PesquisarResponse<List<GrupoDto>>> Pesquisar(FiltroGrupoDto filtro)
        {
            #region filtros da checkbox

            var filtros = new Filter<Grupo>();

            if (filtro.Ativo.HasValue)
                filtros.And(p => p.Ativo == filtro.Ativo.Value);

            if (filtro.Id.HasValue)
                filtros.And(p => p.Id == filtro.Id);

            if (!string.IsNullOrWhiteSpace(filtro.Nome))
                filtros.And(p => EF.Functions.ILike(p.Nome, $"%{filtro.Nome}%"));


            #endregion
            var orderby = new OrderBy<Grupo>(filtro.Campo, !filtro.Crescente).Expression;

            var consulta = await _uow.GetRepositoryAsync<Grupo>().GetListAsync(predicate: filtros.FullExpression,
                                                                                orderBy: orderby,
                                                                                index: filtro.Pagina,
                                                                                size: filtro.Quantidade);

            var resultado = consulta.Items
                .Select(p =>
                new GrupoDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    DataAlteracao = p.DataAlteracao
                }).ToList();
            return new PesquisarResponse<List<GrupoDto>>
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

        public async Task<ICollection<KeyValuePair<string, string>>> ValidarCadastrarEditar(GrupoDto dto)
        {
            ICollection<KeyValuePair<string, string>> retorno = new List<KeyValuePair<string, string>>();

            var consulta = await _uow.GetRepositoryAsync<Grupo>().SingleOrDefaultAsync(x => x.Id != dto.Id && x.Nome.ToUpper() == dto.Nome);
            if (consulta != null)
            {
                retorno.Add(new KeyValuePair<string, string>("Nome", $"O Grupo {dto.Nome} já está cadastrado."));
            }

            return retorno;
        }
    }
}
