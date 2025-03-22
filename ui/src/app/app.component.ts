import {Component, OnInit} from '@angular/core';
import {JwtService} from "./services/auth/jwt.service";
import {NavigationEnd, Router} from "@angular/router";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(private jwtService: JwtService, private router: Router) {
  }

  get isAuthenticated() {
    return this.jwtService.isAuthenticated();
  }

  get fullName() {
    return this.jwtService.getFullName();
  }

  get role() {
    return this.jwtService.getRole();
  }

  ngOnInit(): void {
    if (!this.jwtService.isAuthenticated()) {
      this.jwtService.logout();
    }

    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        if (event.url.startsWith('/internal')) {
          document.body.classList.add('internal-area');
        } else {
          document.body.classList.remove('internal-area');
        }
      }
    });
  }

  logout() {
    this.jwtService.logout();
  }
}
