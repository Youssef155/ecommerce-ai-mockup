import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'] // ✅ Fixed property name
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
      Email: ['', Validators.required],
      Password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  login() {
    this.submitted = true;
    this.errorMessages = [];

    if (this.LoginForm.valid) {
      this.authService.login(this.LoginForm.value).subscribe({
        next: (response) => {
          if (response?.token) {
            console.log('✅ Token received:', response.token);
            this.router.navigate(['/products']);
          } else {
            this.errorMessages.push('Login failed: invalid response');
          }
        },
        error: (error) => {
          console.log('❌ Login error:', error);
          this.errorMessages.push('Login failed: server error');
        }
      });

      console.log('📤 Login form submitted:', this.LoginForm.value);
    }
  }
}
