import { Injectable } from '@angular/core';
import * as L from "leaflet";
import * as esri from 'esri-leaflet';

import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';



@Injectable({
  providedIn: 'root'
})
export class LeafletEsriService {
  getDynamicMapLayer(options: any): any {
    return esri.dynamicMapLayer(options);
  }

  constructor() 
  { 

    if (L){
      console.log('LeafletEsriService has L. Yay!')

    }else{
      console.error('LeafletEsriService does not have L. BOO :(')
    }

  }





  getBaseMapLayer(basemap:esri.Basemaps, options:any){

    
    return esri.basemapLayer(basemap,options);

    // L.esri.basemapLayer('ImageryLabels');
  }

  
 

}
