using acesso.Domain.Entities.Endereco;
using DomainLib.Base.Entities;

namespace acesso.Domain.Entities.Acesso
{
    public class Usuario : EntityBase
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Numero { get; set; }
        public int CidadeId { get; set; }
        public virtual Cidade Cidade { get; set; }
        public bool EmailValidado { get; set; }
        public bool Administrador { get; set; }
    }
}