import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'phone'
})
export class PhonePipe implements PipeTransform {

  transform(value: string | number): string {
    if (!value) return '';
    let valStr = value.toString().replace(/\D/g, '');

    if (valStr.length === 11) {
      return `(${valStr.substring(0, 2)}) ${valStr.substring(2, 7)}-${valStr.substring(7)}`;
    } else if (valStr.length === 10) {
      return `(${valStr.substring(0, 2)}) ${valStr.substring(2, 6)}-${valStr.substring(6)}`;
    } else {
      return value.toString();
    }
  }

}
