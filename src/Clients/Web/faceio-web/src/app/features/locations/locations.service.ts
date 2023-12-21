import { ILocationDto } from './contracts/interfaces';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LocationsService {

  private baseUrl: string;

  constructor(private _httpClient: HttpClient) {
    this.baseUrl = 'https://localhost:32768';
   }

  public getLocations(customerUid: string): Observable<ILocationDto[]>{
    return this._httpClient.get<ILocationDto[]>(`${this.baseUrl}/api/customers/${customerUid}/locations`);
  }
}
