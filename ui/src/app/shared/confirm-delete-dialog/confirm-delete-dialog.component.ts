import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {DomSanitizer, SafeHtml} from "@angular/platform-browser";

@Component({
  selector: 'app-confirmation-modal',
  templateUrl: './confirm-delete-dialog.component.html',
  styleUrls: ['./confirm-delete-dialog.component.scss']
})
export class ConfirmDeleteDialogComponent {
  message: SafeHtml;
  yesText: string = 'Delete';
  noText: string = 'Cancel';
  title: string = 'Confirm Delete'

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { message: string, title?: string, yesText?: string, noText?: string },
    public dialogRef: MatDialogRef<ConfirmDeleteDialogComponent>,
    private sanitizer: DomSanitizer
  ) {
    this.message = this.sanitizer.bypassSecurityTrustHtml(data.message);
    this.title = data.title ?? this.title;
    this.yesText = data.yesText ?? this.yesText;
    this.noText = data.noText ?? this.noText;
  }
}
