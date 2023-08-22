using DomainLib.Base.ViewModel;

namespace acesso.Domain.Filtro
{
    public class FiltroUsuarioGrupoDto : PaginacaoDto
    {
        public int? Id { get; set; }
        public int? UsuarioId { get; set; }
        public int? GrupoId { get; set; }
        public string? UsuarioNome { get; set; }
        public string? GrupoNome { get; set; }
    }
}
