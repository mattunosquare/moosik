export interface IMakeARecommendationForm {
  Title: string,
  PostDescription: string,
  PostResourceTitle: string
}

export interface IMakeARecommendationPayload {
  Title: string,
  PostDescription: string,
  ThreadTypeId: number,
  UserId: number,
  PostResourceTitle: string,
  PostResourceValue: string,
  ResourceTypeId: number
}

export interface IReplyToThreadForm{
  ThreadId: number,
  Description: string,
  PostResourceTitle: string,
  PostResourceValue: string,
  PostResourceTypeId: number
}

export interface IReplyToThreadPayload{
  ThreadId: number,
  UserId: number,
  Description: string,
  PostResourceTitle: string,
  PostResourceValue: string,
  ResourceTypeId: number  
}

export interface IUserDetails{
  id: number;
  username: string
}  

export interface IPostResource{
  id: number,
  title: string,
  value: string,
  resourceType: IPostResourceType
}

export interface IPostResourceType{
  id: number,
  description: string
}

export interface IPostDetails {
  id: number;
  description: string;
  user: IUserDetails;
  createdDate: string;
  postResources: IPostResource[]
}

export interface IThreadTypeDetails {
  id: number;
  description: string;
}


export interface IThreadDetails {
  id: number;
  title: string;
  posts: IPostDetails[];
  user: IUserDetails;
  threadType: IThreadTypeDetails;
  createdDate: string;
}
  