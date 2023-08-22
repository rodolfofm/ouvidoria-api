using DomainLib.Base.ViewModel;
using System.Text.Json.Serialization;

namespace acesso.Domain.ViewModels.Acesso
{
    public class UsuarioGrupoDto : EntityBaseDto
    {
        public int UsuarioId { get; set; }
        [JsonIgnore]
        public UsuarioDto Usuario { get; set; }
        public int GrupoId { get; set; }
        [JsonIgnore]
        public GrupoDto Grupo { get; set; }
    }
}