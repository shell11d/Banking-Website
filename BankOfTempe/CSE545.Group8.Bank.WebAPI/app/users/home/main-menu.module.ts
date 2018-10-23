import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms'

import { Ng2BootstrapModule } from 'ng2-bootstrap';
import { MainMenuComponent } from './main-menu.component';
import { SummaryComponent } from './summary/summary.component';
import { FundTransferComponent } from './fund-transfer/fund-transfer.component';
import { PaymentsComponent } from './payments/payments.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { CreateUserComponent } from './create-user/create-user.component';
import { mainMenuRouting } from './mainmenu-routing.module';
import { UserService } from './shared/user.service';
import { AccountService } from './shared/account.service';
import { MerchantService } from './shared/merchant.service';
import { ViewStatementsComponent } from './view-statements/view-statements.component';
import { ViewStatementsService } from './view-statements/view-statements.service';
import { TransactionsService } from './shared/transactions.service';
import { TransactionBuilder } from './model/transaction-builder';
import { CreditCardComponent } from './credit-card/credit-card.component';
import { ApproveTransactionsComponent } from './approve-transactions/approve-transactions.component';
import { DepositMoneyComponent } from './deposit-money/deposit-money.component';
import { UpdateProfileComponent } from './update-profile/update-profile.component';
import { AddAccountComponent } from './add-account/add-account.component';
import { AddEmployeeComponent } from './add-employee/add-employee.component';
import { AccountManagementComponent } from './account-management/account-management.component';
import { SystemLogComponent } from './system-log/system-log.component';
import { Ng2Bs3ModalModule } from 'ng2-bs3-modal/ng2-bs3-modal';
import { UserBuilder } from './model/user-builder';


@NgModule({
  imports:      [
     BrowserModule,
     Ng2BootstrapModule,
     HttpModule,
     mainMenuRouting,
      FormsModule,
      Ng2Bs3ModalModule 
     ],
  declarations: [ 
    MainMenuComponent,
    SummaryComponent,
    FundTransferComponent,
    PaymentsComponent,
    NotificationsComponent,
    CreateUserComponent,
    ViewStatementsComponent,
    CreditCardComponent,
    ApproveTransactionsComponent,
    DepositMoneyComponent,
    UpdateProfileComponent,
    AddAccountComponent,
    AddEmployeeComponent,
    AccountManagementComponent,
    SystemLogComponent
    ],
  providers: [
    UserService,
    AccountService,
    MerchantService,
    ViewStatementsService,
    TransactionsService,
      TransactionBuilder,
    UserBuilder
    ]
  })

export class MainMenuModule { }
