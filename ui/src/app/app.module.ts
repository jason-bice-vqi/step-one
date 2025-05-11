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
import {InternalDashboardComponent} from "./internal/internal-dashboard/internal-dashboard.component";
import {ProgressBarComponent} from './progress-bar/progress-bar.component';
import {MatTableModule} from "@angular/material/table";
import {MatProgressBarModule} from "@angular/material/progress-bar";
import {MatIconModule} from "@angular/material/icon";
import {MatPaginatorModule} from "@angular/material/paginator";
import {MatTooltipModule} from "@angular/material/tooltip";
import {MatSelectModule} from "@angular/material/select";
import {FileUploadComponent} from './file-upload/file-upload.component';
import {CandidateWorkflowStepComponent} from './candidate-workflow-step/candidate-workflow-step.component';
import {MatCheckboxModule} from "@angular/material/checkbox";
import {MatSortModule} from '@angular/material/sort';
import {MatMenuModule} from "@angular/material/menu";
import {NavBarComponent} from './internal/nav-bar/nav-bar.component';
import {MatToolbarModule} from "@angular/material/toolbar";
import {WorkflowsComponent} from './internal/workflows/workflows.component';
import {
  WorkflowStepConfigDialogComponent
} from './internal/workflow-step-config-dialog/workflow-step-config-dialog.component';
import {MAT_DIALOG_DEFAULT_OPTIONS, MatDialogModule} from "@angular/material/dialog";
import {MatSlideToggleModule} from "@angular/material/slide-toggle";
import { ConfirmDeleteDialogComponent } from './confirm-delete-dialog/confirm-delete-dialog.component';

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
    InternalDashboardComponent,
    ProgressBarComponent,
    FileUploadComponent,
    CandidateWorkflowStepComponent,
    NavBarComponent,
    WorkflowsComponent,
    WorkflowStepConfigDialogComponent,
    ConfirmDeleteDialogComponent
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
    MatTableModule,
    MatProgressBarModule,
    MatIconModule,
    MatPaginatorModule,
    MatTooltipModule,
    MatSelectModule,
    MatCheckboxModule,
    MatSortModule,
    MatMenuModule,
    MatToolbarModule,
    MatDialogModule,
    MatSlideToggleModule
  ],
  providers: [
    {provide: MSAL_INSTANCE, useFactory: MSALInstanceFactory},
    MsalService,
    MsalGuard,
    MsalBroadcastService,
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
    OtpAuthService,
    {
      provide: MAT_DIALOG_DEFAULT_OPTIONS,
      useValue: {
        hasBackdrop: true,
        disableClose: false,
        autoFocus: true,
        maxWidth: '600px',
        position: { top: '20vh' }
      }
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
