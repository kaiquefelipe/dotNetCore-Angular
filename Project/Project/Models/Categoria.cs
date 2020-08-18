using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Categoria
    {
        private readonly Categoria Entidade;
        public Categoria() { }
        public Categoria (Int16 id) { Id = id; }
        public Categoria(Categoria entidade) { Entidade = entidade; }

        public Int16 Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}
