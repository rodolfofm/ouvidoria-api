using DomainLib.Base.ViewModel;

namespace acesso.Domain.ViewModels.Acesso
{
    public class PermissaoDto : EntityBaseDto
    {
        public string? Nome { get; set; }
        public string? Role { get; set; }
    }
}