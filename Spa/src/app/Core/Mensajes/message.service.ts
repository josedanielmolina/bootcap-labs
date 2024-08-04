import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import Swal from 'sweetalert2'
import { ToastrService } from 'ngx-toastr';

@Injectable({
    providedIn: 'root'
})
export class MessageService {

    constructor(private toastr: ToastrService) { }


    showSuccessMesagge(message: string) {
        Swal.fire({
            position: "top-end",
            icon: "success",
            title: message,
            showConfirmButton: false,
            timer: 1500
        });
    }

    showErrorMessage = (message: string) => {

        Swal.fire({
            title: 'Error!',
            text: message,
            icon: 'error',
            confirmButtonText: 'cerrar'
        })

        // this.toastr.error(message, 'Toastr fun!');
    }

    async confirmDelete(): Promise<boolean> {
        const result = await Swal.fire({
            title: '¿Está seguro?',
            text: `¿Esta seguro de eliminar el registro?`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, eliminar!',
            cancelButtonText: 'No, cancelar'
        });

        return result.isConfirmed;
    }
}
