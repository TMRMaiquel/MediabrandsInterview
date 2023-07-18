import { Injectable } from "@angular/core";
import { DATE_CREATION_DEFAULT, STRING_EMPTY } from "../resources/app.constant";

@Injectable()
export class DTOEmployee {
  id: number = 0;
  idOffice: number = 0;
  name: string = STRING_EMPTY;
  firstLastName: string = STRING_EMPTY;
  secondLastName: string = null;
  address: string = STRING_EMPTY;
  birthDate: string = DATE_CREATION_DEFAULT;
  hireDate: string = DATE_CREATION_DEFAULT;
  phone: string = STRING_EMPTY;
  note: string = null;
  state: boolean = true;

  //PROPIEDADES DE NAVEGACION
  office: DTOOffice = null;
  listEmployeePosition: DTOEmployeePosition[] = [];
}

@Injectable()
export class DTOEmployeePosition {
  idEmployee: number = 0;
  idPosition: number = 0;
  state: boolean = true;

  //PROPIEDADES DE NAVEGACION
  employee: DTOEmployee = null;
  position: DTOPosition = null;
}

@Injectable()
export class DTOOffice {
  id: number = 0;
  name: string = STRING_EMPTY;
  state: boolean = true;

  //PROPIEDADES DE NAVEGACION
  listEmployee: DTOEmployee[] = [];

}

@Injectable()
export class DTOPosition {
  id: number = 0;
  name: string = STRING_EMPTY;
  state: boolean = true;

  //PROPIEDADES DE NAVEGACION
  listEmployeePosition: DTOEmployeePosition[] = [];
}

@Injectable()
export class DTOUsuario {
  id: number = 0;
  nombre: string = STRING_EMPTY;
  apellidoPaterno: string = STRING_EMPTY;
  apellidoMaterno: string = STRING_EMPTY;
  state: boolean = true;

  //PROPIEDADES DE NAVEGACION
  listaUsuarioPerfil: DTOUsuarioPerfil[] = [];
}

@Injectable()
export class DTOPerfil {
  id: number = 0;
  nombre: string = STRING_EMPTY;
  state: boolean = true;

  //PROPIEDADES DE NAVEGACION
  listaUsuarioPerfil: DTOUsuarioPerfil[] = [];
}

@Injectable()
export class DTOUsuarioPerfil {
  idUsuario: number = 0;
  idPerfil: number = 0;
  state: boolean = true;

  //PROPIEDADES DE NAVEGACION
  usuario: DTOUsuario[] = [];
  perfil: DTOPerfil[] = [];
}
