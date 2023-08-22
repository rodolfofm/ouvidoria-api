using DomainLib.Base.ViewModel;

namespace acesso.Domain.ViewModels.Acesso
{
    public class GrupoPermissaoRequest : EntityBaseDto
    {

        public int GrupoId { get; set; }
        public int PermissaoId { get; set; }
        public bool? sistemaInsercao { get; set; }
    }
}
