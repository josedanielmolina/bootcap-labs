import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MessageService } from '../../../Core/Mensajes/message.service';
import { AuthService } from '../../../Core/Services/auth.service';
import { Router, RouterModule } from '@angular/router';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [
        ReactiveFormsModule,
        CommonModule,
        RouterModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatSelectModule
    ],
    templateUrl: './login.component.html',
    styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

    private readonly fb = inject(FormBuilder);
    private readonly messageService = inject(MessageService);
    private readonly seguridadService = inject(AuthService);
    private readonly router = inject(Router);



    ngOnInit(): void {
        this.initForm();
    }


    formulario!: FormGroup;

    initForm() {
        this.formulario = this.fb.group({
            correo: ['', [Validators.required]],
            contrasena: ['', Validators.required]
        });
    }

    onSubmit() {
        if (this.formulario.valid) {

            this.seguridadService.login(this.formulario.value)
                .subscribe(
                    {
                        next: (resp) => {
                            this.seguridadService.guardarToken(resp);
                            this.router.navigate(['empleados']);
                        },
                        error: (e) => this.messageService.showErrorMessage("Usuario o contraseña incorrectos")
                    }
                );

        } else {

        }
    }

}
