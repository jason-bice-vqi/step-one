import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from "./login/login.component";
import {UserDashboardComponent} from "./user-dashboard/user-dashboard.component";
import {authGuard} from "./guards/auth.guard";
import {AdminDashboardComponent} from "./admin/admin-dashboard/admin-dashboard.component";
import {internalUserAuthGuard} from "./admin/guards/internal-user-auth.guard";

const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: '', redirectTo: '/login', pathMatch: 'full'},
  {path: 'dashboard', component: UserDashboardComponent, canActivate: [authGuard]},
  {
    path: 'admin',
    canActivate: [internalUserAuthGuard],
    children: [
      { path: 'dashboard', component: AdminDashboardComponent }
    ]
  },
  {path: 'intermediate', redirectTo: '/admin-user-dashboard'},
  // Must be last
  {path: '**', redirectTo: '/login'}, // Wildcard route for 404 handling
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
