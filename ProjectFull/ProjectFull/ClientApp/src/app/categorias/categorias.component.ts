import { Component } from '@angular/core';
import api from '../../Services/api';
import * as models from '../../Models'
import { Router } from '@angular/router';

@Component({
  selector: 'app-categorias',
  templateUrl: './categorias.component.html',
  styleUrls: ['./categorias.component.css']
})
export class CategoriasComponent {
  public categorias: models.Categoria[];

  constructor(private router: Router) {
    this.getCategories();
  }

  getCategories = () => {
    api.get<models.Categoria[]>("/categorias")
    .then(response => {
      this.categorias = response.data
    })
    .catch(error => console.error(error))
  }

  NewCategory = () => this.router.navigate(['/formulariocategoria']);

  DeleteCategory = (id: number, nome: string) => {
    if(confirm(`Deseja realmente deletar a categoria [${id} - ${nome}]?`)) {
      this.ConfirmDeleteCategory(id);
    }
  }

  ConfirmDeleteCategory = (id: number) => {
    api.delete(`/categorias/${id}`).then(() => {
      alert(`Categoria [${id}] deletada com sucesso.`)
      this.categorias = this.categorias.filter(item => item.id != id );
    })
    .catch(error => {
      console.log(error);
      alert(`Não conseguimos deletar a categoria [${id}].`);
    })
  }
}
