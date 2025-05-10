import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from "./login/login.component";
import {UserDashboardComponent} from "./user-dashboard/user-dashboard.component";
import {authGuard} from "./guards/auth.guard";
import {InternalDashboardComponent} from "./internal/internal-dashboard/internal-dashboard.component";
import {internalUserAuthGuard} from "./internal/guards/internal-user-auth.guard";
import {WorkflowsComponent} from "./internal/workflows/workflows.component";

const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: '', redirectTo: '/login', pathMatch: 'full'},
  {path: 'dashboard', component: UserDashboardComponent, canActivate: [authGuard]},
  {
    path: 'internal',
    canActivate: [internalUserAuthGuard],
    children: [
      { path: 'dashboard', component: InternalDashboardComponent },
      { path: 'workflows', component: WorkflowsComponent }
    ]
  },
  {path: 'intermediate', redirectTo: '/internal-user-dashboard'},
  // Must be last
  {path: '**', redirectTo: '/login'}, // Wildcard route for 404 handling
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
