import {Component} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";

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

  constructor(private fb: FormBuilder) {
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

  onOtpRequest() {
    this.oneTimePasswordRequested = true;
  }

  onOtpSubmit() {

  }

  onAdSubmit() {

  }
}
