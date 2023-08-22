using DomainLib.Base.Entities;

namespace acesso.Domain.Entities.Acesso
{
    public class UsuarioGrupo : EntityBase
    {
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public int GrupoId { get; set; }
        public virtual Grupo Grupo { get; set; }
    }
}