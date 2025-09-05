import { Refractory } from './refractory.models';

// Точная копия UserDetails из старого клиента для совместимости
export interface UserDetails {
  Id: number;
  Login: string;
  Name: string;
  LastName: string;
  Position: string;
  UserAvatar: string;
  DateRegistration: Date;
  Email: string;
  Refractories: Refractory[];
  IsAdmin: boolean;
  IsModerator: boolean;
  IsBlocked: boolean;
}

// Модель для ответа от нового API (нужна адаптация)
export interface ProfileViewModel {
  id: number;
  login: string;
  name: string;
  lastName: string;
  position: string;
  userAvatar: string;
  email: string;
  refractories: any[];
  dateRegistration?: Date;
  isAdmin: boolean;
  isModerator: boolean;
  isBlocked: boolean;
}

// Для редактирования профиля (новый API)
export interface ChangeProfileViewModel {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  position?: string;
}

// Форма регистрации (совместимо со старым и новым API)
export interface RegisterModel {
  Login: string;
  Email: string;
  Name: string;
  LastName: string;
  Position: string;
  Password: string;
  ConfirmPassword: string;
}