import {Component, OnInit} from '@angular/core';
import {JwtService} from "./services/auth/jwt.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(private jwtService: JwtService) {
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
  }

  logout() {
    this.jwtService.logout();
  }
}
