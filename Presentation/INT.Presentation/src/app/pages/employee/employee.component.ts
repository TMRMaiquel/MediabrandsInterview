import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import notify from 'devextreme/ui/notify';
import { combineLatest, concatMap, first, forkJoin, Observable, Subscription } from 'rxjs';
import { DTOEmployee, DTOOffice, DTOPosition } from '../../shared/dto/admin.dtos';
import { MethodsCommon } from '../../shared/resources/app.common';
import { ACTION_NEW, ACTION_UPDATE, DATE_CREATION_DEFAULT } from '../../shared/resources/app.constant';
import { EmployeeService } from '../../shared/services/employee.service';
import { EmployeeResourceService } from './employeeResource.service';
import * as moment from 'moment';
import { Router } from '@angular/router';

@Component({
  providers: [MethodsCommon],
  templateUrl: 'employee.component.html',
  styleUrls: ['./employee.component.scss']
})
export class EmployeeComponent implements OnInit, OnDestroy {

  constructor(private employeeService: EmployeeService,
    private employeeResourceService: EmployeeResourceService,
    private methodsCommon: MethodsCommon,
    private router: Router
  ) {
    this.listPositions = employeeService.getPositions();
    this.listStates = employeeService.getStates();
  }

  //Miembros
  //*Formulario*//
  public formEmployee: FormGroup = null;
  public employee: DTOEmployee = null;
  public listOffice: DTOOffice[] = [];
  public listPosition: DTOPosition[] = [];
  public detailForm: any = null;
  public actionToUpKeep: number = 0;
  public labelMode = 'static';
  public stylingMode = 'outlined';
  public phoneRules: any = {
    X: /[02-9]/,
  };
  public listPositions: string[] = [];
  public listStates: string[] = [];
  //*Suscripciones*//
  private getDataServerAndComponentAssociatedSubscription: Subscription;

  //Contructor propio de angular
  ngOnInit() {
    this.setupComponent();
  }

  //Método que permite liberar recursos.
  ngOnDestroy() {
    this.clearResources();
  }

  //Métodos
  //*Formulario*//
  public createForm() {
    try {
      this.formEmployee = new FormGroup({
        name: new FormControl(),
        firstLastName: new FormControl(),
        secondLastName: new FormControl(),
        address: new FormControl(),
        birthDate: new FormControl(),
        office: new FormControl(),
        positions: new FormControl(),
        phone: new FormControl(),
        hireDate: new FormControl(),
        note: new FormControl()
      });
    } catch (ex) {
      console.error(ex["message"]);
    }
  }
  public setupComponent() {
    try {
      this.createForm();
      this.getDataServerAndComponentAssociatedSubscription = this.getDataServerAndComponentAssociated().subscribe(rptaServer => {
        this.assignValuesForm(rptaServer);
      });
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public getDataServerAndComponentAssociated(): Observable<any> {
    try {
      return combineLatest([this.employeeResourceService.actionComponent,
      this.employeeResourceService.employeeSelected]).pipe(first(),
        concatMap(rptaResourceService => {
          this.actionToUpKeep = rptaResourceService[0] !== null && rptaResourceService[0] !== undefined ? rptaResourceService[0] as number : ACTION_NEW;
          this.employee = rptaResourceService[1] !== null && rptaResourceService[1] !== undefined ? rptaResourceService[1] as DTOEmployee : new DTOEmployee();

          var arrayToForkJoin: any[] = [
            this.employeeService.getDependencies(this.employee.id)
          ]
          return forkJoin(arrayToForkJoin);
        }));
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public assignValuesForm(rptaServer: any) {
    try {
      this.listOffice = rptaServer[0].dependencies.listOffice;
      this.listPosition = rptaServer[0].dependencies.listPosition;
      //this.router.navigate(['dashboard/flujosTrabajo/gestionTramite/mantenimiento']);

      if (this.actionToUpKeep === ACTION_NEW) {
      }
      else if (this.actionToUpKeep === ACTION_UPDATE) {
        this.employee = rptaServer[0].dependencies.employee;
      }
    }
    catch (ex) {
      console.error(ex["message"]);
    }
  }
  public getValuesForm() {
    try {

      this.detailForm = {};

      if (this.actionToUpKeep === ACTION_NEW) {
        this.employee = new DTOEmployee();
      }

      let office: DTOOffice = this.formEmployee.get("office").value as DTOOffice;
      let birthDate: Date = this.formEmployee.get("birthDate").value as Date;
      let hireDate: Date = this.formEmployee.get("hireDate").value as Date;

      this.employee.name = this.formEmployee.get("name").value;
      this.employee.firstLastName = this.formEmployee.get("firstLastName").value;
      this.employee.secondLastName = this.formEmployee.get("secondLastName").value;
      this.employee.address = this.formEmployee.get("address").value;
      this.employee.birthDate = birthDate !== null && birthDate !== undefined ? this.methodsCommon.convertToUTCFormat(moment(birthDate)) : DATE_CREATION_DEFAULT;
      this.employee.hireDate = hireDate !== null && hireDate !== undefined ? this.methodsCommon.convertToUTCFormat(moment(hireDate)) : DATE_CREATION_DEFAULT;
      this.employee.phone = this.formEmployee.get("phone").value;
      this.employee.note = this.formEmployee.get("note").value;

      this.detailForm["DTOOffice"] = office;
    } catch (ex) {
      console.error(ex["message"]);
    }
  }
  public save($event: any) {
    try {
      const validate = $event.validationGroup.validate();
      if (validate.isValid === true) {
        if (this.actionToUpKeep === ACTION_NEW) {
          this.insert();
        }
        else if (this.actionToUpKeep === ACTION_UPDATE) {
          this.update();
        }
      }
      else {
        notify('El registro no fue grabado. Por favor revisar si todos los campos son válidos.', 'error');
      }
    } catch (ex) {
      console.error(ex["message"]);
    }
  }
  public insert() {
    try {
      this.getValuesForm();
      this.employeeService.insert(this.employee, this.detailForm).subscribe(rptAPI => {
        let employee = rptAPI.employee as DTOEmployee;
        this.employee = employee;
        this.actionToUpKeep = ACTION_UPDATE;
        notify('El registro fue guardado satisfactoriamente.', 'success');
      }, error => {
        console.error(error);
        notify('Error al grabar el registro.', 'error');
      });
    } catch (ex) {
      console.error(ex["message"]);
    }
  }
  public update() {
    try {
      this.getValuesForm();
      this.employeeService.update(this.employee, this.detailForm).subscribe(rptAPI => {
        let employee = rptAPI.employee as DTOEmployee;
        this.employee = employee;
        this.actionToUpKeep = ACTION_UPDATE;
        notify('El registro fue guardado satisfactoriamente.', 'success');
      }, error => {
        console.error(error);
        notify('Error al grabar el registro.', 'error');
      });
    } catch (ex) {
      console.error(ex["message"]);
    }
  }
  public clearResources() {
    try {
      if (this.getDataServerAndComponentAssociatedSubscription !== null && this.getDataServerAndComponentAssociatedSubscription !== undefined) {
        this.getDataServerAndComponentAssociatedSubscription.unsubscribe();
      }
      this.employeeResourceService.setActionComponent(ACTION_NEW);
      this.employeeResourceService.setEmployeeSelected(null);
    } catch (ex) {
      console.error(ex["message"]);
    }
  }
}
