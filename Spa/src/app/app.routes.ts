import { Routes } from '@angular/router';
import { ListEmpleadosComponent } from './pages/Empleados/list-empleados/list-empleados.component';
import { FormEmpleadoComponent } from './pages/Empleados/form-empleado/form-empleado.component';

export const routes: Routes = [
    // EMPRESA
    { path: 'empleados', component: ListEmpleadosComponent },
    { path: 'empleado/:codigoRH', component: FormEmpleadoComponent }
];
