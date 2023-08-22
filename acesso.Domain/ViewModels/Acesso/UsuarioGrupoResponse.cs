using DomainLib.Base.ViewModel;

namespace acesso.Domain.ViewModels.Acesso
{
    public class UsuarioGrupoResponse : EntityBaseDto
    {
        public int UsuarioId { get; set; }
        public string UsuarioNome { get; set; }
        public int GrupoId { get; set; }
        public string GrupoNome { get; set; }
    }
}