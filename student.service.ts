import { Injectable } from '@angular/core';
import { StudentDetails } from './StudentDetails';
import {HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  formData: StudentDetails= {
    stdId :0,
    studentName: null,
    studentAddress: null,
    dob: null,
    gender: null
  };

  list : StudentDetails[];
  readonly _uri = "http://localhost:58345/api";

  constructor(private _client : HttpClient) { }
  
  PostStudentDetails(){
    var header = new HttpHeaders({'Authorization' : 'Bearer ' + localStorage.getItem('token')})
    return this._client.post(this._uri + '/StudentDetails', this.formData,{headers:header});
  }

  PutStudentDetails(){
    var header = new HttpHeaders({'Authorization' : 'Bearer ' + localStorage.getItem('token')})
    return this._client.put(this._uri + '/StudentDetails/' + this.formData.stdId, this.formData,{headers:header});
  }

  DeleteStudentDetails(id){
    var header = new HttpHeaders({'Authorization' : 'Bearer ' + localStorage.getItem('token')})
    return this._client.delete(this._uri + '/StudentDetails/' + id,{headers:header});
  }

  readAll() : Observable<StudentDetails[]>{
    var header = new HttpHeaders({'Authorization' : 'Bearer ' + localStorage.getItem('token')})
    return this._client.get<StudentDetails[]>(this._uri+'StudentDetails',{headers:header})
  }

  refreshList1(){
    var header = new HttpHeaders({'Authorization' : 'Bearer ' + localStorage.getItem('token')})
    return this._client.get(this._uri + "/StudentDetails",{headers:header});
  }

  
  refreshList(){
    var header = new HttpHeaders({'Authorization' : 'Bearer ' + localStorage.getItem('token')})
    this._client.get(this._uri + '/StudentDetails',{headers:header})
    .toPromise()
    .then(res => this.list = res as StudentDetails[]);
 
  }


}
