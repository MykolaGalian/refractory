import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from "@angular/forms";
import { QuillModule } from 'ngx-quill';
import { RouterModule } from '@angular/router';
import { appRoutes } from './route';
import { AuthGuard } from './auth.guard';
import { ToastrModule } from 'ngx-toastr';

import { RefractoryService } from './shared/refractory.service';
import { UserService } from './shared/user.service';
import { ModeratorService } from './shared/moderator.service';
import { AdminService} from './shared/admin.service';
import { CommentService } from './shared/comment.service';


import { RefractoryAddComponent } from './components/ref-add/ref-add.component';
import { HomeComponent } from './components/home/home.component';
import { ProfileComponent } from './components/profile/profile.component';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ReadRefractoryComponent } from './components/home/read-ref/read-ref.component';
import { UserRefractoryComponent } from './components/profile/user-ref/user-ref.component';
import { AdminComponent } from './components/admin/admin.component';
import { UserListComponent } from './components/admin/user-list/user-list.component';
import { AdminCommandsComponent } from './components/admin/admin-commands/admin-commands.component';
import { ModeratorComponent } from './components/moderator/moderator.component';
import { RefractoryListComponent } from './components/moderator/ref-list/ref-list.component';
import { RefractoryBytegComponent } from './components/home/ref-byteg/ref-byteg.component';
import { EditProfileComponent } from './components/profile/edit-profile/edit-profile.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    LoginComponent,
    RegisterComponent,
    ProfileComponent,
    RefractoryAddComponent,
    HomeComponent,
    ReadRefractoryComponent,
    UserRefractoryComponent,
    AdminComponent,
    UserListComponent,
    AdminCommandsComponent,
    ModeratorComponent,
    RefractoryListComponent, RefractoryBytegComponent, EditProfileComponent

  ],
  exports: [RouterModule],
  imports: [
    BrowserModule,
    //AppRoutingModule,
    FormsModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    HttpClientModule,
    ToastrModule.forRoot(),
    QuillModule.forRoot(),
    RouterModule.forRoot(appRoutes),


  ],
  providers: [UserService,AuthGuard,RefractoryService,CommentService, AdminService, ModeratorService],
  bootstrap: [AppComponent]
})
export class AppModule { }
