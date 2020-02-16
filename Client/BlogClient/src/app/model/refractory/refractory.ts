import { UserDetails } from '../user/user-details';
export class Refractory {
  Id: number;
  RefractoryDescription: string;
  RefractoryBrand: string;
  RefractoryPicture: string;
  UserInfoId: number;
  UserInfo: UserDetails;
  DateCreate: Date;
  LastEdit: Date;
  Comments: Comment[];
  RefractoryType: string;
  Src: string;   // путь к картинке огнеупора
  IsBlocked: Boolean;
}
