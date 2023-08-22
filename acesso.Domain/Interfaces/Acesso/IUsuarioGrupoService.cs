using DomainLib.Base.ViewModel;
using acesso.Domain.ViewModels.Acesso;
using acesso.Domain.Filtro;

namespace acesso.Domain.Interfaces.Acesso
{
    public interface IUsuarioGrupoService
    {
        Task<UsuarioGrupoDto> AtivarInativar(int id, bool ativo);
        Task<UsuarioGrupoDto> BuscarPorId(int id);
        Task<UsuarioGrupoDto> CadastrarEditar(UsuarioGrupoRequest dto);
        Task<PesquisarResponse<List<UsuarioGrupoResponse>>> Pesquisar(FiltroUsuarioGrupoDto filtro);
        Task<ICollection<KeyValuePair<string, string>>> ValidarCadastrarEditar(UsuarioGrupoRequest dto);
    }
}