import { Component } from '@angular/core';
import { Constants } from './Helper/constants';
import { AuthService } from '@auth0/auth0-angular';
import { LoginComponent } from './login/login.component';
import { Router } from '@angular/router';
import { UserService } from './service/user.service';


@Component({

  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})


export class AppComponent {
  title = 'webAuth';
  constructor(public auth: AuthService, private userService: UserService, private router: Router) { }
  onLogout() {
    localStorage.removeItem(Constants.USER_KEY);
    localStorage.removeItem("role");
    localStorage.removeItem("name");
    this.auth.logout();
  }

  get isUserLogin() {
    const user = localStorage.getItem(Constants.USER_KEY);
    const role = localStorage.getItem("role");
    return user && user.length > 0 && role == "AppUser";
  }

  get isLibrarianLogin() {
    const user = localStorage.getItem(Constants.USER_KEY);
    const role = localStorage.getItem("role");
    return user && user.length > 0 && role == "AppLibrarian";
  }

  loginWithRedirect(): void {
    this.auth.loginWithRedirect();
  }
}
