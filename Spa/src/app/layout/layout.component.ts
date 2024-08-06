import { Component } from '@angular/core';
import { SideMenuComponent } from './side-menu/side-menu.component';
import { NgIf } from '@angular/common';

@Component({
    selector: 'app-layout',
    standalone: true,
    imports: [SideMenuComponent, NgIf],
    templateUrl: './layout.component.html',
    styleUrl: './layout.component.css'
})
export class LayoutComponent {

}
