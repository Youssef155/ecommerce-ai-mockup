import { Component, OnInit } from '@angular/core';
import { Router, RouterModule, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-main-navbar',
  imports: [CommonModule, RouterModule],
  templateUrl: './main-navbar.component.html',
  styleUrl: './main-navbar.component.css'
})
export class MainNavbarComponent {
  showNavBar = true;

  constructor(private router: Router) {}

  ngOnInit() {
    // Initial check
    this.updateNavbarVisibility();

    // Subscribe to route changes
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.updateNavbarVisibility();
      });
  }

  private updateNavbarVisibility(): void {
    let currentRoute = this.router.routerState.snapshot.root;
    
    // Traverse through all child routes
    while (currentRoute.firstChild) {
      currentRoute = currentRoute.firstChild;
    }

    // Check if hideFooter is set to true in any route data
    this.showNavBar = !currentRoute.data?.['hideNavbar'];
  }

}

