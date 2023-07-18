import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule, BrowserTransferStateModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SideNavInnerToolbarModule, SideNavOuterToolbarModule, SingleCardModule } from './layouts';
import { ChangePasswordFormModule, CreateAccountFormModule, FooterModule, LoginFormModule, ResetPasswordFormModule } from './shared/components';
import { AppInfoService, AuthService, ScreenService } from './shared/services';
import { UnauthenticatedContentModule } from './unauthenticated-content';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    SideNavOuterToolbarModule,
    SideNavInnerToolbarModule,
    SingleCardModule,
    FooterModule,
    ResetPasswordFormModule,
    CreateAccountFormModule,
    ChangePasswordFormModule,
    LoginFormModule,
    UnauthenticatedContentModule,
    AppRoutingModule,
    BrowserTransferStateModule,
    HttpClientModule
  ],
  providers: [
    AuthService,
    ScreenService,
    AppInfoService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
