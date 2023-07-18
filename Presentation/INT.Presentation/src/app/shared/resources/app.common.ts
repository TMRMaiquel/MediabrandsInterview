import { Location } from '@angular/common';
import { Injectable } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { ActivatedRouteSnapshot, CanDeactivate, Router } from '@angular/router';
import { saveAs } from "file-saver";
import * as _ from 'lodash';
import * as cloneDeep from 'lodash/cloneDeep';
import * as math from 'mathjs';
import * as moment from 'moment';
import { NumeralPipe } from 'ngx-numeral';

declare var window: any;

@Injectable()
export class MethodsCommon {

  //Uso General
  public esEmailValido(email: string): boolean {

    // Primera verificacion
    if (email == null || email == undefined || email == '') return false;

    // Segunda verificacions
    let mailValido = false;
    'use strict';

    var EMAIL_REGEX = /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;;

    if (email.match(EMAIL_REGEX)) {
      mailValido = true;
    }
    return mailValido;
  }
  public elTipoDeArchivoEsValido(extension: string, listaExtensiones: string[]) {
    let index = listaExtensiones.findIndex(x => x == extension);
    extension = '.' + extension;
    let index2 = listaExtensiones.findIndex(x => x == extension);
    return (index > -1 || index2 > -1);
  }
  public agregarColumnasAdicionales() {
    let columnasAdicionales: string[] = ['Nro Poliza', 'Nro Doc Cobranza', 'Fecha Emisión'];

    for (let i = 1; i <= 15; i++) {
      columnasAdicionales.push("Nro Cupón " + i.toString());
      columnasAdicionales.push("Monto " + i.toString());
      columnasAdicionales.push("F. Vencimiento Cupón " + i.toString());
    }

    columnasAdicionales.push("Código Observación CIA");
    columnasAdicionales.push("Descripción Observación CIA");

    return columnasAdicionales;
  }
  public quitarPrefijoGuionDeObjectosEnUnaLista(rowDataTemporal: any = []) {
    var listaObjetos = [];
    for (let i = 0; i < rowDataTemporal.length; i++) {
      let objeto: any = {};
      for (const [key, value] of Object.entries(rowDataTemporal[i])) {
        objeto[key.substring(1)] = value;
      }
      listaObjetos.push(objeto);
    }
    return listaObjetos;
  }
  public primeraLetraEnMayuscula(str) {
    var splitStr = str.toLowerCase().split(' ');
    for (var i = 0; i < splitStr.length; i++) {
      // Assign back to the array
      splitStr[i] = splitStr[i].charAt(0).toUpperCase() + splitStr[i].substring(1);
    }
    // Directly return the joined string
    return splitStr.join(' ');
  }
  public removeAllSpaceWhite(str: string) {
    try {
      if (str != null) {
        return str.replace(/\s/g, "");
      }
      return str;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public oneSpaceWhiteByString(str: string) {
    try {
      if (str != null) {
        return str.replace(/\s+/g, ' ').trim();
      }
      return str;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public correctNumberDecimal(value: any, amountDecimals: number, format?: boolean) {
    try {
      if (typeof value === "number") { value = value.toString(); }
      else if (value == null || value == undefined) { value = ""; }
      if (value !== "") {
        var decimalCheck = value.split('.');
        if (decimalCheck[0] !== undefined) {
          //Elimina valores nulos(0) innecesarios a la izquierda del número entero.
          decimalCheck[0] = decimalCheck[0].replace(/^0+/, '');
          if (decimalCheck[0] === '') { decimalCheck[0] = "0"; }
        }
        //Completa con ceros las cifras de la sección decimal que el usuario no ha ingresado.
        if (decimalCheck[1] !== undefined) {
          if (decimalCheck[1] === "") { value = decimalCheck[0] + '.' + "0".repeat(amountDecimals); }
          else if (decimalCheck[1].length < amountDecimals) {
            value = decimalCheck[0] + '.' + decimalCheck[1] + "0".repeat(amountDecimals - decimalCheck[1].length);
          }
          else { value = decimalCheck[0] + '.' + decimalCheck[1]; }
        }
        else { value = decimalCheck[0] + '.' + "0".repeat(amountDecimals); }
      }
      else { value = "0." + "0".repeat(amountDecimals); }
      if (format) {
        const numeral = new NumeralPipe(value);
        return numeral.format('0,0.' + '0'.repeat(amountDecimals));
      } else { return value; }
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public cloneObject(value: any) {
    try {
      return JSON.parse(JSON.stringify(value));
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public cloneWithLodash(value: any) {
    try {
      //Realiza la copia de un objeto incluyendo funciones.
      return cloneDeep(value);
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public roundNumber(value: any, scale: any) {
    try {
      return _.round(value, scale);
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public removeCommas(value: any) {
    try {
      if (value == null || value == undefined) { return null; }
      if (!isNaN(value)) { value = value.toString(); }
      return value.replace(/,/g, '');
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public downloadFile(blob: any, nameFile: string, typeFile: string) {
    try {
      var url = "";
      var blobTotal = new Blob(blob, { type: typeFile });
      var success = false;

      try {
        var navigator = window.navigator;

        // Intentamos usar msSaveBlob (Navegador IE 10+)
        console.log("Intentando el método saveBlob...");
        if (navigator.msSaveBlob)
          navigator.msSaveBlob(blobTotal, nameFile);
        else {
          // Intentamos usar otros métodos saveBlob si están disponibles.
          var saveBlob = navigator.webkitSaveBlob || navigator.mozSaveBlob || navigator.saveBlob;
          if (saveBlob === undefined) throw "No soportado";
          saveBlob(blobTotal, nameFile);
        }
        console.log("saveBlob utilizado con éxito");
        success = true;
      } catch (ex) {
        console.log("saveBlob método falló con la siguiente excepción:");
        console.log(ex);
      }

      if (!success) {
        // Conseguimos un blob url creator
        var urlCreator = window.URL || window.webkitURL || window.mozURL || window.msURL;
        if (urlCreator) {
          // Intentamos usar un link para la descarga
          var link = document.createElement("a");
          //Verificamos si la etiqueta link (a) tiene el atributo "download"
          if ("download" in link) {
            // Intentamos simular un click por parte del usuario
            try {
              // Preparamos un blob url
              console.log("Intentando simular la descarga a través de un link ...");
              url = urlCreator.createObjectURL(blobTotal);
              link.setAttribute("href", url);

              //Cambiamos el valor del atributo download (Supported in Chrome 14+ / Firefox 20+)
              link.setAttribute("download", nameFile);

              //Simulamos el evento click del atributo download en la etiqueta link
              var event = document.createEvent('MouseEvents');
              event.initMouseEvent("click", true, true, window, 1, 0, 0, 0, 0, false, false, false, false, 0, null);
              link.dispatchEvent(event);
              console.log("Descarga a través de la etiqueta link satisfactoria");
              success = true;

            } catch (ex) {
              console.log("Descarga a través de la etiqueta link con simulación del evento click fallida, con la siguiente excepción:");
              console.log(ex);
            }
          }

          if (!success) {
            // Utilizamos el método método window.location para forzar la descarga
            try {
              // Preparamos un blob url
              // Usar application/octet-stream cuando usamos window.location
              console.log("Intentando la descarga con window.location ...");
              blobTotal = new Blob(blob, { type: "application/octet-stream" });
              url = urlCreator.createObjectURL(blobTotal);
              window.location = url;
              console.log("Descarga con window.location satisfactoria");
              success = true;
            } catch (ex) {
              console.log("Descarga con window.location fallida con la siguiente excepción:");
              console.log(ex);
            }
          }

          if (!success) {
            console.log("Métodos insuficientes para grabar el archivo, usando como último recurso window.open");
            //window.open(httpPath, '_blank', '');
          }
        }
      }
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public downloadFileFromStrBase64(base64File: any, nameFile: string, typeFile: string) {
    try {
      var url = "";
      var success = false;

      try {
        var navigator = window.navigator;

        var byteArray = this.strBase64ToUint8Array(base64File);
        var blobTotal = new Blob([byteArray], { type: typeFile });

        // Intentamos usar msSaveBlob (Navegador IE 10+)
        console.log("Intentando el método saveBlob...");
        if (navigator.msSaveBlob)
          navigator.msSaveBlob(blobTotal, nameFile);
        else {
          // Intentamos usar otros métodos saveBlob si están disponibles.
          var saveBlob = navigator.webkitSaveBlob || navigator.mozSaveBlob || navigator.saveBlob;
          if (saveBlob === undefined) throw "No soportado";
          saveBlob(blobTotal, nameFile);
        }
        console.log("saveBlob utilizado con éxito");
        success = true;
      } catch (ex) {
        console.log("saveBlob método falló con la siguiente excepción:");
        console.log(ex);
      }

      if (!success) {
        // Conseguimos un blob url creator
        var urlCreator = window.URL || window.webkitURL || window.mozURL || window.msURL;
        if (urlCreator) {
          // Intentamos usar un link para la descarga
          var link = document.createElement("a");
          //Verificamos si la etiqueta link (a) tiene el atributo "download"
          if ("download" in link) {
            // Intentamos simular un click por parte del usuario
            try {
              // Preparamos un blob url
              console.log("Intentando simular la descarga a través de un link ...");
              url = urlCreator.createObjectURL(blobTotal);
              link.setAttribute("href", url);

              //Cambiamos el valor del atributo download (Supported in Chrome 14+ / Firefox 20+)
              link.setAttribute("download", nameFile);

              //Simulamos el evento click del atributo download en la etiqueta link
              var event = document.createEvent('MouseEvents');
              event.initMouseEvent("click", true, true, window, 1, 0, 0, 0, 0, false, false, false, false, 0, null);
              link.dispatchEvent(event);
              console.log("Descarga a través de la etiqueta link satisfactoria");
              success = true;

            } catch (ex) {
              console.log("Descarga a través de la etiqueta link con simulación del evento click fallida, con la siguiente excepción:");
              console.log(ex);
            }
          }

          if (!success) {
            // Utilizamos el método método window.location para forzar la descarga
            try {
              // Preparamos un blob url
              // Usar application/octet-stream cuando usamos window.location
              console.log("Intentando la descarga con window.location ...");

              var byteArray = this.strBase64ToUint8Array(base64File);
              blobTotal = new Blob([byteArray], { type: "application/octet-stream" });
              url = urlCreator.createObjectURL(blobTotal);
              window.location = url;
              console.log("Descarga con window.location satisfactoria");
              success = true;
            } catch (ex) {
              console.log("Descarga con window.location fallida con la siguiente excepción:");
              console.log(ex);
            }
          }

          if (!success) {
            console.log("Métodos insuficientes para grabar el archivo, usando como último recurso window.open");
            //window.open(httpPath, '_blank', '');
          }
        }
      }
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public downloadFileFrom64Bytes(fileDownload: string, type: string, name: string) {
    try {
      const byteCharacters = atob(fileDownload);
      const byteNumbers = new Array(byteCharacters.length);
      for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
      }

      const byteArray = new Uint8Array(byteNumbers);
      const blob = new Blob([byteArray], { type: type });
      let file = name;
      saveAs(blob, file);
    }
    catch (ex) {
      console.log(ex["message"]);
    }
  }
  public strBase64ToUint8Array(strBase64: string): Uint8Array {
    try {
      var byteCharacters = atob(strBase64);
      var byteNumbers = new Array(byteCharacters.length);
      for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
      }
      return new Uint8Array(byteNumbers);
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public isEmptyOrNull(s: string) {
    return s == null || s.length === 0;
  }
  public getIdMax(list: any[], nameProperty: string = "id") {
    try {
      var maxId = 0;
      if (list.length > 0) {
        maxId = Math.max.apply(Math, list.map(function (obj) {
          if (obj[nameProperty] !== null && obj[nameProperty] !== undefined) {
            if (math.isInteger(obj[nameProperty])) {
              return obj[nameProperty]
            }
          }
          else {
            throw "La propiedad asociada con el Id no existe en los objetos";
          }
        }));
      }
      return maxId;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public insertarTextoPorPosicionCursor(controlInput: HTMLInputElement, texto) {
    try {
      if (controlInput.selectionStart === 0) {
        var posicionInicial = controlInput.selectionStart;
        var posionFinal = controlInput.selectionEnd;
        controlInput.value = controlInput.value.substring(0, posicionInicial)
          + texto
          + controlInput.value.substring(posionFinal, controlInput.value.length);
        controlInput.selectionStart = posicionInicial + texto.length;
        controlInput.selectionEnd = posicionInicial + texto.length;
      } else {
        controlInput.value += texto;
      }
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public documentoValidoParaVisualizacion(contentType: string): boolean {
    try {

      let valido: boolean = false;

      let listaContentType: string[] = [];
      listaContentType.push("image/jpeg");
      listaContentType.push("application/pdf");
      listaContentType.push("image/png");

      let contentTypeConsultado: string = listaContentType.find(x => x == contentType);
      if (contentTypeConsultado !== null && contentTypeConsultado !== undefined) { valido = true; }

      return valido;

    } catch (ex) {
      console.error(ex["message"]);
    }
  }

  //Formularios
  public focusById(id) {
    try {
      setTimeout(() => {
        //We have access to the context values
        var element = window.document.getElementById(id);
        if (element) { element.focus(); }
      }, 0);
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public scrollWindow(x_coord: number, y_coord: number, tiempo_en_ms: number = 100) {
    try {
      setTimeout(() => {
        window.scrollTo(x_coord, y_coord);
      }, tiempo_en_ms);
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public selectItem(stateButtonsEditionForm: StateButtonsEditionForm) {
    try {
      if (stateButtonsEditionForm !== null) {
        stateButtonsEditionForm.new = false;
        stateButtonsEditionForm.list = false;
        stateButtonsEditionForm.save = true;
        stateButtonsEditionForm.edit = false;
        stateButtonsEditionForm.delete = false;
        stateButtonsEditionForm.cancel = false;
      }
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public getControlName(formControl: AbstractControl) {
    try {
      const controls = formControl.parent.controls;
      return Object.keys(controls).find(name => formControl === controls[name]) || "";
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public getNamesSubFormGroup(formGroupMain: AbstractControl, listNames: string[]) {
    try {
      var formControls = (formGroupMain as FormGroup).controls;
      Object.keys(formControls).forEach(name => {
        var formControl = formControls[name];
        if (formControl instanceof FormGroup) {
          listNames.push(name);
          this.getNamesSubFormGroup(formControl, listNames);
        }
      });
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public fixCloseToDatepicker(event: any, datePicker: any) {
    try {
      if (event.target.offsetParent == null) {
        datePicker.close();
      }
      else if (event.target.offsetParent.nodeName != "NGB-DATEPICKER") {
        datePicker.close();
      }
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public getAmountDecimalsToPreCalculate() {
    return 3;
  }
  public getAmountDecimalsToEndCalculate() {
    return 2;
  }
  public obtenerParametro(diccionarioParametro: any, nombreParametro: string): any {
    try {

      var existeDiccionarioParametro: boolean = diccionarioParametro !== null && diccionarioParametro !== undefined && diccionarioParametro.constructor === Object ? true : false;
      var parametro: any = null;

      if (existeDiccionarioParametro) {
        parametro = nombreParametro in diccionarioParametro ? diccionarioParametro[nombreParametro] : null;
      }

      return parametro;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }

  //Rest Api
  public messageToMaintenance(actionMaintenance: ActionMaintenance, stateMaintenance: StateMaintenance) {
    try {
      var message = "";
      switch (stateMaintenance) {
        case StateMaintenance.None:
          switch (actionMaintenance) {
            case ActionMaintenance.New:
              message = "¿Desea guardar el registro?";
              break;
            case ActionMaintenance.Update:
              message = "¿Desea actualizar el registro?";
              break;
          }
          break;
        case StateMaintenance.Success:
          switch (actionMaintenance) {
            case ActionMaintenance.Register:
              message = "Registro guardado exitosamente.";
              break;
            case ActionMaintenance.Update:
              message = "Registro actualizado exitosamente.";
              break;
            case ActionMaintenance.Delete:
              message = "Registro eliminado exitosamente.";
              break;
          }
          break;
        case StateMaintenance.Fail:
          switch (actionMaintenance) {
            case ActionMaintenance.Register:
              message = "Registro no guardado.";
              break;
            case ActionMaintenance.Update:
              message = "Registro no actualizado.";
              break;
            case ActionMaintenance.Delete:
              message = "Registro no eliminado.";
              break;
          }
          break;
      }
      return message;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public convertToArraybuffer(base64: string) {
    try {
      var binary_string = window.atob(base64);
      var len = binary_string.length;
      var bytes = new Uint8Array(len);
      for (var i = 0; i < len; i++) {
        bytes[i] = binary_string.charCodeAt(i);
      }
      return bytes.buffer;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }

  //Fechas
  public convertToISOFormatFromStringDDMMYYYY(stringDate: string) {
    try {
      var date = moment(stringDate, "DD-MM-YYYY");
      date.set("hour", 0); date.set("minute", 0); date.set("second", 0);
      return date;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public convertToISOFormat(ngbDate: any) {
    try {
      if (typeof ngbDate === "object" && ngbDate != null) {
        var stringDate = ngbDate.year.toString() + "-" + ngbDate.month.toString() + "-" + ngbDate.day.toString();
        var date = moment(stringDate, "YYYY MM DD");
        date.set("hour", 0); date.set("minute", 0); date.set("second", 0);
        return date;
      }
      return null;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public convertToISOFormatOnlyDate(ngbDate: any) {
    try {
      if (typeof ngbDate === "object" && ngbDate != null) {
        var stringDate = ngbDate.year.toString() + "-" + ngbDate.month.toString() + "-" + ngbDate.day.toString();
        var date = moment(stringDate, "YYYY MM DD").format("YYYY-MM-DD");
        //date.set("hour", 0); date.set("minute", 0); date.set("second", 0);
        return date;
      }
      return null;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public convertToUTCFormat(moment: moment.Moment, timeZone?: TimeZone) {
    try {
      if (moment !== null && moment !== undefined) {
        if (moment.isValid()) {
          var nuevoMoment = moment.clone();
          if (timeZone !== null && timeZone !== undefined) {
            switch (timeZone) {
              case TimeZone.SAPacificStandardTime:
                return nuevoMoment.utc().subtract(5, 'hours').format();
              case TimeZone.PacificSAStandardTime:
              case TimeZone.SAWesternStandardTime:
                return nuevoMoment.utc().subtract(4, 'hours').format();
              case TimeZone.SAEasternStandardTime:
                return nuevoMoment.utc().subtract(3, 'hours').format();
              default:
                return nuevoMoment.utc().format();
            }
          }
          else {
            return nuevoMoment.utc().subtract(5, 'hours').format();
          }
        }
        else
          return null;
      }
      return null;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public convertToUTCReportFormat(moment: moment.Moment, timeZone?: TimeZone) {
    try {
      if (moment !== null && moment !== undefined) {
        if (moment.isValid()) {
          var nuevoMoment = moment.clone();
          if (timeZone !== null && timeZone !== undefined) {
            switch (timeZone) {
              case TimeZone.SAPacificStandardTime:
                return nuevoMoment.utc().subtract(5, 'hours').format("YYYY-MM-DD");
              case TimeZone.PacificSAStandardTime:
              case TimeZone.SAWesternStandardTime:
                return nuevoMoment.utc().subtract(4, 'hours').format("YYYY-MM-DD");
              case TimeZone.SAEasternStandardTime:
                return nuevoMoment.utc().subtract(3, 'hours').format("YYYY-MM-DD");
              default:
                return nuevoMoment.utc().format("YYYY-MM-DD");
            }
          }
          else {
            return nuevoMoment.utc().subtract(5, 'hours').format("YYYY-MM-DD");
          }
        }
        else
          return null;
      }
      return null;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public convertToMoment(stringDate: string) {
    try {
      if (stringDate !== undefined && stringDate !== null && moment(stringDate).isValid()) {
        return moment(stringDate, "YYYY-MM-DD HH:mm:ss");
      }

      return null;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public convertToMomentOnlyDate(stringDate: string) {
    try {

      if (stringDate !== undefined && stringDate !== null && moment(stringDate).isValid()) {
        return moment(stringDate, "YYYY-MM-DD");
      }

      return null;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public convertToDateFormat01(stringDate: string) {
    try {
      if (moment(stringDate).isValid()) {
        return moment(stringDate, "YYYY MM DD").format("YYYY-MM-DD");
      }
      return stringDate;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public convertToDateFormat02(stringDate: string) {
    try {
      if (moment(stringDate).isValid()) {
        return this.convertToMoment(stringDate).format("DD/MM/YYYY");
      }
      return stringDate;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public convertToDateTimeFormat01(stringDate: string) {
    try {
      if (stringDate !== null && stringDate !== undefined) {
        if (moment(stringDate).isValid()) {
          return this.convertToMoment(stringDate).format("DD/MM/YYYY HH:mm:ss");
        }
      }
      return stringDate;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public getDateNowWithTZ(timeZone: TimeZone) {
    try {
      switch (timeZone) {
        case TimeZone.SAPacificStandardTime:
          return moment().utc().subtract(5, 'hours');
        case TimeZone.PacificSAStandardTime:
        case TimeZone.SAWesternStandardTime:
          return moment().utc().subtract(4, 'hours');
        case TimeZone.SAEasternStandardTime:
          return moment().utc().subtract(3, 'hours');
        default:
          return moment().utc();
      }
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public minutosToStringTiempo(totalMinutos: number) {
    try {
      var horas: number;
      var minutos: number;
      minutos = totalMinutos % 60;
      totalMinutos -= minutos;
      horas = totalMinutos / 60;
      return horas.toString().padStart(2, '0') + ":" + minutos.toString().padStart(2, '0');
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public stringTiempoToTotalMinutos(stringTiempo: string) {
    try {
      var horas: string;
      var minutos: string;
      horas = stringTiempo.substr(0, 2);
      minutos = stringTiempo.substr(3, 2);
      return parseInt(horas) * 60 + parseInt(minutos);
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public completeNumberCaracterLeft(num: number, size: number, caracter: string): string {
    let s = num + "";
    while (s.length < size) s = caracter + s;
    return s;
  }
  public convertToMomentOnlyTime(stringTime: string) {
    try {
      return moment(stringTime, "HH:mm");
    } catch (ex) {
      console.error(ex.message);
    }
  }
  public convertToFormatTime(moment: moment.Moment, includeSeconds?: boolean) {
    try {
      if (moment !== undefined && moment !== null) {
        if (includeSeconds === true) {
          return moment.format('HH:mm:ss');
        }
        else {
          return moment.format('HH:mm');
        }
      }
      return null;
    } catch (ex) {
      console.error(ex.message);
    }
  }
  public convertToMomentTime(stringTime: string) {
    try {
      var date = moment().utc();
      var splitTime = stringTime.split(/:/);
      return date.hours(parseInt(splitTime[0])).minutes(parseInt(splitTime[1])).seconds(0).milliseconds(0);
    }
    catch (ex) {
      console.error(ex.message);
    }
  }
  public clearNgbDate(formGroup: FormGroup, nameControlAssociated: string) {
    try {
      if (formGroup != null) {
        if (formGroup.get(nameControlAssociated) != null) {
          formGroup.get(nameControlAssociated).patchValue(null);
        }
      }
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public clearMomentDate(formGroup: FormGroup, nameControlAssociated: string) {
    try {
      if (formGroup != null) {
        if (formGroup.get(nameControlAssociated) != null) {
          formGroup.get(nameControlAssociated).patchValue(null);
        }
      }
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
}

@Injectable()
export class StateButtonsEditionForm {
  new: boolean = true;
  list: boolean = true;
  save: boolean = true;
  edit: boolean = true;
  delete: boolean = true;
  cancel: boolean = true;
}

@Injectable()
export class StateButtonsModal {
  cancel: boolean = true;
  delete: boolean = true;
  edit: boolean = true;
  list: boolean = true;
  new: boolean = true;
  out: boolean = true;
  save: boolean = true;
}

@Injectable()
export class PuedeNavegarFormularioMantenimiento implements CanDeactivate<any> {
  constructor(
    private readonly location: Location,
    private readonly router: Router) { }

  public canDeactivate(component: any, currentRoute: ActivatedRouteSnapshot): boolean {
    try {
      if (!component["puedeNavegarFormularioBusqueda"]) {
        const currentUrlTree = this.router.createUrlTree([], currentRoute);
        const currentUrl = currentUrlTree.toString();
        this.location.go(currentUrl);
        return false;
      } else {
        return true;
      }
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
}

@Injectable()
export class AutocompleteCustom {

  public formGroup: FormGroup;
  public nameControl: string;
  public propertyToFormatter: string = "";
  public propertyToInputFormatter: string = "";
  public searching: boolean = false;
  public textSearching: string = "Buscando...";
  public searchFailed: boolean = false;
  public textSearchFailed: string = "Sin resultados.";

  public resultFormatter = (result: any): string => {
    try {
      const nestedProperties: string[] = this.propertyToFormatter.split('.');
      var value: any = result;
      for (const prop of nestedProperties) {
        value = value[prop];
      }
      return value;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  };

  public inputFormatter = (result: any): string => {
    try {
      const nestedProperties: string[] = this.propertyToInputFormatter.split('.');
      var value: any = result;
      for (const prop of nestedProperties) {
        value = value[prop];
      }
      return value;
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  };

  public clear() {
    try {
      setTimeout(() => {
        var valueSelected = this.formGroup.get(this.nameControl).value;
        if (typeof valueSelected === "string") {
          this.formGroup.controls[this.nameControl].reset();
          this.formGroup.controls[this.nameControl].patchValue(null);
          this.searching = false;
          this.searchFailed = false;
        }
      }, 300);
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
}

@Injectable()
export class DataTable {

  public filters: Filter[] = [];
  public sort: Sort = new Sort();
  public pagination: Pagination = new Pagination();
  public waitValidationExt: boolean = false;

}

@Injectable()
export class DataTableAG {

  public filters: Filter[] = [];
  public sort: Sort = new Sort();
  public pagination: Pagination = new Pagination();
  public waitValidationExt: boolean = false;

}

@Injectable()
export class Filter {

  public name: string;
  public value: any;
  public value2: any;
  public values: any[];
  public type: OptionFilter = OptionFilter.None;

}

@Injectable()
export class Sort {

  public name: string;
  public type: OptionSort = OptionSort.None;

}

@Injectable()
export class Pagination {

  public page: number = 1;
  public pageSize: number = 10;

}

export enum MessageBoxButtons {
  Yes,
  YesNo,
  YesNoCancel,
  YesCancel,
  Ok,
  NewReplaceCancel
}

export enum MessageBoxTypes {
  Normal,
  Warning,
  Exception,
  ServerException
}

export enum ActionMaintenance {
  New,
  Register,
  Update,
  Delete
}

export enum StateMaintenance {
  None,
  Success,
  Fail
}

export enum ConditionToRestoreButtons {
  SuccessSave,
  FailSave,
  SuccessDelete,
  FailDelete,
  List,
  SuccessEmitSUNAT,
  FailEmitSUNAT
}

export enum OptionFilter {
  None,
  Contains,
  Equals,
  NotEquals,
  StartsWith,
  EndsWith,
  LessThan,
  LessThanOrEqual,
  GreaterThan,
  GreaterThanOrEqual,
  NotContains,
  InRange,
  Set
}

export enum OptionSort {
  None,
  IsAsc,
  IsDesc,
}

export enum ViewInitTypes {
  None,
  Display,
  Edition,
  Register,
}

export enum TimeZone {
  PacificSAStandardTime,
  SAEasternStandardTime,
  SAPacificStandardTime,
  SAWesternStandardTime,
}

export interface DynamicObject {
  [key: string]: any
}

export enum ResultadoMensajeCCS {
  Ninguno,
  Exitoso,
  NoExitoso,
  ErrorModelState,
  ErrorTransaccion,
}
