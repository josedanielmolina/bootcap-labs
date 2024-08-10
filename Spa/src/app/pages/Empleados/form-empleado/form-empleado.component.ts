import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { EmpleadoService } from '../../../Core/Services/empleado.service';
import { MessageService } from '../../../Core/Mensajes/message.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AreasEmpresaService } from '../../../Core/Services/areas-empresa.service';
import { MatSelectModule } from '@angular/material/select';
import { Areasempresa } from '../../../Core/Models/Areasempresa.model';

@Component({
    selector: 'app-form-empleado',
    standalone: true,
    imports: [
        ReactiveFormsModule,
        CommonModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatSelectModule
    ],
    templateUrl: './form-empleado.component.html',
    styleUrl: './form-empleado.component.css'
})
export class FormEmpleadoComponent {

    isCreateMode = true;
    formulario!: FormGroup;
    mensajeResp = '';

    areasEmpresa: Areasempresa[] = [];

    constructor(
        private fb: FormBuilder,
        private _empleadoService: EmpleadoService,
        private _areasEmpresaService: AreasEmpresaService,
        private _messageService: MessageService,
        private _activatedRoute: ActivatedRoute,
        private _router: Router
    ) { }

    ngOnInit(): void {
        this.initForm();
        this.cargaDataInicial();

        this._activatedRoute.params.subscribe(params => {

            let codigoRh = params['codigoRH'];
            if (codigoRh !== 'new') {
                this.isCreateMode = false;

                this._empleadoService.getEmpleadoById(codigoRh)
                    .subscribe(
                        {
                            next: (resp) => this.formulario.reset(resp),
                            error: () => this._router.navigate(['empleados'])
                        }
                    );
            }
        })
    }

    cargaDataInicial() {
        this._areasEmpresaService.getAreasEmpresa()
            .subscribe(resp => this.areasEmpresa = resp);
    }

    initForm() {
        this.formulario = this.fb.group({
            id: [null],
            nombres: ['', Validators.required],
            correo: ['', [Validators.required, Validators.email]],
            cargo: ['', Validators.required],
            codigoRH: [''],
            areaEmpresaId: [null, Validators.required]
        });
    }

    onAtras() {
        this._router.navigate(['empleados']);
    }

    onSubmit() {
        if (this.formulario.valid) {

            if (this.isCreateMode) {
                this._empleadoService.createEmpleado(this.formulario.value)
                    .subscribe(resp => {
                        this._messageService.showSuccessMesagge('Registro creado exitosamente')
                        this._router.navigate(['empleados']);
                    });
            } else {
                this._empleadoService.actualizarEmpleado(this.formulario.value)
                    .subscribe(resp => {
                        this._messageService.showSuccessMesagge('Registro creado exitosamente')
                        this._router.navigate(['empleados']);
                    });
            }

        } else {
            console.log('Formulario no válido');
        }
    }
}
