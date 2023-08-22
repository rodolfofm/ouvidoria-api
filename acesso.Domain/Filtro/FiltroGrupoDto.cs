using DomainLib.Base.ViewModel;

namespace acesso.Domain.Filtro
{
    public class FiltroGrupoDto : PaginacaoDto
    {
        public int? Id { get; set; }
        public string? Nome { get; set; }
    }
}
