using acesso.Domain.ViewModels.Acesso;
using System.Text.Json.Serialization;

namespace acesso.Domain.ViewModels.Endereco
{
    public class CidadeDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int EstadoId { get; set; }
        public int CodigoIbge { get; set; }
        public string Cep { get; set; }
        [JsonIgnore]
        public ICollection<UsuarioDto> Usuario { get; set; }
    }
}
