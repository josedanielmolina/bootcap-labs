import { BreakpointObserver } from '@angular/cdk/layout';
import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSidenav, MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { delay } from 'rxjs';
import Swal from 'sweetalert2';

@Component({
    selector: 'app-side-menu',
    standalone: true,
    imports: [
        CommonModule,
        RouterModule,
        MatSidenavModule,
        MatIconModule,
        MatButtonModule,
        MatListModule,
        RouterOutlet,
    ],
    templateUrl: './side-menu.component.html',
    styleUrl: './side-menu.component.css'
})
export class SideMenuComponent  {




}
