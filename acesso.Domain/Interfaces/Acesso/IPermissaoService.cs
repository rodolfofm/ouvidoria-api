using DomainLib.Base.ViewModel;
using acesso.Domain.Filtro;
using acesso.Domain.ViewModels.Acesso;

namespace acesso.Domain.Interfaces.Acesso
{
    public interface IPermissaoService
    {
        Task<PermissaoDto> BuscarPorId(int id);
        Task<ICollection<KeyValuePair<string, string>>> ValidarCadastrar(PermissaoDto dto);
        Task<PermissaoDto> CriarEditar(PermissaoDto dto);
        Task<PesquisarResponse<List<PermissaoDto>>> Pesquisar(FiltroPermissaoDto filtro);
        Task<PermissaoDto> AtivarInativar(int id, bool ativo);
    }
}
