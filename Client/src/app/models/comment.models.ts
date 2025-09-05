import { UserDetails } from './user.models';

export interface Comment {
    Id: number;
    CommentBody: string;
    RefId: number;
    DateCreation: Date;
    LastEdit: Date;
    UserInfoId: number;
    UserInfo: UserDetails;
}