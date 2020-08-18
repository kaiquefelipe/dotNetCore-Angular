using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Project.Models
{
    public class CategoriaValidator : AbstractValidator<Categoria>
    {
        public CategoriaValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome da categoria deve ser preenchido.")
                .Length(3, 100).WithMessage("O nome da categoria deve ter entre 3 e 100 caracteres.");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição da categoria deve ser preenchida.")
                .Length(3, 100).WithMessage("A descrição da categoria deve ter entre 3 e 100 caracteres.");

        }
    }
}
