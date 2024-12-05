import {
  ICreateLocationRequest,
  ILocationDto,
  IUpdateLocationRequest,
} from "./contracts/interfaces";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { IGroupDto } from "../groups/contracts/interfaces";

@Injectable({
  providedIn: "root",
})
export class LocationsService {
  private baseUrl: string;

  constructor(private _httpClient: HttpClient) {
    this.baseUrl = "https://localhost:5001";
  }

  public getLocations(customerUid: string): Observable<ILocationDto[]> {
    return this._httpClient.get<ILocationDto[]>(
      `${this.baseUrl}/api/customers/${customerUid}/locations`
    );
  }

  public getGroupsWithAccessToLocation(
    customerUid: string,
    locationUid: string
  ): Observable<IGroupDto[]> {
    return this._httpClient.get<IGroupDto[]>(
      `${this.baseUrl}/api/customers/${customerUid}/locations/${locationUid}/groups`
    );
  }

  public getLocation(
    customerUid: string,
    locationUid: string
  ): Observable<ILocationDto> {
    return this._httpClient.get<ILocationDto>(
      `${this.baseUrl}/api/customers/${customerUid}/locations/${locationUid}`
    );
  }

  public removeLocation(
    customerUid: string,
    locationUid: string
  ): Observable<void> {
    return this._httpClient.delete<void>(
      `${this.baseUrl}/api/customers/${customerUid}/locations/${locationUid}`
    );
  }

  public removeGroupFromLocation(
    customerUid: string,
    locationUid: string,
    groupUid: string
  ): Observable<void> {
    return this._httpClient.delete<void>(
      `${this.baseUrl}/api/customers/${customerUid}/locations/${locationUid}/groups/${groupUid}`
    );
  }

  public addLocation(
    customerUid: string,
    request: ICreateLocationRequest
  ): Observable<void> {
    return this._httpClient.post<void>(
      `${this.baseUrl}/api/customers/${customerUid}/locations`,
      request
    );
  }

  public addGroupToLocation(
    customerUid: string,
    locationUid: string,
    groupUid: string
  ): Observable<void> {
    return this._httpClient.post<void>(
      `${this.baseUrl}/api/customers/${customerUid}/locations/${locationUid}/groups/${groupUid}`,
      null
    );
  }

  public updateLocation(
    customerUid: string,
    locationUid: string,
    request: IUpdateLocationRequest
  ): Observable<void> {
    return this._httpClient.patch<void>(
      `${this.baseUrl}/api/customers/${customerUid}/locations/${locationUid}`,
      request
    );
  }
}
