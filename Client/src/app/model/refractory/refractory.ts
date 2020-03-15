import { UserDetails } from '../user/user-details';
export class Refractory {
  Id: number;
  RefractoryDescription: string;
  RefractoryBrand: string;
  RefractoryType: string;

  Density: string; 
  MaxWorkTemperature: string; 
  Lime: string; 
  Alumina: string; 
  Silica: string; 
  Magnesia: string; 
  Carbon: string; 
  Price: string; 

  RefractoryPicture: string;
  UserInfoId: number;
  UserInfo: UserDetails;
  DateCreate: Date;
  LastEdit: Date;
  Comments: Comment[];

  Src: string;   // путь к картинке огнеупора
  IsBlocked: Boolean;
}
