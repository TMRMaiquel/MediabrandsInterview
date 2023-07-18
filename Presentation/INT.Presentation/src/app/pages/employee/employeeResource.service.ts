import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { DTOEmployee } from '../../shared/dto/admin.dtos';
import { ACTION_NEW } from '../../shared/resources/app.constant';

@Injectable({
  providedIn: 'root'
})
export class EmployeeResourceService {

  constructor() { }

  private actionComponentBS = new BehaviorSubject<number>(ACTION_NEW);
  public actionComponent = this.actionComponentBS.asObservable();

  private employeeSelectedBS = new BehaviorSubject<DTOEmployee>(null);
  public employeeSelected = this.employeeSelectedBS.asObservable();

  public setActionComponent(action: number) {
    try {
      this.actionComponentBS.next(action);
    }
    catch (ex) {
      console.error(ex["message"])
    }
  }
  public setEmployeeSelected(employee: DTOEmployee) {
    try {
      this.employeeSelectedBS.next(employee);

    } catch (ex) {
      console.error(ex["message"]);
    }
  }

}
