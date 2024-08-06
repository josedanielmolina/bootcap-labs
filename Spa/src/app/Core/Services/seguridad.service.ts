import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';




@Injectable({
    providedIn: 'root'
})
export class SeguridadService {

    private readonly llaveToken = 'token';
    private readonly llaveExpiracion = 'token-expiracion';
    private readonly campoRol = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

    private url = 'https://localhost:44371/api/Autorizacion';


    constructor(private http: HttpClient) { }

    login(credenciales: any): Observable<any> {
        return this.http.post<any>(`${this.url}/login`, credenciales)
    }

    guardarToken(token: any) {
        localStorage.setItem(this.llaveToken, token.Autorizacion.Token);
        localStorage.setItem(this.llaveExpiracion, token.Autorizacion.Expiration.toString());
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
        localStorage.removeItem('modulos');
    }




}
