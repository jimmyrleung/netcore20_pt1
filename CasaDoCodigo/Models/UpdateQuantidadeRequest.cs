using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Models
{
    public class UpdateQuantidadeRequest
    {
        public int itemPedidoId { get; set; }
        public int quantidade { get; set; }
    }
}
