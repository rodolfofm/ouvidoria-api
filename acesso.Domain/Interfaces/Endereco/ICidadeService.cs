using acesso.Domain.ViewModels.Endereco;
using DomainLib.Base.ViewModel;

namespace acesso.Domain.Interfaces.Endereco
{
    public interface ICidadeService
    {
        Task<CidadeDto> BuscarPorId(int id);
        Task<List<CidadeDto>> ListarPorEstadoSigla(string Sigla);
        Task<PesquisarResponse<List<CidadeDto>>> Pesquisar(FiltroPadraoDto filtro);

    }
}
