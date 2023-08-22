using AutoMapper;
using DomainLib.Base.ViewModel;
using RepositoryLib.DataManagement;
using RepositoryLib;
using acesso.Domain.Interfaces.Endereco;
using acesso.Domain.ViewModels.Endereco;
using acesso.Domain.Entities.Endereco;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace acesso.Service.Endereco
{
    public class CidadeService : ICidadeService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CidadeService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<CidadeDto> BuscarPorId(int id)
        {
            //return _mapper.Map<CidadeDto>(await _uow.GetRepositoryAsync<Cidade>().FindAsync(id));
            var query = _uow.GetRepositoryAsync<Cidade>().GetQueryable(include: p => p.Include(p => p.Estado));
            return _mapper.Map<CidadeDto>(await query.SingleOrDefaultAsync(x => x.Id == id));
        }
        public async Task<List<CidadeDto>> ListarPorEstadoSigla(string Sigla)
        {
            var query = await _uow.GetRepositoryAsync<Cidade>().GetQueryable().Where(x=> x.Estado.Sigla == Sigla).ToListAsync();
            return _mapper.Map<List<CidadeDto>>(query);
        }

        public async Task<PesquisarResponse<List<CidadeDto>>> Pesquisar(FiltroPadraoDto filtro)
        {
            #region filtros da checkbox

            var filtros = new Filter<Cidade>();

            if (filtro.Id.HasValue)
                filtros.And(p => p.Id == filtro.Id);

            if (!string.IsNullOrWhiteSpace(filtro.Nome))
                filtros.And(p => EF.Functions.ILike(p.Nome, $"%{filtro.Nome}%"));


            #endregion
            var orderby = new OrderBy<Cidade>(filtro.Campo, !filtro.Crescente).Expression;

            var consulta = await _uow.GetRepositoryAsync<Cidade>().GetListAsync(predicate: filtros.FullExpression,
                                                                                orderBy: orderby,
                                                                                include: x => x.Include(x => x.Estado),
                                                                                index: filtro.Pagina,
                                                                                size: filtro.Quantidade);

            var resultado = consulta.Items
                .Select(p =>
                new CidadeDto
                {
                    Id = p.Id,
                    Nome = p.Nome + " / " + p.Estado.Sigla,
                }).ToList();
            return new PesquisarResponse<List<CidadeDto>>
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

    }
}
