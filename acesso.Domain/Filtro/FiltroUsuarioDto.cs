using DomainLib.Base.ViewModel;

namespace acesso.Domain.Filtro
{
    public class FiltroUsuarioDto : PaginacaoDto
    {
        public int? EquipeId { get; set; }
        public int? GrupoId { get; set; }
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Login { get; set; }
    }
}
