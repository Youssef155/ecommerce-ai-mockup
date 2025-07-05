import { Routes } from '@angular/router';
import { DesignListComponent } from './design-list/design-list.component';
import { DesignUploadComponent } from './design-upload/design-upload.component';
import { DesignGenerateComponent } from './design-generate/design-generate.component';

export const DESIGN_ROUTES: Routes = [
  { path: '', component: DesignListComponent },
  { path: 'upload', component: DesignUploadComponent },
  { path: 'generate', component: DesignGenerateComponent },
  { path: '**', component: DesignListComponent },

];