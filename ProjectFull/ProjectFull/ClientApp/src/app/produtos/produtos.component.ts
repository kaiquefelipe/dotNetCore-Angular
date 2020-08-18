import { Component } from '@angular/core';
import api from '../../Services/api';
import * as models from '../../Models'
import { Router } from '@angular/router';

@Component({
  selector: 'app-produtos',
  templateUrl: './produtos.component.html',
  styleUrls: ['./produtos.component.css']
})
export class ProdutosComponent {
  public produtos: models.Produto[];

  constructor(private router: Router) {
    this.GetCategories();
  }

  GetCategories = () => {
    api.get<models.Produto[]>("/produtos").then(response => {
      this.produtos = response.data
    })
    .catch(error => console.error(error))
  }

  NewProduct = () => this.router.navigate(['/formulario']);

  DeleteProduct = (id: number, nome: string) => {
    if(confirm(`Deseja realmente deletar o produto [${id} - ${nome}]?`)) {
      this.ConfirmDeleteProduct(id);
    }
  }

  ConfirmDeleteProduct = (id: number) => {
    api.delete(`/produtos/${id}`).then(() => {
      alert(`Produto [${id}] deletado com sucesso.`)
      this.produtos = this.produtos.filter(item => item.id != id );
    })
    .catch(error => {
      console.log(error);
      alert(`Não conseguimos deletar o produto [${id}].`);
    })
  }

  FormatPrice = (price: number) => new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(price);
}