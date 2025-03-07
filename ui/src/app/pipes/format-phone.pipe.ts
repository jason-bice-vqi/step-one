import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatPhone'
})
export class FormatPhonePipe implements PipeTransform {

  transform(value: string | number): string {
    if (!value) return '';

    // Convert input to string and remove all non-numeric characters
    const cleaned = value.toString().replace(/\D/g, '');

    // Ensure it's a valid 10-digit number
    if (cleaned.length !== 10) {
      return value.toString(); // Return as is if not a valid phone number
    }

    // Format as (xxx) xxx-xxxx
    return `(${cleaned.slice(0, 3)}) ${cleaned.slice(3, 6)}-${cleaned.slice(6, 10)}`;
  }

}
