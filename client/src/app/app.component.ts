import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./layout/header/header.component";
import { HttpClient } from '@angular/common/http';
import { Product } from './shared/models/products';
import { Pagination } from './shared/models/pagination';

//@component decorator that specifies the metadata for the component
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  //tell angular where our api is
  baseUrl = 'https://localhost:5001/api/';
  //inject the http client
  private http = inject(HttpClient);
  title = 'Skinet';
  //use our created product type
  products: Product[] = [];

  //this will run when the component is created
  ngOnInit(): void {
    //this will send a get request to our api/products
    this.http.get<Pagination<Product>>(this.baseUrl + 'products').subscribe({
      //tells us what to do with the response, int this case we want to log the data(products)
      next: response => this.products = response.data,
      //tells us what to do with the error
      error: error => console.log(error),
      //tells us what to do when the request is done
      complete: () => console.log('Request has completed')
    });
  }

}
