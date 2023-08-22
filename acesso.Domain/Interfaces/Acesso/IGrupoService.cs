using DomainLib.Base.ViewModel;
using acesso.Domain.Filtro;
using acesso.Domain.ViewModels.Acesso;

namespace acesso.Domain.Interfaces.Acesso
{
    public interface IGrupoService
    {
        Task<GrupoDto> BuscarPorId(int id);
        Task<GrupoDto> CadastrarEditar(GrupoDto dto);
        Task<PesquisarResponse<List<GrupoDto>>> Pesquisar(FiltroGrupoDto filtro);
        Task<ICollection<KeyValuePair<string, string>>> ValidarCadastrarEditar(GrupoDto dto);
        Task<GrupoDto> AtivarInativar(int id, bool ativo);
    }
}