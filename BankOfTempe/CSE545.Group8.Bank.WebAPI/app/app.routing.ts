import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LogoutComponent } from './users/logout/logout.component';

const appRoutes: Routes = [
{
	path:'logout',
	component: LogoutComponent
}
    
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes)