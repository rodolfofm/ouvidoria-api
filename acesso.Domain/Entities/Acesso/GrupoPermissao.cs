using DomainLib.Base.Entities;

namespace acesso.Domain.Entities.Acesso
{
    public class GrupoPermissao : EntityBase
    {
        public int PermissaoId { get; set; }
        public virtual Permissao Permissao { get; set; }
        public int GrupoId { get; set; }
        public virtual Grupo Grupo { get; set; }

    }
}
