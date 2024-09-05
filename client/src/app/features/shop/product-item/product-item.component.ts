import { Component, Input, input } from '@angular/core';
import { Product } from '../../../shared/models/products';
import { MatCard, MatCardActions, MatCardContent } from '@angular/material/card';
import { CurrencyPipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-product-item',
  standalone: true,
  imports: [
    MatCard,
    MatCardContent,
    MatCardActions,
    MatButton,
    MatIcon,
    CurrencyPipe
  ],
  templateUrl: './product-item.component.html',
  styleUrl: './product-item.component.scss'
})
export class ProductItemComponent {
  //access the product passed in from the parent component(shop component)
  @Input() product?: Product;
}
