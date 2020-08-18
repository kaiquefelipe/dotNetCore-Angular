import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ProdutosComponent } from './produtos/produtos.component';
import { FormComponent } from './form/form.component';
import { FormCategoryComponent } from "./form-category/form-category.component";
import { CategoriasComponent } from './categorias/categorias.component';

import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { CurrencyMaskModule } from "ng2-currency-mask";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ProdutosComponent,
    CategoriasComponent,
    FormComponent,
    FormCategoryComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AngularFontAwesomeModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'produtos', component: ProdutosComponent },
      { path: 'categorias', component: CategoriasComponent },
      { path: 'formulario', component: FormComponent },
      { path: 'formulariocategoria', component: FormCategoryComponent },
      { path: 'formulario/:id/:onlyView', component: FormComponent },
      { path: 'formulariocategoria/:id/:onlyView', component: FormCategoryComponent },
    ]),
    CurrencyMaskModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
