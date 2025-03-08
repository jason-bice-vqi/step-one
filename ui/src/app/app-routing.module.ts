import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from "./login/login.component";
import {DashboardComponent} from "./dashboard/dashboard.component";
import {authGuard} from "./guards/auth.guard";

const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: '', redirectTo: '/login', pathMatch: 'full'}, // Redirect root to login
  {path: 'dashboard', component: DashboardComponent, canActivate: [authGuard]},
  {path: 'intermediate', redirectTo: '/dashboard'},
  // Must be last
  {path: '**', redirectTo: '/login'}, // Wildcard route for 404 handling
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
