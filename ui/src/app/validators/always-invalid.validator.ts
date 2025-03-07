import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function alwaysInvalidValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    return { alwaysInvalid: true };
  };
}
