import { Component, OnInit } from '@angular/core';
import { Router, RouterModule, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './footer.Component.html',
  styleUrl: './footer.Component.css'
})

export class FooterComponent implements OnInit {
  showFooter = true;

  constructor(private router: Router) {}

  ngOnInit() {
    // Initial check
    this.updateFooterVisibility();

    // Subscribe to route changes
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.updateFooterVisibility();
      });
  }

  private updateFooterVisibility(): void {
    let currentRoute = this.router.routerState.snapshot.root;
    
    // Traverse through all child routes
    while (currentRoute.firstChild) {
      currentRoute = currentRoute.firstChild;
    }

    // Check if hideFooter is set to true in any route data
    this.showFooter = !currentRoute.data?.['hideFooter'];
  }
}
