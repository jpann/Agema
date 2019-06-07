import { Injectable } from '@angular/core';
import * as L from "leaflet";
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LeafletHEREService {

  private _POITypes;

  constructor(private http: HttpClient) { 
    if (L){
      console.log('LeafletHEREService has L. Yay!')

      this.initializeHERE(L);

      if (!(L.TileLayer as any).HERE){
        console.error('HERE not configured for L global object.');
      }
      
      this.getPOITypes().subscribe((data: {}) => {
        this._POITypes = data;
        console.log('_POITypes=',this._POITypes);
      });

    }else{
      console.error('LeafletHEREService does not have L. BOO :(')
    }
  }

  createTileLayer(opts:any){
    return new (L.TileLayer as any).HERE(opts);
  }

  getPOITypes(): Observable<any> {
    const endpoint = 'https://1.base.maps.api.here.com/maptile/2.1/meta/pois?output=json&app_id=eo2x8382v6oI5RWUQkWD&app_code=aS_JaLHkcC-YnvgYVCUOJA';
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json'
      })
    };

    return this.http.get(endpoint).pipe(
      map(this.extractData))
  }

  private extractData(res: Response) {
    let body = res;
    return body || { };
  }

  //hereAppId:string,hereAppCode:string

  /**
   * Initializes Leaflet with HERE.  Pass in the global L.
   * @param leafletRef 
   */
  initializeHERE( leafletRef:any ){

    leafletRef.TileLayer.HERE =  leafletRef.TileLayer.extend({	
      options: {
        subdomains: '1234',
        minZoom: 2,
        maxZoom: 20,
    
        // ðŸ‚option scheme: String = 'normal.day'
        // The "map scheme", as documented in the HERE API.
        scheme: 'normal.day',
    
        // ðŸ‚option resource: String = 'maptile'
        // The "map resource", as documented in the HERE API.
        resource: 'maptile',
    
        // ðŸ‚option mapId: String = 'newest'
        // Version of the map tiles to be used, or a hash of an unique map
        mapId: 'newest',
    
        // ðŸ‚option format: String = 'png8'
        // Image format to be used (`png8`, `png`, or `jpg`)
        format: 'png8',
    
        // ðŸ‚option appId: String = ''
        // Required option. The `app_id` provided as part of the HERE credentials
        appId: '',
    
        // ðŸ‚option appCode: String = ''
        // Required option. The `app_code` provided as part of the HERE credentials
        appCode: '',
      },
    
    
      initialize: function initialize(options) {
        options = leafletRef.setOptions(this, options);
    
        // Decide if this scheme uses the aerial servers or the basemap servers
        var schemeStart = options.scheme.split('.')[0];
        options.tileResolution = 256;
    
        if (L.Browser.retina) {
          options.tileResolution = 512;
        }
    
    // 		{Base URL}{Path}/{resource (tile type)}/{map id}/{scheme}/{zoom}/{column}/{row}/{size}/{format}
    // 		?app_id={YOUR_APP_ID}
    // 		&app_code={YOUR_APP_CODE}
    // 		&{param}={value}
    
        var path = '/{resource}/2.1/{resource}/{mapId}/{scheme}/{z}/{x}/{y}/{tileResolution}/{format}?app_id={appId}&app_code={appCode}&pois';
        var attributionPath = '/maptile/2.1/copyright/{mapId}?app_id={appId}&app_code={appCode}';
    
        var tileServer = 'base.maps.api.here.com';
        if (schemeStart == 'satellite' ||
            schemeStart == 'terrain' ||
            schemeStart == 'hybrid') {
          tileServer = 'aerial.maps.api.here.com';
        }
        if (options.scheme.indexOf('.traffic.') !== -1) {
          tileServer = 'traffic.maps.api.here.com';
          path = '/{resource}/2.1/flowtile/{mapId}/{scheme}/{z}/{x}/{y}/{tileResolution}/{format}?app_id={appId}&app_code={appCode}&pois';
        }
    
        var tileUrl = 'https://{s}.' + tileServer + path;
    
        this._attributionUrl = L.Util.template('https://1.' + tileServer + attributionPath, this.options);
    
        leafletRef.TileLayer.prototype.initialize.call(this, tileUrl, options);
    
        this._attributionText = '';
    
      },
    
      onAdd: function onAdd(map) {
        L.TileLayer.prototype.onAdd.call(this, map);
    
        if (!this._attributionBBoxes) {
          this._fetchAttributionBBoxes();
        }
      },
    
      onRemove: function onRemove(map) {
        L.TileLayer.prototype.onRemove.call(this, map);
    
        this._map.attributionControl.removeAttribution(this._attributionText);
    
        this._map.off('moveend zoomend resetview', this._findCopyrightBBox, this);
      },
    
      _fetchAttributionBBoxes: function _onMapMove() {
        var xmlhttp = new XMLHttpRequest();
        xmlhttp.onreadystatechange = leafletRef.bind(function(){
          if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            this._parseAttributionBBoxes(JSON.parse(xmlhttp.responseText));
          }
        }, this);
        xmlhttp.open("GET", this._attributionUrl, true);
        xmlhttp.send();
      },
    
      _parseAttributionBBoxes: function _parseAttributionBBoxes(json) {
        if (!this._map) { return; }
        var providers = json[this.options.scheme.split('.')[0]] || json.normal;
        for (var i=0; i<providers.length; i++) {
          if (providers[i].boxes) {
            for (var j=0; j<providers[i].boxes.length; j++) {
              var box = providers[i].boxes[j];
              providers[i].boxes[j] = L.latLngBounds( [ [box[0], box[1]], [box[2], box[3]] ]);
            }
          }
        }
    
        this._map.on('moveend zoomend resetview', this._findCopyrightBBox, this);
    
        this._attributionProviders = providers;
    
        this._findCopyrightBBox();
      },
    
      _findCopyrightBBox: function _findCopyrightBBox() {
        if (!this._map) { return; }
        var providers = this._attributionProviders;
        var visibleProviders = [];
        var zoom = this._map.getZoom();
        var visibleBounds = this._map.getBounds();
    
        for (var i=0; i<providers.length; i++) {
          if (providers[i].minLevel < zoom && providers[i].maxLevel > zoom)
    
          if (!providers[i].boxes) {
            // No boxes = attribution always visible
            visibleProviders.push(providers[i]);
            break;
          }
    
          for (var j=0; j<providers[i].boxes.length; j++) {
            var box = providers[i].boxes[j];
            if (visibleBounds.overlaps(box)) {
              visibleProviders.push(providers[i]);
              break;
            }
          }
        }
    
        var attributions = ['<a href="https://legal.here.com/terms/serviceterms/gb/">HERE maps</a>'];
        for (var i=0; i<visibleProviders.length; i++) {
          var provider = visibleProviders[i];
          attributions.push('<abbr title="' + provider.alt + '">' + provider.label + '</abbr>');
        }
    
        var attributionText = 'Â© ' + attributions.join(', ') + '. ';
    
        if (attributionText !== this._attributionText) {
          this._map.attributionControl.removeAttribution(this._attributionText);
          this._map.attributionControl.addAttribution(this._attributionText = attributionText);
        }
      },    
    });



    leafletRef.TileLayer.FlowTiles =  leafletRef.TileLayer.extend({	
      options: {
        subdomains: '1234',
        minZoom: 2,
        maxZoom: 20,
    
        // ðŸ‚option scheme: String = 'normal.day'
        // The "map scheme", as documented in the HERE API.
        scheme: 'normal.day',
    
        // ðŸ‚option resource: String = 'maptile'
        // The "map resource", as documented in the HERE API.
        resource: 'maptile',
    
        // ðŸ‚option mapId: String = 'newest'
        // Version of the map tiles to be used, or a hash of an unique map
        mapId: 'newest',
    
        // ðŸ‚option format: String = 'png8'
        // Image format to be used (`png8`, `png`, or `jpg`)
        format: 'png8',
    
        // ðŸ‚option appId: String = ''
        // Required option. The `app_id` provided as part of the HERE credentials
        appId: '',
    
        // ðŸ‚option appCode: String = ''
        // Required option. The `app_code` provided as part of the HERE credentials
        appCode: '',
      },
    
    
      initialize: function initialize(options) {
        console.log('Initialize');

        options = leafletRef.setOptions(this, options);
    
        // Decide if this scheme uses the aerial servers or the basemap servers
        var schemeStart = options.scheme.split('.')[0];
        options.tileResolution = 256;
    
        if (L.Browser.retina) {
          options.tileResolution = 512;
        }
    
    // 		{Base URL}{Path}/{resource (tile type)}/{map id}/{scheme}/{zoom}/{column}/{row}/{size}/{format}
    // 		?app_id={YOUR_APP_ID}
    // 		&app_code={YOUR_APP_CODE}
    // 		&{param}={value}
    
        var path = '/{resource}/2.1/{resource}/{mapId}/{scheme}/{z}/{x}/{y}/{tileResolution}/{format}?app_id={appId}&app_code={appCode}&pois';
        var attributionPath = '/maptile/2.1/copyright/{mapId}?app_id={appId}&app_code={appCode}';
    
        var tileServer = 'base.maps.api.here.com';
        if (schemeStart == 'satellite' ||
            schemeStart == 'terrain' ||
            schemeStart == 'hybrid') {
          tileServer = 'aerial.maps.api.here.com';
        }

        debugger;
        console.log('options.scheme.indexOf(.traffic.)' , options.scheme.indexOf('.traffic.') )

        if (options.scheme.indexOf('.traffic.') !== -1) {
          tileServer = 'traffic.maps.api.here.com';
          path='/maptile/2.1/traffictile/{mapId}/{scheme}/{z}/{x}/{y}/{tileResolution}/{format}?app_id={appId}&app_code={appCode}&pois';
        }
    
        var tileUrl = 'https://{s}.' + tileServer + path;
    
        this._attributionUrl = L.Util.template('https://1.' + tileServer + attributionPath, this.options);
    
        leafletRef.TileLayer.prototype.initialize.call(this, tileUrl, options);
    
        this._attributionText = '';
    
      },
    
      onAdd: function onAdd(map) {
        L.TileLayer.prototype.onAdd.call(this, map);
    
        if (!this._attributionBBoxes) {
          this._fetchAttributionBBoxes();
        }
      },
    
      onRemove: function onRemove(map) {
        L.TileLayer.prototype.onRemove.call(this, map);
    
        this._map.attributionControl.removeAttribution(this._attributionText);
    
        this._map.off('moveend zoomend resetview', this._findCopyrightBBox, this);
      },
    
      _fetchAttributionBBoxes: function _onMapMove() {
        var xmlhttp = new XMLHttpRequest();
        xmlhttp.onreadystatechange = leafletRef.bind(function(){
          if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            this._parseAttributionBBoxes(JSON.parse(xmlhttp.responseText));
          }
        }, this);
        xmlhttp.open("GET", this._attributionUrl, true);
        xmlhttp.send();
      },
    
      _parseAttributionBBoxes: function _parseAttributionBBoxes(json) {
        if (!this._map) { return; }
        var providers = json[this.options.scheme.split('.')[0]] || json.normal;
        for (var i=0; i<providers.length; i++) {
          if (providers[i].boxes) {
            for (var j=0; j<providers[i].boxes.length; j++) {
              var box = providers[i].boxes[j];
              providers[i].boxes[j] = L.latLngBounds( [ [box[0], box[1]], [box[2], box[3]] ]);
            }
          }
        }
    
        this._map.on('moveend zoomend resetview', this._findCopyrightBBox, this);
    
        this._attributionProviders = providers;
    
        this._findCopyrightBBox();
      },
    
      _findCopyrightBBox: function _findCopyrightBBox() {
        if (!this._map) { return; }
        var providers = this._attributionProviders;
        var visibleProviders = [];
        var zoom = this._map.getZoom();
        var visibleBounds = this._map.getBounds();
    
        for (var i=0; i<providers.length; i++) {
          if (providers[i].minLevel < zoom && providers[i].maxLevel > zoom)
    
          if (!providers[i].boxes) {
            // No boxes = attribution always visible
            visibleProviders.push(providers[i]);
            break;
          }
    
          for (var j=0; j<providers[i].boxes.length; j++) {
            var box = providers[i].boxes[j];
            if (visibleBounds.overlaps(box)) {
              visibleProviders.push(providers[i]);
              break;
            }
          }
        }
    
        var attributions = ['<a href="https://legal.here.com/terms/serviceterms/gb/">HERE maps</a>'];
        for (var i=0; i<visibleProviders.length; i++) {
          var provider = visibleProviders[i];
          attributions.push('<abbr title="' + provider.alt + '">' + provider.label + '</abbr>');
        }
    
        var attributionText = 'Â© ' + attributions.join(', ') + '. ';
    
        if (attributionText !== this._attributionText) {
          this._map.attributionControl.removeAttribution(this._attributionText);
          this._map.attributionControl.addAttribution(this._attributionText = attributionText);
        }
      },    
    });

    




  }
}
