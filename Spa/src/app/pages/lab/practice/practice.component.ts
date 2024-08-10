import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-practice',
    standalone: true,
    imports: [FormsModule],
    templateUrl: './practice.component.html',
    styleUrl: './practice.component.css'
})
export class PracticeComponent {


    inputIncrementador: number = 0;
    contador: number = 0;

    onAumentarContador() {
        this.contador = this.contador + this.inputIncrementador;
    }

}
