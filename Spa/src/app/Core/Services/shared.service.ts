import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  private apiUrl = 'https://localhost:7232/api';

  constructor(private http: HttpClient) { }


  createEmpleado(empleado: any) {
    return this.http.post<any>(`${this.apiUrl}/Domain/CreateEmpleadoUseCase`, empleado);
  }

}
