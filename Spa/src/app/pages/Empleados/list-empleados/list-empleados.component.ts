import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { Empleado } from '../../../Core/Models/Empleado.interface';
import { EmpleadoService } from '../../../Core/Services/empleado.service';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatIcon } from '@angular/material/icon';
import { MessageService } from '../../../Core/Mensajes/message.service';
import { Router } from '@angular/router';


@Component({
    selector: 'app-list-empleados',
    standalone: true,
    imports: [MatTableModule, MatPaginatorModule, MatIcon],
    templateUrl: './list-empleados.component.html',
    styleUrl: './list-empleados.component.css'
})
export class ListEmpleadosComponent implements OnInit {

    empleados: Empleado[] = [];
    displayedColumns: string[] = ['codigoRh', 'nombres', 'cargo', 'acciones'];
    dataSource = new MatTableDataSource<Empleado>(this.empleados);


    @ViewChild(MatPaginator) paginator!: MatPaginator;

    constructor(
        private _empleadoService: EmpleadoService,
        private _messageService: MessageService,
        private _router: Router
    ) {

    }

    ngAfterViewInit() {
        this.dataSource.paginator = this.paginator;
    }


    ngOnInit(): void {
        this.cargarEmpleados();
    }

    cargarEmpleados() {
        this._empleadoService.getEmpleados().subscribe(resp => {
            this.empleados = resp;
            this.dataSource.data = this.empleados;

        });
    }

    onCreateEmpleado() {
        this._router.navigate(['empleado/new']);
    }

    onEditar(element: Empleado): void {
        this._router.navigate(['empleado', element.codigoRH]);
    }

    async onEliminar(element: Empleado): Promise<void> {

        let confirm = await this._messageService.confirmDelete()
        if (confirm) {
            this._empleadoService.deleteEmpleado(element.id!)
                .subscribe(
                    {
                        complete: () => {
                            this._messageService.showSuccessMesagge('Registro eliminado exitosamente');
                            this.cargarEmpleados();
                        },
                        error: (e) => this._messageService.showErrorMessage(e.error)
                    }
                )
        }
    }

}
