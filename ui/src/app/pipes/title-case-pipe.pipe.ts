import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'titleCasePipe'
})
export class TitleCasePipePipe implements PipeTransform {

  transform(value: string): string {
    if (!value) return '';

    const spaced = value.replace(/([a-z])([A-Z])/g, '$1 $2');

    return spaced.charAt(0).toUpperCase() + spaced.slice(1);
  }
}
