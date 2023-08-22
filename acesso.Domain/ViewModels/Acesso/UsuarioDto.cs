using System.Text.Json.Serialization;
using acesso.Domain.ViewModels.Endereco;
using DomainLib.Base.ViewModel;

namespace acesso.Domain.ViewModels.Acesso
{
    public class UsuarioDto : EntityBaseDto
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string? Telefone { get; set; }
        public string Login { get; set; }
        [JsonIgnore]
        public string? Senha { get; set; }
        public string? Cep { get; set; }
        public string? Logradouro { get; set; }
        public string? Complemento { get; set; }
        public string? Numero { get; set; }
        public int CidadeId { get; set; }
        public CidadeDto? Cidade { get; set; }
        public bool EmailValidado { get; set; }
        public bool Administrador { get; set; }

        [JsonIgnore]
        public ICollection<UsuarioGrupoDto>? UsuarioGrupo { get; set; }
    }
}
