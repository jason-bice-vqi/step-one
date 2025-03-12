import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {OtpAuthService} from "../services/auth/otp-auth.service";
import {Router} from "@angular/router";
import {alwaysInvalidValidator} from "../validators/always-invalid.validator";
import {JwtService} from "../services/auth/jwt.service";
import {AdAuthService} from "../services/auth/ad-auth.service";
import {take} from "rxjs";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  @ViewChild('otpInput') otpInput!: ElementRef;
  @ViewChild('phoneNumberInput') phoneNumberInput!: ElementRef;

  otpRequestForm: FormGroup;
  otpLoginForm: FormGroup;

  phoneNumberRegex = /^\(\d{3}\) \d{3}-\d{4}$|^\d{10}$|^\d{3}-\d{3}-\d{4}$/;
  oneTimePasswordRequested = false;

  constructor(private adAuthService: AdAuthService,
              private fb: FormBuilder,
              private jwtService: JwtService,
              private otpService: OtpAuthService,
              private router: Router) {

    this.otpRequestForm = this.fb.group({
      phone: ['', [Validators.required, Validators.pattern(this.phoneNumberRegex), alwaysInvalidValidator]]
    });

    this.otpLoginForm = this.fb.group({
      otp: ['', [Validators.required, alwaysInvalidValidator]]
    });
  }

  get phoneControl() {
    return this.otpRequestForm.get('phone')!;
  }

  get phoneNumber() {
    return this.phoneControl!.value;
  }

  get otpControl() {
    return this.otpLoginForm.get('otp')!;
  }

  onOtpRequest(isCodeSent: boolean = false) {
    const phoneNumber = this.phoneControl.value;

    if (isCodeSent) {
      this.oneTimePasswordRequested = true;
      this.focusOtp();
      return;
    }

    this.otpService.requestOtp(phoneNumber).subscribe({
      next: () => {
        console.info('OTP request succeeded.');

        this.oneTimePasswordRequested = true;
        this.focusOtp();
      },
      error: (err) => {
        console.error('Invalid OTP request (phone number).', err);

        this.phoneControl.setErrors({alwaysInvalid: true});
        this.focusPhone();
      }
    });
  }

  onOtpSubmit() {
    const phoneNumber = this.phoneControl.value;
    const otp = this.otpControl.value;

    this.otpService.authenticate(phoneNumber, otp).subscribe({
      next: (token) => {
        console.info('OTP authentication succeeded.');

        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        console.error('Invalid OTP.', err);

        this.otpControl.setErrors({alwaysInvalid: true});
        this.focusOtp();
      }
    });
  }

  onAdLogin() {
    this.adAuthService.authenticate().pipe(take(1)).subscribe({
      next: (token) => {
        console.info('AD authentication succeeded.');

        this.router.navigate(['/admin/dashboard']);
      },
      error: (err) => {
        console.error('AD authentication failed.', err);
      }
    });
  }

  onResendOtp() {
    this.phoneControl.patchValue(null);
    this.otpControl.patchValue(null);

    this.oneTimePasswordRequested = false;
    this.focusPhone();
  }

  focusOtp() {
    setTimeout(() => {
      this.otpInput.nativeElement.focus();
    }, 100);
  }

  focusPhone() {
    setTimeout(() => {
      this.phoneNumberInput.nativeElement.focus();
    }, 100);
  }

  ngOnInit(): void {
    if (this.jwtService.isAuthenticated()) {
      console.log('User is already authenticated. Redirecting to admin-user-dashboard.', this.jwtService.decodeToken());

      this.router.navigate(['/admin-user-dashboard']);
    }
  }
}
