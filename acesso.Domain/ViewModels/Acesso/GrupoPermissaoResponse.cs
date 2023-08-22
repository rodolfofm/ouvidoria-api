using DomainLib.Base.ViewModel;

namespace acesso.Domain.ViewModels.Acesso
{
    public class GrupoPermissaoResponse : EntityBaseDto
    {
        public int PermissaoId { get; set; }
        public string PermissaoNome { get; set; }
        public string PermissaoRole { get; set; }
        public int GrupoId { get; set; }
        public string GrupoNome { get; set; }

    }
}