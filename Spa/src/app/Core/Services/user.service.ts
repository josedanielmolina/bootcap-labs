// user.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UsuarioDTO, UsuarioCreateDTO, UsuarioUpdateDTO } from '../Models/user.model';

@Injectable({
    providedIn: 'root'
})
export class UserService {
    private apiUrl = 'https://localhost:7232/api/usuarios';

    constructor(private http: HttpClient) { }

    // Obtener todos los usuarios
    getUsuarios(): Observable<UsuarioDTO[]> {
        return this.http.get<UsuarioDTO[]>(this.apiUrl);
    }

    // Obtener un usuario por ID
    getUsuario(id: number): Observable<UsuarioDTO> {
        return this.http.get<UsuarioDTO>(`${this.apiUrl}/${id}`);
    }

    // Crear un nuevo usuario
    createUsuario(usuario: UsuarioCreateDTO): Observable<UsuarioDTO> {
        return this.http.post<UsuarioDTO>(this.apiUrl, usuario);
    }

    // Actualizar un usuario existente
    updateUsuario(id: number, usuario: UsuarioUpdateDTO): Observable<void> {
        return this.http.put<void>(`${this.apiUrl}/${id}`, usuario);
    }

    // Eliminar un usuario
    deleteUsuario(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
