using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Project.Models
{
    public class ProdutoValidator : AbstractValidator<Produto>
    {
        public ProdutoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome do produto deve ser preenchido.")
                .Length(5, 100).WithMessage("O nome do produto deve ter entre 5 e 100 caracteres.");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição do produto deve ser preenchida.")
                .Length(5, 100).WithMessage("A descrição do produto deve ter entre 5 e 100 caracteres.");

            RuleFor(x => x.Preco)
                .NotEmpty().WithMessage("O Preço do produto deve ser preenchida.")
                .ScalePrecision(2, 6).WithMessage("Preço deve ter duas casas decimais")
                .GreaterThan(0).WithMessage("O preço deve ser maior que '0'");

            RuleFor(x => x.Quantidade)
                .NotEmpty().WithMessage("A Quantidade do produto deve ser preenchida.");

        }
    }
}
