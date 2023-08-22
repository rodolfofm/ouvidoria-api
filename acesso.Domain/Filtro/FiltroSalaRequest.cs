using DomainLib.Base.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acesso.Domain.Filtro
{
    public class FiltroSalaRequest : PaginacaoDto
    {
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public string? Subsecao { get; set; }
        public string? Dns { get; set; }
    }
}
