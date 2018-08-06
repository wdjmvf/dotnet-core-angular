import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class VehicleService {
  private readonly vehicleEndpoint = 'api/vehicles';
  constructor(private http: Http) { }

  getMakes(){
    return this.http.get('/api/makes')
      .map(res => res.json());
  }

  getFeatures(){
    return this.http.get('/api/features')
      .map(res => res.json());
  }

  create(vehicle : any){
    return this.http.post(this.vehicleEndpoint +'/', vehicle)
      .map(res => res.json());
  }

  update(vehicle : any){
    return this.http.put(this.vehicleEndpoint +'/'+ vehicle.id, vehicle)
      .map(res => res.json());
  }

  delete(id:any){
    return this.http.delete(this.vehicleEndpoint +'/'+id)
      .map(res => res.json());
  }

  getVehicle(id : any){
    return this.http.get(this.vehicleEndpoint +'/'+id)
      .map(res => res.json());
  }

  getVehicles(filter: any){
    return this.http.get(this.vehicleEndpoint+'?'+this.toQueryString(filter))
      .map(res => res.json());
  }

  toQueryString(obj: any){
    var parts = [];
    for(var property in obj){
      var value = obj[property];
      if(value != null && value != undefined){
        parts.push(encodeURIComponent(property)+'='+encodeURIComponent(value));
      }
    }
    return parts.join('&');
  }

}
