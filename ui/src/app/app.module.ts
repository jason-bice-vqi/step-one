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
import {UserDashboardComponent} from './user-dashboard/user-dashboard.component';
import {AuthInterceptor} from "./interceptors/auth.interceptor";
import {FormatPhonePipe} from './pipes/format-phone.pipe';
import {MSAL_INSTANCE, MsalBroadcastService, MsalGuard, MsalService} from "@azure/msal-angular";
import {BrowserCacheLocation, LogLevel, PublicClientApplication} from "@azure/msal-browser";
import {environment} from "../environments/environment";
import {AdminDashboardComponent} from "./admin/admin-dashboard/admin-dashboard.component";
import { ProgressBarComponent } from './progress-bar/progress-bar.component';

export function MSALInstanceFactory(): PublicClientApplication {
  return new PublicClientApplication({
    auth: {
      clientId: environment.clientId,
      authority: environment.authority,
      redirectUri: environment.adLoginRedirectUri,
      navigateToLoginRequestUrl: false,
    },
    cache: {
      cacheLocation: BrowserCacheLocation.LocalStorage,
      storeAuthStateInCookie: false
    },
    system: {
      loggerOptions: {
        loggerCallback: (level, message, containsPii) => {
          if (containsPii) {
            return;
          }

          switch (level) {
            case LogLevel.Error:
              console.error(message);
              return;

            case LogLevel.Warning:
              console.warn(message);
              return;
          }
        }
      }
    }
  });
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    UserDashboardComponent,
    FormatPhonePipe,
    AdminDashboardComponent,
    ProgressBarComponent
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
    HttpClientModule
  ],
  providers: [
    {provide: MSAL_INSTANCE, useFactory: MSALInstanceFactory},
    MsalService,
    MsalGuard,
    MsalBroadcastService,
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
    OtpAuthService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
