import { inject, Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router, CanActivateFn, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, of, switchMap } from 'rxjs';
import { SeguridadService } from '../Services/seguridad.service';

export const AuthGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> => {
    const router = inject(Router);
    const seguridadService = inject(SeguridadService);

    return of(seguridadService.estaLogueado()).pipe(
        switchMap((estaLogueado) => {
            if (!estaLogueado) {
                const redirectURL = state.url === '/sign-out' ? '' : `redirectURL=${state.url}`;
                const urlTree = router.parseUrl(`/login?${redirectURL}`);
                return of(urlTree);
            }
            return of(true);
        })
    );
};
