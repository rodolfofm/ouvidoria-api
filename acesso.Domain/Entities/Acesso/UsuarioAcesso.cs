namespace acesso.Domain.Entities.Acesso
{
    public class UsuarioAcesso
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public DateTime DataUlitmoAcesso { get; set; }
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}