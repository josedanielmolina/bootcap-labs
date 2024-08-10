import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ChangePasswordDTO } from '../Models/ChangePassword.model';




@Injectable({
    providedIn: 'root'
})
export class AuthService {

    private readonly llaveToken = 'token';
    private readonly llaveExpiracion = 'token-expiracion';
    private readonly campoRol = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

    private url = 'https://localhost:7232/api/Auth';


    constructor(private http: HttpClient) { }

    login(credenciales: any): Observable<any> {
        return this.http.post<any>(`${this.url}/login`, credenciales)
    }

    validarCorreo(correo: string): Observable<any> {
        return this.http.get<any>(`${this.url}/validar-correo/${correo}`);
    }

    validarCodigo(correo: string, codigo: string): Observable<void> {
        return this.http.get<void>(`${this.url}/validar-codigo/${correo}/${codigo}`);
    }

    changePassword(changePasswordDTO: ChangePasswordDTO): Observable<void> {
        return this.http.post<void>(`${this.url}/cambiar-contrasena`, changePasswordDTO);
      }

    guardarToken(resp: any) {
        localStorage.setItem(this.llaveToken, resp.Token);
        localStorage.setItem(this.llaveExpiracion, resp.Expiration.toString());
    }

    obtenerCampoJWT(campo: string): string {
        const token = localStorage.getItem(this.llaveToken);
        if (!token) { return '0'; }
        var dataToken = JSON.parse(atob(token.split('.')[1]));
        return dataToken[campo];
    }

    obtenerRol(): string {
        return this.obtenerCampoJWT(this.campoRol);
    }

    obtenerToken() {
        return localStorage.getItem(this.llaveToken);
    }

    estaLogueado(): boolean {

        const token = localStorage.getItem(this.llaveToken);

        if (!token) {
            return false;
        }

        const expiracion = localStorage.getItem(this.llaveExpiracion);
        const expiracionFecha = new Date(expiracion!);

        if (expiracionFecha <= new Date()) {
            this.logout();
            return false;
        }

        return true;
    }

    logout() {
        localStorage.removeItem(this.llaveToken);
        localStorage.removeItem(this.llaveExpiracion);
    }




}
