import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import {
  ICreatePersonRequest,
  IPersonDto,
  IUpdatePersonRequest,
} from "./contracts/interfaces";
import { Observable } from "rxjs/internal/Observable";

@Injectable({
  providedIn: "root",
})
export class PersonsService {
  private baseUrl: string;

  constructor(private _httpClient: HttpClient) {
    this.baseUrl = "https://localhost:5001";
  }

  public getPersons(customerUid: string): Observable<IPersonDto[]> {
    return this._httpClient.get<IPersonDto[]>(
      `${this.baseUrl}/api/customers/${customerUid}/persons`
    );
  }

  public getPerson(
    customerUid: string,
    personUid: string
  ): Observable<IPersonDto> {
    return this._httpClient.get<IPersonDto>(
      `${this.baseUrl}/api/customers/${customerUid}/persons/${personUid}`
    );
  }

  public removePerson(
    customerUid: string,
    personUid: string
  ): Observable<void> {
    return this._httpClient.delete<void>(
      `${this.baseUrl}/api/customers/${customerUid}/persons/${personUid}`
    );
  }

  public addPerson(
    customerUid: string,
    request: ICreatePersonRequest
  ): Observable<void> {
    return this._httpClient.post<void>(
      `${this.baseUrl}/api/customers/${customerUid}/persons`,
      request
    );
  }

  public updatePerson(
    customerUid: string,
    personUid: string,
    request: IUpdatePersonRequest
  ): Observable<void> {
    return this._httpClient.patch<void>(
      `${this.baseUrl}/api/customers/${customerUid}/persons/${personUid}`,
      request
    );
  }
}
