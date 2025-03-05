import { Component } from '@angular/core';

@Component({
  selector: 'app-otp-input',
  templateUrl: './otp-input.component.html',
  styleUrls: ['./otp-input.component.scss']
})
export class OtpInputComponent {
  otpArray = new Array(6); // 6-digit OTP
  otp: string[] = new Array(6).fill('');

  moveToNext(index: number, event: any) {
    if (event.target.value.length === 1 && index < this.otp.length - 1) {
      (document.querySelectorAll('input')[index + 1] as HTMLInputElement).focus();
    }
  }

  moveToPrev(index: number, event: any) {
    if (!this.otp[index] && index > 0) {
      (document.querySelectorAll('input')[index - 1] as HTMLInputElement).focus();
    }
  }
}
