namespace acesso.Domain.Entities.Endereco
{
    public class Cidade
    {
        public Cidade()
        {
        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public int EstadoId { get; set; }
        public virtual Estado Estado { get; set; }
        public int CodigoIbge { get; set; }
        public string Cep { get; set; }
    }
}