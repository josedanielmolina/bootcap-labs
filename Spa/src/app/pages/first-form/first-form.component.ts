import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SharedService } from '../../Core/Services/shared.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
    selector: 'app-first-form',
    standalone: true,
    imports: [
        ReactiveFormsModule,
        CommonModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule
    ],
    providers: [SharedService],
    templateUrl: './first-form.component.html',
    styleUrl: './first-form.component.css'
})
export class FirstFormComponent implements OnInit {

    formulario!: FormGroup;
    mensajeResp = '';

    constructor(
        private fb: FormBuilder,
        private _sharedService: SharedService
    ) { }

    ngOnInit(): void {
        this.initForm();
    }

    initForm() {
        this.formulario = this.fb.group({
            nombres: ['jose', Validators.required],
            correo: ['jdmolina@gmail.com', [Validators.required, Validators.email]],
            cargo: ['dictador', Validators.required],
            codigoRH: ['1234', Validators.required]
        });
    }

    onSubmit() {
        if (this.formulario.valid) {
            console.log(this.formulario.value);

            this._sharedService.createEmpleado(this.formulario.value)
                .subscribe(resp => {
                    console.log(resp);
                    this.mensajeResp = resp.mensaje;
                });
        } else {
            console.log('Formulario no válido');
        }
    }
}
