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
import { ShopParams } from '../../shared/models/shopParams';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../shared/models/pagination';
import { FormsModule } from '@angular/forms';

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
    MatMenuTrigger,
    MatPaginator,
    FormsModule
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
  products?: Pagination<Product>;

  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' }
  ]

  //create a new instance of our shop params class
  shopParams: ShopParams = new ShopParams();

  //create a array of our page size options
  pageSizeOptions = [5,10,15,20];

  //this will run when the component is created
  ngOnInit(): void {
    this.initializeShop();
  }

  initializeShop() {
    //get the brands
    this.shopService.getBrands();

    //get the types
    this.shopService.getTypes();

    //get the products
    this.getProducts();
  }

  getProducts() {
    //get the products
    //this will send a get request to our api/products, subscribe to the observable response
    //using our service
    this.shopService.getProducts(this.shopParams).subscribe({
      //tells us what to do with the response, int this case we want to log the data(products)
      next: response => this.products = response,
      //tells us what to do with the error
      error: error => console.log(error)
    });
  }

  onSearchChange() {
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  //event handler for when the user changes the page size
  handlePageEvent(event: PageEvent){
    //set the page number and page size
    this.shopParams.pageNumber = event.pageIndex + 1;
    this.shopParams.pageSize = event.pageSize;
    this.getProducts();
  }

  //event handler for when the user changes the sort
  onSortChange(event: MatSelectionListChange) {
    //get the selected option from the event
    const selectedOption = event.options[0];

    //set the selected sort to the selected option
    if (selectedOption) {
      this.shopParams.sort = selectedOption.value;
      //reset the page number when the sort is changed
      this.shopParams.pageNumber = 1;
      this.getProducts();

    }
  }

  openFiltersDialog() {
    // set up a dialog with our dialog service and pass in our data
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: 500,
      data: {
        selectedBrands: this.shopParams.brands,
        selectedTypes: this.shopParams.types
      }
    });

    //when the dialog closes subscribe to the result and apply the filters
    dialogRef.afterClosed().subscribe({
      next: result => {
        if (result) {
          this.shopParams.brands = result.selectedBrands;
          this.shopParams.types = result.selectedTypes;
          //reset the page number when filters are applied
          this.shopParams.pageNumber = 1;
          //apply filters
          this.getProducts();
        }
      }

    });
  }

}
