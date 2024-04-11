import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {HttpClient, HttpHeaders} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private fb : FormBuilder, private _client : HttpClient) { }

  readonly _uri = "http://localhost:58345/api";

  formModel = this.fb.group({
    UserName : ['', Validators.required],
    Email: ['', [Validators.email,Validators.required]],
    FullName: [''],
    Passwords : this.fb.group({
      Password : ['', [Validators.required,Validators.minLength(4)]],
      ConfirmPassword :['', Validators.required]
    },{validators:this.ComparePassword })
  });

  ComparePassword(fb:FormGroup)
  {
    let confirmPwd = fb.get("ConfirmPassword");
    if(confirmPwd.errors == null  || 'passwordMismatch' in confirmPwd.errors)
    {
      if(fb.get("Password").value != confirmPwd.value)
      confirmPwd.setErrors({passwordMismatch: true});
      else
      confirmPwd.setErrors(null);
    }
  }

register()
{
  debugger;
  var std = {
    UserName : this.formModel.value.UserName,
    Email : this.formModel.value.Email,
    FullName : this.formModel.value.FullName,
    Password : this.formModel.value.Passwords.Password
  }

  return this._client.post(this._uri + "/ApplicationUser/Register" , std);
}

Login(formData)
{
  return this._client.post(this._uri + "/ApplicationUser/Login" , formData);
}

GetStdInfo()
{
  var header = new HttpHeaders({'Authorization' : 'Bearer ' + localStorage.getItem('token')})
  return this._client.get(this._uri + "/StudentBasicInfo",{headers:header});
}


}
