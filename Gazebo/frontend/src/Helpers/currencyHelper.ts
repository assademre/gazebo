export {};

export class CurrencyConverter {
    private static currencySymbols: Record<string, string> = {
      USD: '$',
      EUR: '€',
      GBP: '£',
    };
  
    static getCurrencySymbol(currencyCode: string): string {
      const code = currencyCode.toUpperCase();
      
      if (this.currencySymbols.hasOwnProperty(code)) {
        return this.currencySymbols[code];
      } else {
        return code;
      }
    }
  }
  