import { Component, inject } from '@angular/core';
import { MaterialModule } from '../../../Core/Common/material/material.module';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../Core/Services/auth.service';
import { MessageService } from '../../../Core/Mensajes/message.service';
import { Router } from '@angular/router';
import { LoadingComponent } from '../../../Core/Common/loading/loading.component';

@Component({
    selector: 'app-recuperar-contrasenna',
    standalone: true,
    imports: [
        MaterialModule,
        FormsModule,
        LoadingComponent
    ],
    templateUrl: './recuperar-contrasenna.component.html',
    styleUrl: './recuperar-contrasenna.component.css'
})
export class RecuperarContrasennaComponent {

    private readonly authService = inject(AuthService);
    private readonly router = inject(Router);

    loading: boolean = false;

    validacionCorreoExitosa: boolean = false;
    validacionCodigoExitosa: boolean = false;

    errorMessage: string = '';
    correo: string = '';
    codigoValdiacion: string = '';
    contrasennaNueva: string = '';
    contrasennaNuevaValidacion: string = '';

    onRecuperarContrasenna() {
        this.loading = true;
        this.authService.validarCorreo(this.correo)
            .subscribe(
                {
                    complete: () => {
                        this.validacionCorreoExitosa = true;
                        this.errorMessage = '';
                        this.loading = false;
                    },
                    error: (e) => {
                        this.errorMessage = e.error
                        this.loading = false;
                    }
                }
            )
    }

    onValidarCodigo() {
        this.loading = true;
        this.authService.validarCodigo(this.correo, this.codigoValdiacion)
            .subscribe(
                {
                    complete: () => {
                        this.validacionCodigoExitosa = true;
                        this.errorMessage = '';
                        this.loading = false;
                    },
                    error: (e) => {
                        this.errorMessage = e.error
                        this.loading = false;
                    }
                }
            )
    }

    onCambiarContrasenna() {
        this.loading = true;
        if (this.contrasennaNueva !== this.contrasennaNuevaValidacion) {
            this.errorMessage = 'Las contraseñas no coinciden';
            this.loading = false;
            return;
        }

        this.authService.changePassword(
            {
                correo: this.correo,
                contrasena: this.contrasennaNueva,
                codigo: this.codigoValdiacion
            }
        )
            .subscribe(
                {
                    next: (resp) => {
                        // logear y mandar al home
                        this.authService.guardarToken(resp);
                        this.router.navigate(['empleados']);
                        this.loading = false;
                    },
                    error: (e) => {
                        this.errorMessage = e.error
                        this.loading = false;
                    }
                }
            )
    }
}
