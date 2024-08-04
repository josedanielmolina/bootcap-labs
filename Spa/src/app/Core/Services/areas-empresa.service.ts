import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Empleado } from '../Models/Empleado.interface';
import { Observable } from 'rxjs';
import { Areasempresa } from '../Models/Areasempresa.interface';

@Injectable({
    providedIn: 'root'
})
export class AreasEmpresaService {

    private apiUrl = 'https://localhost:7232/api';

    constructor(private http: HttpClient) { }


    getAreasEmpresa(): Observable<Areasempresa[]> {
        return this.http.get<Areasempresa[]>(`${this.apiUrl}/AreasEmpresa/GetAreasEmpresa`);
    }

}
