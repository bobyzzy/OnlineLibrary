import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../service/user.service';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public loginForm = this.formBuilder.group({
    email: ['', [Validators.email, Validators.required]],
    password: ['', Validators.required]
  })
  constructor(private formBuilder: FormBuilder, private userService: UserService, private router: Router, public auth: AuthService) { }

  ngOnInit(): void {
    this.auth.user$.subscribe(data => {
      let email = data.email;
      let password = "7798929aQ_";
      this.userService.login(email, password).subscribe(data => {
        if (data.success) {
          localStorage.setItem("token", data.token);
          localStorage.setItem("name", data.name);
          localStorage.setItem("role", data.role);

          if (data.role == "AppUser") {
            this.router.navigateByUrl('user-get-all-books');
          }
          else {
            this.router.navigateByUrl('librarian-get-all-orders');
          }
        }
      })
    });
  }


  onSubmit() {
    let email = this.loginForm.controls["email"].value;
    let password = this.loginForm.controls["password"].value;
    this.userService.login(email, password).subscribe(data => {
      console.log(data.name)
      if (data.success) {
        localStorage.setItem("token", data.token);
        localStorage.setItem("name", data.name);
        localStorage.setItem("role", data.role);
        if (data.role == "AppUser") {
          this.router.navigateByUrl('user-get-all-books');
        }
        else {
          this.router.navigateByUrl('librarian-get-all-orders');
        }
      }
    })
  }
}