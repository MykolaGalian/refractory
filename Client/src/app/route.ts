import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ProfileComponent } from './components/profile/profile.component';
import { AuthGuard } from './auth.guard';
import { RefractoryAddComponent } from './components/ref-add/ref-add.component';
import { HomeComponent } from './components/home/home.component';
import { ReadRefractoryComponent } from './components/home/read-ref/read-ref.component';
import  {UserRefractoryComponent}  from './components/profile/user-ref/user-ref.component';
import { AdminComponent } from './components/admin/admin.component';
import {ModeratorComponent} from './components/moderator/moderator.component';
import { RefractoryBytegComponent } from './components/home/ref-byteg/ref-byteg.component';
import { EditProfileComponent } from './components/profile/edit-profile/edit-profile.component';
import { RefCalcComponent } from './components/home/ref-calc/ref-calc.component';




export const appRoutes: Routes = [

    {path:'',redirectTo:'/login',pathMatch:'full'},   
    {path: 'home', component: HomeComponent, canActivate:[AuthGuard] }, 
    {path: 'read-ref', component: ReadRefractoryComponent, canActivate:[AuthGuard] }, 
    {path: 'registration', component: RegisterComponent, canActivate:[AuthGuard] }, 
    {path: 'login', component: LoginComponent},
    {path: 'profile', component: ProfileComponent, canActivate:[AuthGuard]},
    {path: 'ref-add', component: RefractoryAddComponent, canActivate:[AuthGuard]}, 
    {path: 'user-ref', component: UserRefractoryComponent, canActivate:[AuthGuard]},  //'user-ref/:login/:id'
    {path: 'admin', component: AdminComponent, canActivate:[AuthGuard]},  
    {path: 'moder', component: ModeratorComponent, canActivate:[AuthGuard]},
    {path: 'ref-byteg', component: RefractoryBytegComponent}, 
    {path: 'profile-edit', component: EditProfileComponent, canActivate:[AuthGuard]},
    {path: 'ref-calc', component: RefCalcComponent, canActivate:[AuthGuard]},
    
    {path: '**', redirectTo: '/login'}
    

];
