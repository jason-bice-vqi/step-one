import { Component, Inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA } from '@angular/material/snack-bar';

@Component({
  selector: 'app-html-snackbar',
  template: `<span [innerHTML]="data"></span>`,
})
export class HtmlSnackbarComponent {
  constructor(@Inject(MAT_SNACK_BAR_DATA) public data: string) {}
}
