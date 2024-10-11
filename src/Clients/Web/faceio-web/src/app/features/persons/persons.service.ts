import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import {
  ICreatePersonRequest,
  IPersonDto,
  IUpdatePersonRequest,
} from "./contracts/interfaces";
import { Observable } from "rxjs/internal/Observable";
import { IGroupDto } from "../groups/contracts/interfaces";

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

  public getPersonGroups(
    customerUid: string,
    personUid: string
  ): Observable<IGroupDto[]> {
    return this._httpClient.get<IGroupDto[]>(
      `${this.baseUrl}/api/customers/${customerUid}/persons/${personUid}/groups`
    );
  }

  public getPersonFace(
    customerUid: string,
    personUid: string
  ): Observable<string> {
    return this._httpClient.get(
      `${this.baseUrl}/api/customers/${customerUid}/persons/${personUid}/face`, {responseType: 'text'}
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

  public removePersonFromGroup(
    customerUid: string,
    groupUid: string,
    personUid: string
  ): Observable<void> {
    return this._httpClient.delete<void>(
      `${this.baseUrl}/api/customers/${customerUid}/groups/${groupUid}/persons/${personUid}`
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

  public uploadPersonFace(
    customerUid: string,
    personUid: string,
    file: File
  ): Observable<void> {
    const formData: FormData = new FormData();
    formData.append('image', file, file.name);

    return this._httpClient.post<void>(
      `${this.baseUrl}/api/customers/${customerUid}/persons/${personUid}/face`,
      formData
    );
  }
}
