import { Injectable } from '@angular/core';

/**
 * Workaround to ActivedRoute issue in Angular 6 where the querystring params were empty during DI bootstrap.
 */
@Injectable({
  providedIn: 'root'
})
export class QueryStringService {

  _history: string[] = [];
  _maxHistorySize : number = 10;

  constructor() { 
    this.addToHistory(window.location.search);   
  }

  /**
   * Gets a query string parameter value
   * @param parameterName 
   */
  getQueryStringParameter (parameterName:string){    
    const urlParams = new URLSearchParams(window.location.search); // https://developer.mozilla.org/en-US/docs/Web/API/URLSearchParams
    this.addToHistory(window.location.search);
    return urlParams.get(parameterName); 
  }

  /**
   * Keep a history for diagnostic and troubleshooting purposes.
   * @param url 
   */
  private addToHistory(url : string){
    if (this._history.push(window.location.search) > this._maxHistorySize){
      this._history.pop(); 
    }

    console.log('QueryStringService.History:',this._history);
  }

  /**
   * Get a history for diagnostic and troubleshooting purposes.
   */
  getHistory () {
    return this._history;
  }
}
