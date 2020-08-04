import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DoSigninComponent } from './signin/do-signin/do-signin.component';
import { DashboardComponent } from './dashboard/dashboard.component';
//import { ListTteste1Component } from './tteste1/list-tteste1/list-tteste1.component';
//import { AddTteste1Component } from './tteste1/add-tteste1/add-tteste1.component';
//import { RemoveTteste1Component } from './tteste1/remove-tteste1/remove-tteste1.component';

export const rootRouterConfig: Routes = [
    {path: '', redirectTo: 'home', pathMatch: 'full' },
    {path: 'home', component: HomeComponent },
    {path: 'do-signin', component: DoSigninComponent } ,
    {path: 'dashboard', component: DashboardComponent } //,
    //{path: 'add-tteste1', component: AddTteste1Component },
    //{path: 'remove-tteste1', component: RemoveTteste1Component }
]