using DomainLib.Base.ViewModel;
using acesso.Domain.ViewModels.Acesso;
using acesso.Domain.Filtro;

namespace acesso.Domain.Interfaces.Acesso
{
    public interface IGrupoPermissaoService
    {
        Task<GrupoPermissaoDto> BuscarPorId(int id);
        Task<ICollection<KeyValuePair<string, string>>> ValidarCadastrarEditar(GrupoPermissaoRequest dto);
        Task<GrupoPermissaoDto> CadastrarEditar(GrupoPermissaoRequest dto);
        Task<PesquisarResponse<List<GrupoPermissaoResponse>>> Pesquisar(FiltroGrupoPermissaoDto filtro);
        Task<GrupoPermissaoDto> AtivarInativar(int id, bool ativo);
    }
}