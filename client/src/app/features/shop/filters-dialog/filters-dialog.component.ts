import { Component, inject } from '@angular/core';
import { ShopService } from '../../../core/services/shop.service';
import { MatDivider } from '@angular/material/divider';
import { MatListOption, MatSelectionList } from '@angular/material/list';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-filters-dialog',
  standalone: true,
  imports: [
    MatDivider,
    MatSelectionList,
    MatListOption,
    MatButton,
    FormsModule
  ],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss'
})
export class FiltersDialogComponent {
  //declare a public property of type ShopService and inject it
  shopService = inject(ShopService);
  //get access to the dialog ref
  private dialogRef = inject(MatDialogRef<FiltersDialogComponent>);
  //get access to the dialog data
  data = inject(MAT_DIALOG_DATA);
  //set our selected brands and types to the data passed in the dialog
  selectedBrands: string[] = this.data.selectedBrands;
  selectedTypes: string[] = this.data.selectedTypes;

  applyFilters(){
    //close the dialog and return the selected brands and types
    this.dialogRef.close({
      selectedBrands: this.selectedBrands,
      selectedTypes: this.selectedTypes
    });
  }
}
