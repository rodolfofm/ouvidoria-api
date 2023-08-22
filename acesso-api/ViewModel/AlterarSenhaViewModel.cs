using System.ComponentModel.DataAnnotations;

namespace acesso_api.ViewModel
{
    public class AlterarSenhaViewModel
    {
        [Required]
        public string? SenhaAtual { get; set; }
        [Required]
        public string? SenhaNova { get; set; }
        [Required]
        public string? ConfirmacaoSenhaNova { get; set; }

    }
}
