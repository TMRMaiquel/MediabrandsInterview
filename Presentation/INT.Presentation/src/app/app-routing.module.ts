import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { DxButtonModule, DxDataGridModule, DxDateBoxModule, DxFormModule, DxSelectBoxModule, DxTagBoxModule, DxTextAreaModule, DxTextBoxModule, DxValidatorModule } from 'devextreme-angular';
import { EmployeeComponent } from './pages/employee/employee.component';
import { HomeComponent } from './pages/home/home.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { TasksComponent } from './pages/tasks/tasks.component';
import { UsuarioBusquedaComponent } from './pages/usuario/busqueda/usuarioBusqueda.component';
import { UsuarioComponent } from './pages/usuario/mantenimiento/usuario.component';
import { ChangePasswordFormComponent, CreateAccountFormComponent, LoginFormComponent, ResetPasswordFormComponent } from './shared/components';
import { AuthGuardService } from './shared/services';

const routes: Routes = [
  {
    path: 'tasks',
    component: TasksComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'employee',
    component: EmployeeComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'usuario',
    component: UsuarioComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'usuarioBusqueda',
    component: UsuarioBusquedaComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'login-form',
    component: LoginFormComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'reset-password',
    component: ResetPasswordFormComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'create-account',
    component: CreateAccountFormComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'change-password/:recoveryCode',
    component: ChangePasswordFormComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: '**',
    redirectTo: 'home'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true }),
    FormsModule,
    ReactiveFormsModule,
    DxSelectBoxModule,
    DxButtonModule,
    DxDataGridModule,
    DxFormModule,
    DxTagBoxModule,
    DxTextBoxModule,
    DxTextAreaModule,
    DxDateBoxModule,
    DxValidatorModule],
  providers: [AuthGuardService],
  exports: [RouterModule],
  declarations: [
    HomeComponent,
    ProfileComponent,
    EmployeeComponent,
    UsuarioBusquedaComponent,
    UsuarioComponent,
    TasksComponent
  ]
})
export class AppRoutingModule { }
