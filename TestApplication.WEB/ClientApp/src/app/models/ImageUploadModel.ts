export interface FileParameter {
    data: any;
    fileName: string;
  }
  
  export class ImageUploadModel{
    name: string
    nic: string
    nicCopy: FileParameter
    profilePic: FileParameter
  }

  export class UploadResultModel{
    url: string
  }