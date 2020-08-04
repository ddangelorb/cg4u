import { BrowserModule, Title } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { rootRouterConfig } from './app.routes';
import { MyDatePickerModule } from 'mydatepicker';
import { ChartsModule } from 'ng2-charts';

//bootstrap
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { CarouselModule } from 'ngx-bootstrap/carousel';

//services
import { SeoService } from './services/seo.services';

//shared components
import { FooterComponent } from './shared/footer/footer.component';
import { MainComponent } from './shared/main/main.component';
import { MenuComponent } from './shared/menu/menu.component';
import { LoginComponent } from './shared/login/login.component';

//components
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { DoSigninComponent } from './signin/do-signin/do-signin.component';
import { RegisterSigninComponent } from './signin/register-signin/register-signin.component';
import { DashboardComponent } from './dashboard/dashboard.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    FooterComponent,
    MainComponent,
    MenuComponent,
    LoginComponent,
    DoSigninComponent,
    RegisterSigninComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule,
    CollapseModule.forRoot(),
    CarouselModule.forRoot(),
    RouterModule.forRoot(rootRouterConfig, { useHash: false}),
    MyDatePickerModule,
    ChartsModule
  ],
  providers: [
    Title,
    SeoService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
