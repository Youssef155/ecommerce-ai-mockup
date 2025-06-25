import { Component } from '@angular/core';
import { RegisterComponent } from "../../pages/auth/register/register.component";
import { LoginComponent } from "../../pages/auth/login/login.component";
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-auth-layout',
  standalone: true,
  imports: [
    RouterModule, // ✅ Needed for AuthService
    CommonModule
  ],
  templateUrl: './auth-layout.component.html',
  styleUrls: ['./auth-layout.component.css'] // ✅ Corrected here
})
export class AuthLayoutComponent { }
