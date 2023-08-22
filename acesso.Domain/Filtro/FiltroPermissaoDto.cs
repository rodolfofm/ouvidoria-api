using DomainLib.Base.ViewModel;

namespace acesso.Domain.Filtro
{
    public class FiltroPermissaoDto : PaginacaoDto
    {
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public string? Role { get; set; }
    }
}
