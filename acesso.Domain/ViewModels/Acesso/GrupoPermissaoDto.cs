using DomainLib.Base.ViewModel;
using System.Text.Json.Serialization;

namespace acesso.Domain.ViewModels.Acesso
{
    public class GrupoPermissaoDto : EntityBaseDto
    {
        public int PermissaoId { get; set; }
        [JsonIgnore]
        public PermissaoDto Permissao { get; set; }
        public int GrupoId { get; set; }
        [JsonIgnore]
        public GrupoDto Grupo { get; set; }
    }
}