import { Component } from '@angular/core';
import api from '../../Services/api';
import * as models from '../../Models'
import { Router, ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-form-category',
  templateUrl: './form-category.component.html',
  styleUrls: ['./form-category.component.css']
})
export class FormCategoryComponent {
  public categorias: models.Categoria[];
  public submitted = false;
  public idCategoria = null;
  public nomeCategoria = null;
  public descricaoCategoria = null;
  private isNew: boolean;
  public onlyView = true;

  constructor(private actRoute: ActivatedRoute, private router: Router) {
    this.onlyView = Boolean(Number(this.actRoute.snapshot.params.onlyView)) ? true : undefined;

    if(this.actRoute.snapshot.params.id){
      this.isNew = false
      this.getCategorySpecific(Number(this.actRoute.snapshot.params.id))
    }
    else
      this.isNew = true;

    this.GetCategories();
  }


  GetCategories = () => {
    api.get<models.Categoria[]>("/categorias")
    .then(response => {
      this.categorias = response.data
    })
    .catch(error => console.log(error))
  }

  onSubmit = (dataCategoryForm: NgForm) => {
    this.submitted = true;
    
    // // Novo Cadastro
    if(this.isNew)
      this.SubmitNewData(dataCategoryForm);
    else
      this.SubmitUpdateData(dataCategoryForm, this.idCategoria);
  }

  onRedirect = () => this.router.navigate(['/categorias']);

  getCategorySpecific = (id: number) => {
    api.get<models.Categoria>(`/categorias/${id}`)
    .then(response => {
      let obj = {... response.data};

      this.idCategoria = obj.id;
      this.nomeCategoria = obj.nome;
      this.descricaoCategoria = obj.descricao;
      this.submitted = false;
    })
    .catch(error => console.log(error));
  };

  SubmitNewData = (dataCategoryForm: NgForm) => {
    const data = {
      Nome: this.nomeCategoria,
      Descricao: this.descricaoCategoria,
    }
    api.post<models.Categoria>("/categorias", data)
    .then(response => {
      let obj = {... response.data};

      this.idCategoria = obj.id;
      this.nomeCategoria = obj.nome;
      this.descricaoCategoria = obj.descricao
    })
    .catch(err => {
      if(err.response.status === 400){
        let res = err.response.data;
        const resJson = JSON.parse(res);
        if(resJson && resJson.length > 0){
          resJson.forEach((element: any) => {
            dataCategoryForm.form.controls[element.Key].setErrors({ invalid: element.Value });
          });
        }
      }
    })
    .finally(() => this.onRedirect());
  }

  SubmitUpdateData = (dataCategoryForm: NgForm, id:number) => {
    this.submitted = false;
    const data = {
      Id: this.idCategoria,
      Nome: this.nomeCategoria,
      Descricao: this.descricaoCategoria
    }
    api.put<models.Categoria>(`/Categorias/${id}`, data)
    .then(() => {
      alert(`Categoria [${id}] atualizado com sucesso!`);
    })
    .catch(err => {
      if(err.response.status === 400){
        let res = err.response.data;
        const resJson = JSON.parse(res);
        if(resJson && resJson.length > 0){
          resJson.forEach((element: any) => {
            dataCategoryForm.form.controls[element.Key].setErrors({ invalid: element.Value });
          });
        }
      }
      alert(`Não conseguimos atualizar a categoria [${id}]!`);
    })
    .finally(() => this.onRedirect());
  }
}