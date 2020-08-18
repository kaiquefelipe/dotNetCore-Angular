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
    public class CategoriasController : ControllerBase
    {
        private readonly ProjectContext _context;
        private readonly IDataRepository<Categoria> _repo;

        public CategoriasController(ProjectContext context, IDataRepository<Categoria> repo)
        {
            _context = context;
            _repo = repo;
        }

        // GET: api/Categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoria()
        {
            return await _context.Categoria.ToListAsync();
        }

        // GET: api/Categorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(short id)
        {
            var categoria = await _context.Categoria.FindAsync(id);

            if (categoria == null)
                return NotFound();

            return categoria;
        }

        // PUT: api/Categorias/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(short id, Categoria categoria)
        {
            if (id != categoria.Id)
                return BadRequest();

            var validator = new CategoriaValidator();
            var isValid = validator.Validate(categoria);

            if (!isValid.IsValid)
            {
                var listaErros = new List<RetornoErrosDTO>();
                isValid.Errors.ToList().ForEach(erro => listaErros.Add(new RetornoErrosDTO() { Key = erro.PropertyName.ToLower(), Value = erro.ErrorMessage.Replace("\"", "'") }));
                return BadRequest(JsonConvert.SerializeObject(listaErros));
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                _repo.Update(categoria);
                await _repo.SaveAsync(categoria);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Categorias
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
        {
            var validator = new CategoriaValidator();
            var isValid = validator.Validate(categoria);
            if (!isValid.IsValid)
            {
                var listaErros = new List<RetornoErrosDTO>();
                isValid.Errors.ToList().ForEach(erro => listaErros.Add(new RetornoErrosDTO() { Key = erro.PropertyName.ToLower(), Value = erro.ErrorMessage.Replace("\"", "'") }));
                return BadRequest(JsonConvert.SerializeObject(listaErros));//(erros, null, 412, "Erros " + isValid.Errors.Count);
            }

            _repo.Add(categoria);
            await _repo.SaveAsync(categoria);

            return CreatedAtAction("GetCategoria", new { id = categoria.Id }, categoria);
        }

        // DELETE: api/Categorias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Categoria>> DeleteCategoria(short id)
        {
            var categoria = await _context.Categoria.FindAsync(id);
            if (categoria == null)
                return NotFound();

            //_context.Categoria.Remove(categoria);
            _repo.Delete(categoria);
            await _repo.SaveAsync(categoria);

            return categoria;
        }

        private bool CategoriaExists(short id) => _context.Categoria.Any(e => e.Id == id);
    }
}
