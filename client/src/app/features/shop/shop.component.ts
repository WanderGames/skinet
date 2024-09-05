import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { Product } from '../../shared/models/products';
import { MatCard } from '@angular/material/card';
import { ProductItemComponent } from "./product-item/product-item.component";
import { MatDialog } from '@angular/material/dialog';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    MatCard,
    MatButton,
    MatIcon,
    ProductItemComponent,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit {
  //inject our shop service
  private shopService = inject(ShopService);
  //inject the MatDialog service from angular material
  private dialogService = inject(MatDialog);

  title = 'Skinet';
  //use our created product type
  products: Product[] = [];

  selectedBrands: string[] = [];
  selectedTypes: string[] = [];
  selectedSort: string = 'name';
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' }
  ]

  //this will run when the component is created
  ngOnInit(): void {
    this.initializeShop();
  }

  initializeShop() {
    //get the brands
    this.shopService.getBrands();

    //get the types
    this.shopService.getTypes();

    this.shopService.getProducts();
  }

  getProducts() {
    //get the products
    //this will send a get request to our api/products, subscribe to the observable response
    //using our service
    this.shopService.getProducts(this.selectedBrands, this.selectedTypes, this.selectedSort).subscribe({
      //tells us what to do with the response, int this case we want to log the data(products)
      next: response => this.products = response.data,
      //tells us what to do with the error
      error: error => console.log(error)
    });
  }

  onSortChange(event: MatSelectionListChange) {
    //get the selected option from the event
    const selectedOption = event.options[0];

    //set the selected sort to the selected option
    if (selectedOption) {
      this.selectedSort = selectedOption.value;
      this.getProducts();

    }
  }

  openFiltersDialog() {
    // set up a dialog with our dialog service and pass in our data
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: 500,
      data: {
        selectedBrands: this.selectedBrands,
        selectedTypes: this.selectedTypes
      }
    });

    //when the dialog closes subscribe to the result and apply the filters
    dialogRef.afterClosed().subscribe({
      next: result => {
        if (result) {
          this.selectedBrands = result.selectedBrands;
          this.selectedTypes = result.selectedTypes;
          //apply filters
          this.shopService.getProducts(this.selectedBrands, this.selectedTypes).subscribe({
            next: response => this.products = response.data,
            error: error => console.log(error)
          });
        }
      }

    });
  }

}
