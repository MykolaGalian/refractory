import { Refractory } from '../refractory/refractory';
export class UserEdit {
  Id: number;
    Login: string;
    Name: string;
    LastName: string;
    Position: string;
    UserAvatar: string;
    DateRegistration: Date;
    Email: string;
    Refractories: Refractory[];
    IsAdmin: Boolean;
    IsModerator: Boolean;
    IsBlocked: Boolean;
}

