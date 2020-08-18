using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.DTO
{
    public class ProdutoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Preco { get; set; }
        public Int16 Quantidade { get; set; }
        public Int16 IdCategoria { get; set; }
        public Categoria? Categoria { get; set; }
    }
}
