import {Component} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {OtpService} from "../services/otp.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  otpRequestForm: FormGroup;
  otpLoginForm: FormGroup;
  adLoginForm: FormGroup;

  phoneNumberRegex = /^\(\d{3}\) \d{3}-\d{4}$|^\d{10}$|^\d{3}-\d{3}-\d{4}$/;
  oneTimePasswordRequested = false;

  constructor(private fb: FormBuilder, private otpService: OtpService) {
    this.otpRequestForm = this.fb.group({
      phone: ['', [Validators.required, Validators.pattern(this.phoneNumberRegex)]]
    });

    this.otpLoginForm = this.fb.group({
      otp: ['', [Validators.required]]
    });

    this.adLoginForm = this.fb.group({
      email: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }

  get phoneControl() {
    return this.otpRequestForm.get('phone');
  }

  get otpControl() {
    return this.otpLoginForm.get('otp');
  }

  onOtpRequest() {
    const phoneNumber = this.phoneControl?.value;

    this.otpService.requestOtp(phoneNumber).subscribe(() => {
      this.oneTimePasswordRequested = true;
    });
  }

  onOtpSubmit() {
    const phoneNumber = this.phoneControl!.value;
    const otp = this.otpControl!.value;

    this.otpService.verifyOtp(phoneNumber, otp).subscribe((result) => {
      if (result) {
        this.oneTimePasswordRequested = true;

        console.log('OTP successfully validated.');
      } else {
        console.log('OTP validation failed.');
      }
    });
  }

  onAdSubmit() {

  }
}
