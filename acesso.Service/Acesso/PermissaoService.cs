using AutoMapper;
using RepositoryLib;
using RepositoryLib.DataManagement;
using Microsoft.EntityFrameworkCore;
using acesso.Domain.Interfaces.Acesso;
using acesso.Domain.ViewModels.Acesso;
using acesso.Domain.Entities.Acesso;
using AutenticacaoLib.Core;
using acesso.Domain.Filtro;
using DomainLib.Base.ViewModel;

namespace acesso.Service.Acesso
{
    public class PermissaoService : IPermissaoService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAuthorizationSession _authorizationSession;

        public PermissaoService(IUnitOfWork uow, IMapper mapper, IAuthorizationSession authorizationSession)
        {
            _uow = uow;
            _mapper = mapper;
            _authorizationSession = authorizationSession;
        }

        public async Task<PermissaoDto> BuscarPorId(int id)
        {
            return _mapper.Map<PermissaoDto>(await _uow.GetRepositoryAsync<Permissao>().FindAsync(id));
        }

        public async Task<PermissaoDto> CriarEditar(PermissaoDto dto)
        {
            dto.UsuarioAlteracao = _authorizationSession.GetUserId();
            dto.DataAlteracao = DateTime.Now;

            Permissao objeto = null;

            if (dto.Id == 0)
            {
                objeto = _mapper.Map<Permissao>(dto);
                await _uow.GetRepositoryAsync<Permissao>().InsertAsync(objeto);
            }
            else
            {
                objeto = await _uow.GetRepositoryAsync<Permissao>().SingleOrDefaultAsync(x => x.Id == dto.Id);
                objeto = _mapper.Map(dto, objeto);
            }
            await _uow.CommitAsync();
            return _mapper.Map<PermissaoDto>(objeto);
        }
        public async Task<PermissaoDto> AtivarInativar(int id, bool ativo)
        {
            var objeto = await _uow.GetRepositoryAsync<Permissao>().SingleOrDefaultAsync(x => x.Id == id);
            objeto.UsuarioAlteracao = _authorizationSession.GetUserId();
            objeto.DataAlteracao = DateTime.Now;
            objeto.Ativo = ativo;
            await _uow.CommitAsync();
            return _mapper.Map<PermissaoDto>(objeto);
        }
        public async Task<PesquisarResponse<List<PermissaoDto>>> Pesquisar(FiltroPermissaoDto filtro)
        {
            #region filtros da checkbox

            var filtros = new Filter<Permissao>();

            if (filtro.Id.HasValue)
            {
                filtros.And(p => p.Id == filtro.Id.Value);
            }

            if (filtro.Ativo.HasValue)
            {
                filtros.And(p => p.Ativo == filtro.Ativo.Value);
            }

            if (!string.IsNullOrWhiteSpace(filtro.Nome))
            {
                filtros.And(p => EF.Functions.ILike(p.Nome, $"%{filtro.Nome}%"));
            }

            if (!string.IsNullOrWhiteSpace(filtro.Role))
            {
                filtros.And(p => EF.Functions.ILike(p.Role, $"%{filtro.Role}%"));
            }


            #endregion
            var orderby = new OrderBy<Permissao>(filtro.Campo, !filtro.Crescente).Expression;

            var consulta = await _uow.GetRepositoryAsync<Permissao>().GetListAsync(predicate: filtros.FullExpression,
                                                                                orderBy: orderby,
                                                                                index: filtro.Pagina,
                                                                                size: filtro.Quantidade);

            var resultado = consulta.Items
                .Select(p =>
                new PermissaoDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Role = p.Role,
                    DataAlteracao = p.DataAlteracao
                }).ToList();
            return new PesquisarResponse<List<PermissaoDto>>
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

        public async Task<ICollection<KeyValuePair<string, string>>> ValidarCadastrar(PermissaoDto dto)
        {
            ICollection<KeyValuePair<string, string>> retorno = new List<KeyValuePair<string, string>>();
            var consulta = await _uow.GetRepositoryAsync<Permissao>().SingleOrDefaultAsync(x => x.Role.ToUpper() == dto.Role);
            if (consulta != null)
            {
                retorno.Add(new KeyValuePair<string, string>("ROLE", $"A Role {dto.Role} já está cadastrado."));
            }
            return retorno;
        }
    }
}
