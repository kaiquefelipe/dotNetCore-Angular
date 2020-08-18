import { Component } from '@angular/core';
import api from '../../Services/api';
import * as models from '../../Models'
import { Router, ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.css']
})
export class FormComponent {
  public categorias: models.Categoria[];
  public submitted = false;
  public idProduto = null;
  public nomeProduto = null;
  public descricaoProduto = null;
  public precoProduto = null;
  public quantidadeProduto = null;
  public idCategoriaProduto = null;
  public DescricaoCategoriaProduto = null;
  private isNew: boolean;
  public onlyView = true;

  constructor(private actRoute: ActivatedRoute, private router: Router) {
    console.log(typeof this.actRoute.snapshot.params.onlyView)
    this.onlyView = Boolean(Number(this.actRoute.snapshot.params.onlyView)) ? true : undefined;

    if(this.actRoute.snapshot.params.id){
      this.isNew = false
      this.getProductSpecific(Number(this.actRoute.snapshot.params.id))
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
    .catch(error => console.error(error))
  }

  onSubmit = (formData: NgForm) => {
    //e.preventDefault();
    this.submitted = true;
    
    // // Novo Cadastro
    if(this.isNew)
      this.SubmitNewData(formData);
    else
      this.SubmitUpdateData(formData, this.idProduto);
  }

  onRedirect = () => this.router.navigate(['/produtos'])

  getProductSpecific = (id: number) => {
    api.get<models.Produto>(`/produtos/${id}`)
    .then(response => {
      let obj = {... response.data};

      this.idProduto = obj.id;
      this.nomeProduto = obj.nome;
      this.descricaoProduto = obj.descricao;
      this.precoProduto = obj.preco;
      this.quantidadeProduto = obj.quantidade;
      this.idCategoriaProduto = obj.categoria.id;
      this.submitted = false;
    })
    .catch(error => console.log(error));
  };

  SubmitNewData = (formData: NgForm) => {
    const data = {
      Nome: this.nomeProduto,
      Descricao: this.descricaoProduto,
      Preco: this.precoProduto.toString(),
      Quantidade : this.quantidadeProduto,
      IdCategoria: this.idCategoriaProduto
    }
    console.log(data);
    api.post<models.Produto>("/produtos", data)
    .then(response => {
      let obj = {... response.data};

      this.idProduto = obj.id;
      this.nomeProduto = obj.nome;
      this.descricaoProduto = obj.descricao;
      this.precoProduto = obj.preco;
      this.quantidadeProduto = obj.quantidade;
      this.idCategoriaProduto = obj.categoria.id;
    })
    .catch(err => {
      if(err.response.status === 400){
        let res = err.response.data;
        console.log(res);
        const resJson = JSON.parse(res);
        console.log(resJson)
        console.log(resJson.length)
        if(resJson && resJson.length > 0){
          //let keyTemp: string;
          resJson.forEach((element: any) => {
            //if(keyTemp )
            console.log(formData.form.controls['nome']);
            formData.form.controls[element.Key].setErrors({ invalid: element.Value });
          });
        }
      }
    })
    .finally(() => this.onRedirect());
  }

  SubmitUpdateData = (formData: NgForm, id:number) => {
    this.submitted = false;
    const data = {
      Id: this.idProduto,
      Nome: this.nomeProduto,
      Descricao: this.descricaoProduto,
      Preco: this.precoProduto.toString(),
      Quantidade : this.quantidadeProduto,
      IdCategoria: this.idCategoriaProduto
    }
    console.log(data);
    api.put<models.Produto>(`/produtos/${id}`, data)
    .then(() => {
      alert(`Produto [${id}] atualizado com sucesso!`);
    })
    .catch(err => {
      if(err.response.status === 400){
        let res = err.response.data;
        console.log(res);
        const resJson = JSON.parse(res);
        console.log(resJson)
        console.log(resJson.length)
        if(resJson && resJson.length > 0){
          //let keyTemp: string;
          resJson.forEach((element: any) => {
            //if(keyTemp )
            console.log(formData.form.controls['nome']);
            formData.form.controls[element.Key].setErrors({ invalid: element.Value });
          });
        }
      }
      alert(`Não conseguimos atualizar o produto [${id}]!`);
    })
    .finally(() => this.onRedirect());
  }

  onOptionsSelected = (event: any) => this.DescricaoCategoriaProduto = event.target.options[event.target.options.selectedIndex].text;
}