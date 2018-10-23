import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';
import { Ng2BootstrapModule } from 'ng2-bootstrap';

import { AppComponent }   from './app.component';
import { LoginModule } from './users/login/login.module';
import { MainMenuModule } from './users/home/main-menu.module';
import { LogoutComponent } from './users/logout/logout.component';

import { routing } from './app.routing';

@NgModule({
  imports:      [
     BrowserModule,
     Ng2BootstrapModule,
     FormsModule,
     HttpModule,
     LoginModule,
     MainMenuModule,
     routing 
     ],
  declarations: [
    AppComponent,
    LogoutComponent
    ],
  bootstrap: [AppComponent]
})

export class AppModule { }
