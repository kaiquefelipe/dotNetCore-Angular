using System;

namespace Project.Models.DTO
{
    public class FiltroDTO
    {
        public int? IdProduto { get; set; }
        public string NomeProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public decimal? PrecoProduto { get; set; }
        public Int16? IdCategoria { get; set; }
    }
}
