import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { environment } from "../../../environments/environment";
import { DTOEmployee, DTOUsuario } from "../dto/admin.dtos";
import { MethodsCommon } from '../resources/app.common';
import { ErrorInfo } from "../resources/app.errorInfo";

const positions: string[] = [
  'HR Manager',
  'IT Manager',
  'CEO',
  'Controller',
  'Sales Manager',
  'Support Manager',
  'Shipping Manager',
];

const states: string[] = ['AL', 'AK', 'AZ', 'AR', 'CA', 'CO', 'CT', 'DE', 'FL', 'GA', 'HI', 'ID', 'IL', 'IN', 'IA', 'KS', 'KY', 'LA', 'ME', 'MD', 'MA', 'MI', 'MN', 'MS', 'MO', 'MT', 'NE', 'NV', 'NH', 'NJ', 'NM', 'NY', 'NC', 'ND', 'OH', 'OK', 'OR', 'PA', 'RI', 'SC', 'SD', 'TN', 'TX', 'UT', 'VT', 'VA', 'WA', 'WV', 'WI', 'WY'];

@Injectable({
  providedIn: 'root'
}
)
export class UsuarioService {

  constructor(private httpClient: HttpClient) { }

  getPositions() {
    return positions;
  }

  getStates() {
    return states;
  }

  private urlApi: string = environment.apiUrl + "admin/usuario/";
  public methodsCommon: MethodsCommon = new MethodsCommon();

  public getDependencies(id: any): Observable<any> {
    try {

      const params = new HttpParams()
        .set('id', id);

      return this.httpClient.get<any>(this.urlApi + "getDependencies", { params: params })
        .pipe(
          map(response => { return response }),
          catchError(new ErrorInfo().parseObservableResponseError)
        );
    }
    catch (ex) {
      console.error(ex['message']);
    }
  }

  public getByParameters(filterToSearch: any): Observable<any> {
    try {

      var objectJSON = {
        DTOFilterToSearch: filterToSearch
      };

      return this.httpClient.post<any>(this.urlApi + "getByParameters", objectJSON)
        .pipe(
          map((response) => {
            return response;
          }),
          catchError(new ErrorInfo().parseObservableResponseError)
        );
    } catch (ex) {
      console.error(ex["message"]);
    }
  }

  public insert(usuario: DTOUsuario, detailForm: any): Observable<any> {
    try {

      var objectJSON = { DTOUsuario: usuario, DTODetailForm: detailForm };

      return this.httpClient.post<any>(this.urlApi + "insert", objectJSON)
        .pipe(
          map(respuestaAPI => { return respuestaAPI }),
          catchError(new ErrorInfo().parseObservableResponseError)
        );
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }

  public update(usuario: DTOUsuario, detailForm: any): Observable<any> {
    try {

      var objectJSON = { DTOUsuario: usuario, DTODetailForm: detailForm };

      return this.httpClient.post<any>(this.urlApi + "update", objectJSON)
        .pipe(
          map(respuestaAPI => { return respuestaAPI }),
          catchError(new ErrorInfo().parseObservableResponseError)
        );
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }

  public delete(usuario: DTOUsuario): Observable<any> {
    try {
      var objectJSON = { DTOUsuario: usuario };

      return this.httpClient.post<any>(this.urlApi + "delete", objectJSON)
        .pipe(
          map(response => { return response }),
          catchError(new ErrorInfo().parseObservableResponseError)
        );
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }

}
