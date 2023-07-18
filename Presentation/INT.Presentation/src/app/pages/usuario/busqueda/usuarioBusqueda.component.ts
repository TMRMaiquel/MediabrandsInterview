import { Component, OnDestroy, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { MethodsCommon } from "../../../shared/resources/app.common";
import { UsuarioService } from "../../../shared/services/usuario.service";
import { UsuarioResourceService } from "../mantenimiento/usuarioResource.service";

@Component({
  providers: [MethodsCommon],
  templateUrl: 'usuarioBusqueda.component.html',
  styleUrls: ['./usuarioBusqueda.component.scss']
})
export class UsuarioBusquedaComponent implements OnInit, OnDestroy
{
  constructor(private usuarioService: UsuarioService,
    private usuarioResourceService: UsuarioResourceService,
    private methodsCommon: MethodsCommon,
    private router: Router
  ) {
   
  }

  public dataSource: any;

  //Contructor propio de angular
  ngOnInit() {
    //this.setupComponent();
  }

  //Método que permite liberar recursos.
  ngOnDestroy() {
    //this.clearResources();
  }
}
