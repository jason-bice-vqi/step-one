import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {LoginComponent} from './login/login.component';
import {MatCardModule} from "@angular/material/card";
import {MatInputModule} from "@angular/material/input";
import {MatListModule} from "@angular/material/list";
import {MatButtonModule} from "@angular/material/button";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {NgOptimizedImage} from "@angular/common";
import {OtpAuthService} from "./services/auth/otp-auth.service";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {DashboardComponent} from './dashboard/dashboard.component';
import {AuthInterceptor} from "./interceptors/auth.interceptor";
import {FormatPhonePipe} from './pipes/format-phone.pipe';
import {MsalBroadcastService, MsalGuard, MsalModule, MsalService} from "@azure/msal-angular";
import {InteractionType, PublicClientApplication} from "@azure/msal-browser";

const msalConfig = {
  auth: {
    clientId: '80476fb6-af06-435b-b9c4-08f41a3cf912',
    authority: 'https://login.microsoftonline.com/e1515776-9ec0-40ed-af87-8fea22e50ded',
    redirectUri: 'http://localhost:4200/intermediate',
  },
  cache: {
    cacheLocation: 'localStorage',
    storeAuthStateInCookie: true,
  },
};

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    DashboardComponent,
    FormatPhonePipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatCardModule,
    MatInputModule,
    MatListModule,
    MatButtonModule,
    ReactiveFormsModule,
    NgOptimizedImage,
    FormsModule,
    HttpClientModule,
    MsalModule.forRoot(
      new PublicClientApplication(msalConfig),
      {
        interactionType: InteractionType.Redirect, // or InteractionType.Popup
        authRequest: {
          scopes: ['user.read'], // Default scope for Microsoft Graph API
        },
      },
      {
        interactionType: InteractionType.Redirect, // or InteractionType.Popup
        protectedResourceMap: new Map([
          ['http://localhost:5063/auth/ad/*', ['user.read']], // Protect API calls
        ]),
      }
    ),
  ],
  providers: [
    MsalService, MsalGuard, MsalBroadcastService,
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
    OtpAuthService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
