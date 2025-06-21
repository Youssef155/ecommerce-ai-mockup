import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common'; // Optional if you use ngIf or similar
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule], // ✅ Required for formGroup to work
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup = new FormGroup({});
  submitted = false;
  errorMessages : string[] = [];
  constructor(
    private authService: AuthService,
    private formbuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = this.formbuilder.group({
      FirstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
      LastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
      Email: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$')]],
      Password: ['', [Validators.required, Validators.minLength(6)]],
      City: ['', [Validators.maxLength(15)]],
      Address: [''],
      PhoneNumber:['',Validators.required]
    });
  }

  register() {

    this.submitted = true;
    this.errorMessages = [];
   //if (this.registerForm.valid) {
      this.authService.register(this.registerForm.value).subscribe({
        next: (response) =>{
          console.log(response);
        },
        error: error=>{
          console.log(error);
        }
      })
      console.log('Register form submitted:', this.registerForm.value);
    //}
  }
}
