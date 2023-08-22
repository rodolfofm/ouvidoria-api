using DomainLib.Base.ViewModel;

namespace acesso.Domain.ViewModels.Acesso
{
    public class UsuarioGrupoRequest : EntityBaseDto
    {
        public int GrupoId { get; set; }
        public int UsuarioId { get; set; }
    }
}