import { ModuleWithProviders }  from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login.component';
//import { ForgotPasswordComponent } from './forgotpassword.component';
//import { SecurityQuestionsComponent } from './securityquestions.component';
const appRoutes: Routes = [
  {
    path: '',
    redirectTo: '/login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent
  },
  /*{
  path: 'forgotpassword',
  component: ForgotPasswordComponent
},
{
  path: 'setupquestions',
  component: SecurityQuestionsComponent
},*/

];

export const loginRouting: ModuleWithProviders = RouterModule.forChild(appRoutes);

