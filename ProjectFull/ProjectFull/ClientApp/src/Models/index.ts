export interface Categoria {
    id: number;
    nome: string;
    descricao: string;
}

export class ProdutoDTO {
    id: number;
    nome: string;
    descricao: string;
    preco: number;
    quantidade: number;
    idCategoria: number;
    constructor(
        id: number,
        nome: string,
        descricao: string,
        preco: number,
        quantidade: number,
        idCategoria: number
    ){ }
}

export class Produto {
    id: number;
    nome: string;
    descricao: string;
    preco: number;
    quantidade: number;
    categoria: Categoria;
}