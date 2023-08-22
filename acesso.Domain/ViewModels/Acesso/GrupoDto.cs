using DomainLib.Base.ViewModel;
using System.Text.Json.Serialization;

namespace acesso.Domain.ViewModels.Acesso
{
    public class GrupoDto : EntityBaseDto
    {
        public string? Nome { get; set; }
        [JsonIgnore]
        public ICollection<UsuarioGrupoDto>? UsuarioGrupoVeterinario { get; set; }
        [JsonIgnore]
        public ICollection<GrupoPermissaoDto>? GrupoPermissao { get; set; }
    }
}