import { Routes } from '@angular/router';
import { ListEmpleadosComponent } from './pages/Empleados/list-empleados/list-empleados.component';
import { FormEmpleadoComponent } from './pages/Empleados/form-empleado/form-empleado.component';
import { LayoutComponent } from './layout/layout.component';
import { LoginComponent } from './pages/auth/login/login.component';
import { AuthGuard } from './Core/Guards/auth.guard';
import { ListUserComponent } from './pages/users/list-user/list-user.component';
import { FormUserComponent } from './pages/users/form-user/form-user.component';
import { PracticeComponent } from './pages/lab/practice/practice.component';
import { RecuperarContrasennaComponent } from './pages/auth/recuperar-contrasenna/recuperar-contrasenna.component';

export const routes: Routes = [

    { path: 'login', component: LoginComponent },
    { path: 'recuperar-contrasenna', component: RecuperarContrasennaComponent },

    {
        path: '',
        component: LayoutComponent,
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard],
        children: [
            { path: 'empleados', component: ListEmpleadosComponent },
            { path: 'empleado/:codigoRH', component: FormEmpleadoComponent },
            { path: 'users', component: ListUserComponent },
            { path: 'edit-user/:id', component: FormUserComponent },
            { path: 'practice', component: PracticeComponent },
        ]
    }
];
