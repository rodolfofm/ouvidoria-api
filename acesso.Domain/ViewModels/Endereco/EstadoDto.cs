namespace acesso.Domain.ViewModels.Endereco
{
    public class EstadoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public int CodigoIbge { get; set; }
        public ICollection<CidadeDto> Cidade { get; set; }
    }
}