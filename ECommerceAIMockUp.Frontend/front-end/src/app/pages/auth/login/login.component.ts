import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { LoginDto } from '../../../core/models/LoginDto';
import { JwtUserData } from '../../../core/models/TokenData';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  LoginForm: FormGroup = new FormGroup({});
  submitted = false;
  errorMessages: string[] = [];

  constructor(
    private authService: AuthService,
    private formbuilder: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.LoginForm = this.formbuilder.group({
      Email: ['', [Validators.required, Validators.email]],
      Password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  login() {
   this.authService.Login(this.LoginForm.value).subscribe((result : any) => {
    console.log('Form', result);
    localStorage.setItem('angular19Token', result.data.token);
    console.log("the token",result.data.token);
    this.router.navigateByUrl('/products');
   })
  }
}
