import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Empleado } from '../Models/Empleado.model';

@Injectable({
    providedIn: 'root'
})
export class EmpleadoService {

    private apiUrl = 'https://localhost:7232/api';

    constructor(private http: HttpClient) { }


    createEmpleado(empleado: any) {
        return this.http.post<any>(`${this.apiUrl}/Empleados/CreateEmpleado`, empleado);
    }

    actualizarEmpleado(empleado: any) {
        return this.http.post<any>(`${this.apiUrl}/Empleados/ActualizarEmpleado`, empleado);
    }

    getEmpleados(): Observable<Empleado[]> {
        return this.http.get<Empleado[]>(`${this.apiUrl}/Empleados/GetEmpleados`);
    }

    getEmpleadoById(codigoRH: string): Observable<Empleado> {
        return this.http.get<Empleado>(`${this.apiUrl}/Empleados/GetEmpleadoByCodigoRH/${codigoRH}`);
    }


    deleteEmpleado(empleadoId: number) {
        return this.http.delete(`${this.apiUrl}/Empleados/DeleteEmpleado/${empleadoId}`);
    }
}
