using DomainLib.Base.ViewModel;

namespace acesso.Domain.Filtro
{
    public class FiltroGrupoPermissaoDto : PaginacaoDto
    {
        public int? Id { get; set; }
        public int? GrupoId { get; set; }
        public string? GrupoNome { get; set; }
        public int? PermissaoId { get; set; }
        public string? PermissaoNome { get; set; }
    }
}
