import { ModuleWithProviders }  from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MainMenuComponent } from './main-menu.component';
import { SummaryComponent } from './summary/summary.component';
import { FundTransferComponent } from './fund-transfer/fund-transfer.component';
import { PaymentsComponent } from './payments/payments.component';
import { NotificationsComponent } from './notifications/notifications.component';
import {CreateUserComponent} from "./create-user/create-user.component";
import { ViewStatementsComponent } from './view-statements/view-statements.component';
import { CreditCardComponent } from './credit-card/credit-card.component';
import { ApproveTransactionsComponent } from './approve-transactions/approve-transactions.component';
import { DepositMoneyComponent } from './deposit-money/deposit-money.component';
import { UpdateProfileComponent } from './update-profile/update-profile.component';
import { AddAccountComponent } from './add-account/add-account.component';
import { AddEmployeeComponent } from './add-employee/add-employee.component';
import { AccountManagementComponent } from './account-management/account-management.component';
import { SystemLogComponent } from './system-log/system-log.component';

const mainMenuroutes: Routes = [
    {
        path:'mainmenu/:userId',
        component: MainMenuComponent,
        children: [
            {
                path:'',
                component:SummaryComponent
            },
             {
                path:'summary',
                component:SummaryComponent
            },
            {
                path:'fund-transfer',
                component:FundTransferComponent
            },
            {
                path:'payments',
                component:PaymentsComponent
            },
            {
                path:'notifications',
                component:NotificationsComponent
            },
            {
                path: 'create-user',
                component: CreateUserComponent
            },
            {
                path: 'view-statements',
                component: ViewStatementsComponent
            },
            {
                path: 'credit-card',
                component: CreditCardComponent
            },

            {
                path: 'deposit-money',
                component: DepositMoneyComponent
            },
            {
                path: 'update-profile',
                component: UpdateProfileComponent
            },
            {
                path: 'add-account',
                component: AddAccountComponent
            },
            {
                path: 'add-employee',
                component: AddEmployeeComponent
            },
            {
                path: 'account-management',
                component: AccountManagementComponent
            },
            {
                path: 'system-log',
                component: SystemLogComponent
            }
        ]
    }
];
export const mainMenuRouting: ModuleWithProviders = RouterModule.forChild(mainMenuroutes);

