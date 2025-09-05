import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { ReadRefractoryComponent } from './components/home/read-ref/read-ref.component';
import { RefractoryBytegComponent } from './components/home/ref-byteg/ref-byteg.component';
import { RefCalcComponent } from './components/home/ref-calc/ref-calc.component';
import { ProfileComponent } from './components/profile/profile.component';
import { EditProfileComponent } from './components/profile/edit-profile/edit-profile.component';
import { UserRefractoryComponent } from './components/profile/user-ref/user-ref.component';
import { RefractoryAddComponent } from './components/ref-add/ref-add.component';
import { AdminComponent } from './components/admin/admin.component';
import { ModeratorComponent } from './components/moderator/moderator.component';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'home', component: HomeComponent, canActivate: [authGuard] },
  { path: 'read-ref', component: ReadRefractoryComponent, canActivate: [authGuard] },
  { path: 'ref-byteg', component: RefractoryBytegComponent, canActivate: [authGuard] },
  { path: 'ref-calc', component: RefCalcComponent, canActivate: [authGuard] },
  { path: 'profile', component: ProfileComponent, canActivate: [authGuard] },
  { path: 'profile-edit', component: EditProfileComponent, canActivate: [authGuard] },
  { path: 'user-ref', component: UserRefractoryComponent, canActivate: [authGuard] },
  { path: 'ref-add', component: RefractoryAddComponent, canActivate: [authGuard] },
  { path: 'admin', component: AdminComponent, canActivate: [authGuard] },
  { path: 'moder', component: ModeratorComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: '/login' }
];
