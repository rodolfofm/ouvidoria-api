using AutoMapper;
using AutenticacaoLib.Core;
using AutenticacaoLib.Model;
using acesso.Domain.Entities.Acesso;
using RepositoryLib;

namespace acesso.Service.Acesso
{
    public class UsuarioAcessoService : IUsuarioAcessoService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UsuarioAcessoService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<UsuarioAcessoDto> BuscarPorId(int id)
        {
            return _mapper.Map<UsuarioAcessoDto>(await _uow.GetRepositoryAsync<UsuarioAcesso>().FindAsync(id));
        }
        public DateTime? BuscarUltimoAcessoPorId(int id)
        {
            var consulta = _uow.GetRepositoryAsync<UsuarioAcesso>().GetQueryable().OrderByDescending(x => x.DataUlitmoAcesso).FirstOrDefault(x => x.UsuarioId == id);
            if (consulta == null)
                return null;
            return consulta.DataUlitmoAcesso;
        }
        public async Task Cadastrar(UsuarioAcessoDto dto)
        {
            var objeto = _mapper.Map<UsuarioAcesso>(dto);
            await _uow.GetRepositoryAsync<UsuarioAcesso>().InsertAsync(objeto);
            await _uow.CommitAsync();
        }

    }
}
