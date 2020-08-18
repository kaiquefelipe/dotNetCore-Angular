using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project.Data;
using Project.Models;
using Project.Models.DTO;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ProjectContext _context;
        private readonly IDataRepository<Produto> _repo;

        public ProdutosController(ProjectContext context, IDataRepository<Produto> repo)
        {
            _context = context;
            _repo = repo;
        }

        // GET: api/Produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProduto()
        {
            return await _context.Produto.Include(x => x.Categoria).ToListAsync();
        }

        // GET: api/Produtos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var produto = await _context.Produto.Include(x => x.Categoria).SingleOrDefaultAsync(i => i.Id == id);

            if (produto == null)
                return NotFound();

            return produto;
        }

        // GET: api/Produtos
        [HttpGet("buscarporfiltro")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutoFilter(FiltroDTO filtro)
        {
            return await _context.Produto.Include(cat => cat.Categoria).Where(
                produto =>    (!filtro.IdProduto.HasValue || produto.Id == filtro.IdProduto.Value) &&
                        (string.IsNullOrEmpty(filtro.NomeProduto) || produto.Nome == filtro.NomeProduto) &&
                        (string.IsNullOrEmpty(filtro.DescricaoProduto) || produto.Descricao == filtro.DescricaoProduto) &&
                        (!filtro.PrecoProduto.HasValue || produto.Preco == filtro.PrecoProduto.Value) &&
                        (!filtro.IdCategoria.HasValue || produto.Categoria.Id == filtro.IdCategoria.Value)
            ).ToListAsync();
        }

        // PUT: api/Produtos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.Id)
                return BadRequest();

            var produto = await _context.Produto.FindAsync(id);
            var categoria = await _context.Categoria.FindAsync(produtoDTO.IdCategoria);
            var listaErros = new List<RetornoErrosDTO>();

            if (produto == null || categoria == null)
                return BadRequest();

            produtoDTO.Categoria = categoria;

            try
            {
                produtoDTO.Preco = produtoDTO.Preco.Replace(",", "").Replace(".", "");
                if ((Convert.ToDecimal(produtoDTO.Preco) / 100) > 0)
                    produtoDTO.Preco = decimal.Parse(string.Format("{0:#.00}", Convert.ToDecimal(produtoDTO.Preco) / 100)).ToString();

            }
            catch (InvalidCastException)
            {
                listaErros.Add(new RetornoErrosDTO() { Key = "preco", Value = "Preço inválido!" });
                return BadRequest(JsonConvert.SerializeObject(listaErros));
            }

            var validator = new ProdutoValidator();
            var isValid = validator.Validate(produto);
            if (!isValid.IsValid)
            {
                isValid.Errors.ToList().ForEach(erro => listaErros.Add(new RetornoErrosDTO() { Key = erro.PropertyName.ToLower(), Value = erro.ErrorMessage.Replace("\"", "'") }));
                return BadRequest(JsonConvert.SerializeObject(listaErros));
            }

            produto = ParseToProduct(produtoDTO, ref produto);
            _context.Entry(produto).State = EntityState.Modified;
            try
            {
                _repo.Update(produto);
                await _repo.SaveAsync(produto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Produtos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(ProdutoDTO produtoDTO)
        {
            decimal precoResult = 0;
            var listaErros = new List<RetornoErrosDTO>();
            produtoDTO.Preco = produtoDTO.Preco.Replace(",", "").Replace(".", "");
            try
            {
                if ((Convert.ToDecimal(produtoDTO.Preco) / 100) > 0)
                    precoResult = decimal.Parse(string.Format("{0:#.00}", Convert.ToDecimal(produtoDTO.Preco) / 100));
            }
            catch (InvalidCastException)
            {
                listaErros.Add(new RetornoErrosDTO() { Key = "preco", Value = "Preço inválido!" });
                return BadRequest(JsonConvert.SerializeObject(listaErros));
            }
            

            var produto = new Produto()
            {
                Nome = produtoDTO.Nome,
                Descricao = produtoDTO.Descricao,
                Preco = precoResult,
                Quantidade = produtoDTO.Quantidade
            };
            var categoria = await _context.Categoria.FindAsync(produtoDTO.IdCategoria);
            if (categoria != null)
                produto.Categoria = categoria;
            else
                return NotFound();

            var validator = new ProdutoValidator();
            var isValid = validator.Validate(produto);
            if (!isValid.IsValid)
            {
                isValid.Errors.ToList().ForEach(erro => listaErros.Add(new RetornoErrosDTO() { Key = erro.PropertyName.ToLower(), Value = erro.ErrorMessage.Replace("\"", "'") }));
                return BadRequest(JsonConvert.SerializeObject(listaErros));
            }

            _repo.Add(produto);
            await _repo.SaveAsync(produto);

            return CreatedAtAction("GetProduto", new { id = produto.Id }, produto);
        }

        // DELETE: api/Produtos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Produto>> DeleteProduto(int id)
        {
            var produto = await _context.Produto.FindAsync(id);
            if (produto == null)
                return NotFound();

            //_context.Produto.Remove(produto);
            _repo.Delete(produto);
            await _repo.SaveAsync(produto);

            return produto;
        }

        private bool ProdutoExists(int id) => _context.Produto.Any(e => e.Id == id);

        private Produto ParseToProduct(ProdutoDTO produtoDTO, ref Produto produto)
        {
            produto.Nome = produtoDTO.Nome;
            produto.Descricao = produtoDTO.Descricao;
            produto.Preco = decimal.Parse(produtoDTO.Preco);
            produto.Quantidade = produtoDTO.Quantidade;
            produto.Categoria = produtoDTO.Categoria;

            return produto;
        }
    }

}
