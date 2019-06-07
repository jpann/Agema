import { Injectable } from '@angular/core';
import * as L from "leaflet";
import { HttpClient, HttpHeaders, HttpErrorResponse, HttpClientJsonpModule } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class LeafletGeoJsonService {

  private GetGeAPI_URL = 'https://5upozs6ae5.execute-api.us-east-1.amazonaws.com/default/getGeoJsonUrl';

  // arn:aws:lambda:us-east-1:080939433152:function:getGeoJsonUrl


  constructor(private http: HttpClient) {
    // this.getGeoJsonUrl('http://data-phl.opendata.arcgis.com/datasets/1839b35258604422b0b520cbb668df0d_0.geojson');
   }

  /**
   * 
   * @param url (e.g. http://data.phl.opendata.arcgis.com/datasets/1839b35258604422b0b520cbb668df0d_0.geojson)
   */
  getGeoJsonUrl(url:string) : Observable<any>{
    
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json'
      })
    };
    
    var lambdaUrl = `${this.GetGeAPI_URL}/?url=${url}`;
    console.log('lambdaUrl=',lambdaUrl);

    this.http.get(lambdaUrl).subscribe (x=> {
        console.log('lambdaUrl',x);
    });

    
    //  var xx = this.http.jsonp(url,'df2e890b0d5a4678b9b2a88d63e215549449ccfef0c8441aa79bd81538ece8af').pipe(map(this.extractData));

    //  xx.subscribe(x=>{
    //    debugger;
    //    console.log('x',x);
    //  })


      return;

  }
 
  private extractData(res: Response) {
    let body = res;
    return body || { };
  }

  /**
   * Gets the geojson from the assets directory
   * @param name The file name, including extension (.json or .geojson)
   * @param path The path from inside the assets folder.
   */
  getGeoJsonAsset(name:string,path?:string){

    if (!path){
      path = 'json'
    }

    return this.http.get(`./assets/${path}/${name}`).toPromise();
  }
  
}
