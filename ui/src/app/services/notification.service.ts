import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
import {HtmlSnackbarComponent} from "../shared/html-snackbar/html-snackbar.component";

@Injectable({ providedIn: 'root' })
export class NotificationService {
  defaultDuration = 5000;
  introDuration = 15000;

  private defaultConfig: MatSnackBarConfig = {
    duration: this.defaultDuration,
    horizontalPosition: 'center',
    verticalPosition: 'top'
  };

  constructor(private snackBar: MatSnackBar) {}

  success(message: string, duration = this.defaultDuration): void {
    this.show(message, 'snack-success', duration);
  }

  error(message: string, duration = this.defaultDuration): void {
    this.show(message, 'snack-error', duration);
  }

  info(message: string, duration = this.defaultDuration): void {
    this.show(message, 'snack-info', duration);
  }

  intro(message: string, duration = this.introDuration): void {
    this.show(message, 'snack-intro', duration);
  }

  warn(message: string, duration = this.defaultDuration): void {
    this.show(message, 'snack-warn', duration);
  }

  custom(message: string, config: MatSnackBarConfig = {}): void {
    this.snackBar.open(message, 'Dismiss', {
      ...this.defaultConfig,
      ...config
    });
  }

  private show(message: string, panelClass: string, duration: number) {
    this.snackBar.openFromComponent(HtmlSnackbarComponent, {
      data: message,
      ...this.defaultConfig,
      duration,
      panelClass: [panelClass]
    });
  }
}
