using DomainLib.Base.ViewModel;
using acesso.Domain.ViewModels.Acesso;
using acesso.Domain.Filtro;

namespace acesso.Domain.Interfaces.Acesso
{
    public interface IUsuarioService
    {
        Task<UsuarioDto> BuscarPorId(int id);
        Task<PesquisarResponse<List<UsuarioDto>>> Pesquisar(FiltroUsuarioDto filtro);
        Task<ICollection<KeyValuePair<string, string>>> ValidarCadastrarEditar(UsuarioDto dto);
        Task<UsuarioDto> CadastrarEditar(UsuarioDto dto);
        Task<UsuarioDto> AtivarInativar(int id, bool ativo);
        Task<ICollection<KeyValuePair<string, string>>> ValidarAtivarInativar(int id);
        Task AlterarSenha(string novaSenha);
        Task<ICollection<KeyValuePair<string, string>>> ValidarAlterarSenha(string senhaAtual, string novaSenha, string confirmacaoNovaSenha);
    }
}
