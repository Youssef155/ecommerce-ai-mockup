import { Component } from '@angular/core';
import { FooterComponent } from '../app/shared/components/footer/footer.component';
import { MainNavbarComponent } from "./shared/components/navbar/main-navbar/main-navbar.component";
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, FooterComponent, MainNavbarComponent, CommonModule, RouterModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'EcommerceAI';
  
}

