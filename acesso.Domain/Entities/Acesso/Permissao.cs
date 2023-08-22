using DomainLib.Base.Entities;

namespace acesso.Domain.Entities.Acesso
{
    public class Permissao : EntityBase
    {
        public string Nome { get; set; }
        public string Role { get; set; }
    }
}