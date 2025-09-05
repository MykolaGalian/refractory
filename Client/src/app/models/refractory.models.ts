import { UserDetails } from './user.models';
import { Comment } from './comment.models';

// Точная копия модели Refractory из старого клиента
export interface Refractory {
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
  IsBlocked: boolean;
}

// Точная копия EditRefractory из старого клиента
export interface EditRefractory {
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
}

// Точная копия Refcalc из старого клиента  
export interface Refcalc {  
  a1: string;
  a2: string;
  b1: string;
  b2: string;
  BrickLength: string;
  TopDiameter: string; 
  BottomDiameter: string; 
  RowNumber: string;
  Density: string;
  Price: string;
}

// Точная копия RefcalcResult из старого клиента
export interface RefcalcResult {  
  TotalA: string;
  TotalB: string;
  TotalMass: string;
  TotalPrice: string;
}