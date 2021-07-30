import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ImageUploadModel, UploadResultModel } from '../models/ImageUploadModel';
import { TestService } from '../services/test.service';

@Component({
  selector: 'app-image-upload',
  templateUrl: './image-upload.component.html',
  styleUrls: ['./image-upload.component.css']
})
export class ImageUploadComponent implements OnInit {

  public profileForm: FormGroup;
  imageModel: ImageUploadModel = new ImageUploadModel();
  uploadResult: Array<UploadResultModel> = [];
  fileToUpload: File = null;
  url: string = '';
  files: Array<any> = new Array<any>();
  submitted= false;

  constructor(private testService: TestService, private fb: FormBuilder) { }

  ngOnInit() {
    this.profileForm = this.fb.group({
      name: [null, Validators.compose([Validators.required])],
      nic: [null, Validators.compose([Validators.required])],
      nicCopy: [null, Validators.compose([Validators.required])],
      profilePic: [null, Validators.compose([Validators.required])]
    });
    this.getImages();
  }

  get f() { return this.profileForm.controls; }

  onSelectFile(files: FileList, id: string) {
    if (files.length === 0)
      return;
  
    this.fileToUpload = files.item(0);
  
  
    const fileReader: FileReader = new FileReader();
    fileReader.readAsDataURL(this.fileToUpload);
  
    fileReader.onload = (event: any) => {
      this.url = event.target.result;
    };

    var filesToPush = { data: this.fileToUpload, fileName: this.fileToUpload.name };
    this.files.push(filesToPush);
    var elements = document.getElementById(id);
    elements.innerHTML = this.fileToUpload.name;

    this.profileForm.get(id).setValue(filesToPush);
  }

  onSubmit(){
    this.submitted = true;

    if(this.profileForm.invalid){
      return;
    }
    this.imageModel = this.profileForm.value;

    this.testService.uploadImage(this.imageModel).subscribe(
      result => {
        console.log(result);
        this.profileForm.reset();
        this.submitted = false;
        this.resetFileInput();
        this.getImages();
      },
      error => {
        console.log(error);
      }
    );
  }

  resetFileInput(){
    var nic = document.getElementById("nicCopy");
    nic.innerHTML = "Choose file";
    
    var profile = document.getElementById("profilePic");
    profile.innerHTML = "Choose file";
  }

  getImages(){
    this.testService.getImages().subscribe(
      result => {
        this.uploadResult = result;
      },
      error => {
        console.log(error);
      }
    );
  }

}
