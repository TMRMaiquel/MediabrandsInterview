import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import notify from 'devextreme/ui/notify';
import { combineLatest, concatMap, first, forkJoin, Observable, Subscription } from 'rxjs';
import { DTOPerfil, DTOUsuario, DTOUsuarioPerfil } from '../../../shared/dto/admin.dtos';
import { MethodsCommon } from '../../../shared/resources/app.common';
import { ACTION_NEW, ACTION_UPDATE } from '../../../shared/resources/app.constant';
import { UsuarioService } from '../../../shared/services/usuario.service';


import { UsuarioResourceService } from './usuarioResource.service';

@Component({
  providers: [MethodsCommon],
  templateUrl: 'usuario.component.html',
  styleUrls: ['./usuario.component.scss']
})
export class UsuarioComponent implements OnInit, OnDestroy {

  constructor(private usuarioService: UsuarioService,
    private usuarioResourceService: UsuarioResourceService,
    private methodsCommon: MethodsCommon,
    private router: Router
  ) {
  }

  //Miembros
  //*Formulario*//
  public formUsuario: FormGroup = null;
  public usuario: DTOUsuario = null;
  public listaPerfil: DTOPerfil[] = [];
  public listaUsuarioPerfil: DTOUsuarioPerfil[] = [];
  public detailForm: any = null;
  public actionToUpKeep: number = 0;
  public labelMode = 'static';
  public stylingMode = 'outlined';
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
      this.formUsuario = new FormGroup({
        nombre: new FormControl(),
        apellidoPaterno: new FormControl(),
        apellidoMaterno: new FormControl(),
        perfiles: new FormControl()
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
      return combineLatest([this.usuarioResourceService.actionComponent,
      this.usuarioResourceService.usuarioSelected]).pipe(first(),
        concatMap(rptaResourceService => {
          this.actionToUpKeep = rptaResourceService[0] !== null && rptaResourceService[0] !== undefined ? rptaResourceService[0] as number : ACTION_NEW;
          this.usuario = rptaResourceService[1] !== null && rptaResourceService[1] !== undefined ? rptaResourceService[1] as DTOUsuario : new DTOUsuario();

          var arrayToForkJoin: any[] = [
            this.usuarioService.getDependencies(this.usuario.id)
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
      this.listaPerfil = rptaServer[0].dependencies.listaPerfil;
      this.listaUsuarioPerfil = rptaServer[0].dependencies.listaUsuarioPerfil;

      if (this.actionToUpKeep === ACTION_NEW) {
      }
      else if (this.actionToUpKeep === ACTION_UPDATE) {
        this.usuario = rptaServer[0].dependencies.usuario;
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
        this.usuario = new DTOUsuario();
      }

      let listaPerfil: DTOPerfil[] = this.formUsuario.get("perfiles").value as DTOPerfil[];

      this.usuario.nombre = this.formUsuario.get("nombre").value;
      this.usuario.apellidoPaterno = this.formUsuario.get("apellidoPaterno").value;
      this.usuario.apellidoMaterno = this.formUsuario.get("apellidoMaterno").value;

      this.detailForm["listaPerfil"] = listaPerfil;
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
      this.usuarioService.insert(this.usuario, this.detailForm).subscribe(rptAPI => {
        let usuario = rptAPI.usuario as DTOUsuario;
        this.usuario = usuario;
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
      this.usuarioService.update(this.usuario, this.detailForm).subscribe(rptAPI => {
        let usuario = rptAPI.usuario as DTOUsuario;
        this.usuario = usuario;
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
      this.usuarioResourceService.setActionComponent(ACTION_NEW);
      this.usuarioResourceService.setUsuarioSelected(null);
    } catch (ex) {
      console.error(ex["message"]);
    }
  }
}
