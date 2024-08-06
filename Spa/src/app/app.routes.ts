import { Routes } from '@angular/router';
import { ListEmpleadosComponent } from './pages/Empleados/list-empleados/list-empleados.component';
import { FormEmpleadoComponent } from './pages/Empleados/form-empleado/form-empleado.component';
import { LayoutComponent } from './layout/layout.component';
import { LoginComponent } from './pages/auth/login/login.component';
import { AuthGuard } from './Core/Guards/auth.guard';

export const routes: Routes = [

    { path: 'login', component: LoginComponent },

    {
        path: '',
        component: LayoutComponent,
        canActivateChild: [AuthGuard],
        children: [
            { path: 'empleados', component: ListEmpleadosComponent },
            { path: 'empleado/:codigoRH', component: FormEmpleadoComponent },
        ]
    }
];
