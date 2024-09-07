import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/products';
import { ShopParams } from '../../shared/models/shopParams';

//can inject this service whereever we need it in our code
@Injectable({
  //provided on start up for entire application
  providedIn: 'root'
})

//this is our service
//a service is initilized when the app starts and is a singleton
export class ShopService {
  //tell angular where our api is
  baseUrl = 'https://localhost:5001/api/';
  //inject the http client
  private http = inject(HttpClient);
  
  types: string[] = [];
  brands: string[] = [];

  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();

    //add the brands to the params query string using a comma seperated string
    if(shopParams.brands.length > 0) {
      params = params.append('brands', shopParams.brands.join(','));
    }

    //add the types to the params query string using a comma seperated string
    if(shopParams.types.length > 0) {
      params = params.append('types', shopParams.types.join(','));
    }

    //add the sort to the params query string
    if(shopParams.sort) {
      params = params.append('sort', shopParams.sort);
    }

    //add the search to the params query string
    if(shopParams.search){
      params = params.append('search', shopParams.search);
    }

    //add the pageSize to the params query string
    params = params.append('pageSize', shopParams.pageSize);
    //add the pageIndex to the params query string
    params = params.append('pageIndex', shopParams.pageNumber);

    //use our params as a query string i.e. ?brands=react&types=boards
    return this.http.get<Pagination<Product>>(this.baseUrl + 'products', {params: params});
  }

  /**
   * Get a single product by id
   * @param id the id of the product
   * @returns an observable that emits a single product
   */
  getProduct(id: number) {
    return this.http.get<Product>(this.baseUrl + 'products/' + id);
  }

  /**
   * Get all the brands
   * If the list of brands is already populated, return
   * Otherwise, make an API call to get the list of brands and populate the list
   * @returns an observable that emits a list of strings
   */
  getBrands() {
    //if the list of brands is already populated, return
    if(this.brands.length > 0) return;
    
    return this.http.get<string[]>(this.baseUrl + 'products/brands').subscribe({
      next: response => this.brands = response
    });
  }

  /**
   * Get all the types
   * If the list of types is already populated, return
   * Otherwise, make an API call to get the list of types and populate the list
   * @returns an observable that emits a list of strings
   */
  getTypes() {
    //if the list of types is already populated, return
    if(this.types.length > 0) return;

    return this.http.get<string[]>(this.baseUrl + 'products/types').subscribe({
      next: response => this.types = response
    });
  }
}
