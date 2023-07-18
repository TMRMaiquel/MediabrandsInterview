import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ErrorInfo {

  constructor() {
    this.reset();
  }

  //Miembros
  public message: string;
  public messages: string[] = [];
  public icon: string;
  public dismissable: boolean;
  public header: string;
  public imageIcon: string;
  public iconColor: string;
  public response: Response = null;

  //Métodos
  public reset() {
    this.message = "";
    this.messages = [];
    this.header = "";
    this.dismissable = false;
    this.icon = "warning";
    this.imageIcon = "warning";
    this.iconColor = "inherit";
  }
  public show(msgOrMsgs: string | string[], icon?: string, iconColor?: string) {
    try {
      if (typeof msgOrMsgs === "string") { this.message = msgOrMsgs as string; }
      else { this.messages = msgOrMsgs; }
      this.icon = icon ? icon : "warning";
      if (iconColor) { this.iconColor = iconColor; }
      this.fixupIcons();
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public error(msgOrMsgs: string | string[]) {
    this.show(msgOrMsgs, "danger");
  }
  public info(msg) {
    this.show(msg, "info");
  }
  public fixupIcons() {
    try {
      var err = this;

      if (err.icon === "info")
        err.imageIcon = "info-circle";
      if (err.icon === "warning") {
        err.imageIcon = "warning";
        err.iconColor = "firebrick";
      }
      if (err.icon === "error" || err.icon === "danger") {
        err.imageIcon = "times-circle";
        err.iconColor = "firebrick";
      }
      if (err.icon === "success") {
        err.imageIcon = "check";
        err.iconColor = "green";
      }
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public parseObservableResponseError(response): Observable<any> {
    try {
      //Autoreferencia a los métodos de la clase errorInfo que se pierden cuando otra clase ejecuta o se suscribe a este observable.
      let errorInfo = new ErrorInfo();
      if (response.hasOwnProperty("error")) {
        var error = response["error"];
        if (error !== null && error !== undefined) {
          if (error.constructor === ArrayBuffer) { error = errorInfo.decodeErrorArrayBuffer(error); }
          if (error.hasOwnProperty("typeError")) {
            var typeError = error["typeError"] as number;
            switch (typeError) {
              case 1:
                var responseToTypeError01 = { message: error["message"], modelState: error["errorsModelState"], typeError: ErrorsOfServer.BusinessObject };
                return throwError(responseToTypeError01);
              case 2:
                var responseToTypeError02 = { message: error["message"], maintenanceState: error["errorsMaintenanceState"], typeError: ErrorsOfServer.Maintenance };
                return throwError(responseToTypeError02);
              default:
                break;
            }

          }
        }
        else {
          var status = response["status"];
          if (status == 403 || status == 401) {
            var responseToTypeErrorNull = { message: "Forbidden", typeError: ErrorsOfServer.AspNetIdentity };
            return throwError(responseToTypeErrorNull);
          }
        }

        if (errorInfo.hasOwnProperty("message"))
          return throwError(errorInfo);
        if (errorInfo.hasOwnProperty("Message")) {
          errorInfo.message = errorInfo["Message"];
          return throwError(errorInfo);
        }
      }

      if (response.hasOwnProperty("message"))
        return throwError(response);
      if (response.hasOwnProperty("Message")) {
        response.message = response.Message;
        return throwError(response);
      }

      errorInfo.response = response;
      errorInfo.message = response.statusText;

      try {
        let data = response.json();
        if (data && data.message)
          errorInfo.message = data.message;
      }
      catch (ex) {
      }

      if (!errorInfo.message)
        errorInfo.message = "Unknown server failure.";

      return throwError(errorInfo);
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  private decodeErrorArrayBuffer = (errorInArrayBuffer: any) => {
    try {
      var decodedString = String.fromCharCode.apply(null, new Uint8Array(errorInArrayBuffer));
      var obj = JSON.parse(decodedString);
      return obj;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
}

export enum ErrorsOfServer {
  None,
  BusinessObject,
  Maintenance,
  AspNetIdentity,
  Oauth
}
