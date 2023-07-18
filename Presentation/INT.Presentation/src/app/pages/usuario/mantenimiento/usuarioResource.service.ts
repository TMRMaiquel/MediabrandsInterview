import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { DTOUsuario } from '../../../shared/dto/admin.dtos';
import { ACTION_NEW } from '../../../shared/resources/app.constant';


@Injectable({
  providedIn: 'root'
})
export class UsuarioResourceService {

  constructor() { }

  private actionComponentBS = new BehaviorSubject<number>(ACTION_NEW);
  public actionComponent = this.actionComponentBS.asObservable();

  private usuarioSelectedBS = new BehaviorSubject<DTOUsuario>(null);
  public usuarioSelected = this.usuarioSelectedBS.asObservable();

  public setActionComponent(action: number) {
    try {
      this.actionComponentBS.next(action);
    }
    catch (ex) {
      console.error(ex["message"])
    }
  }
  public setUsuarioSelected(usuario: DTOUsuario) {
    try {
      this.usuarioSelectedBS.next(usuario);

    } catch (ex) {
      console.error(ex["message"]);
    }
  }

}
