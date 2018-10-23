import { NgModule }       from '@angular/core';
import { CommonModule }   from '@angular/common';
import { FormsModule }    from '@angular/forms';

import { LoginComponent }    from './login.component';
import { Ng2Bs3ModalModule } from 'ng2-bs3-modal/ng2-bs3-modal';

//import { AngularVirtualKeyboard } from 'ng-virtual-keyboard';
//import { ForgotPasswordComponent }  from './forgotpassword.component';
//import { SecurityQuestionsComponent } from './securityquestions.component';

import { LoginService } from './login.service';

import { loginRouting } from './login-routing.module';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    loginRouting,
    Ng2Bs3ModalModule 
  ],
  declarations: [
    LoginComponent,
    //ForgotPasswordComponent,
   // SecurityQuestionsComponent
  ],
  providers: [
      LoginService
  ]
})
export class LoginModule {}