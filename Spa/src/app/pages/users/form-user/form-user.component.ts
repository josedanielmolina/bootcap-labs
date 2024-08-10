import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { UserService } from '../../../Core/Services/user.service';
import { UsuarioDTO, UsuarioCreateDTO, UsuarioUpdateDTO } from '../../../Core/Models/user.model';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-form-user',
  templateUrl: './form-user.component.html',
  styleUrls: ['./form-user.component.css'],
  standalone: true,
  imports: [  ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule, MatInputModule]
})
export class FormUserComponent implements OnInit {
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  userForm!: FormGroup;
  userId: number | null = null;
  isEditMode: boolean = false;

  ngOnInit(): void {
    this.userForm = this.fb.group({
      nombre: ['', Validators.required],
      correo: ['', [Validators.required, Validators.email]]
    });

    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.userId = +id;
        this.isEditMode = true;
        this.loadUser();
      }
    });
  }

  loadUser(): void {
    if (this.userId !== null) {
      this.userService.getUsuario(this.userId).subscribe((user: UsuarioDTO) => {
        this.userForm.patchValue(user);
      });
    }
  }

  onSubmit(): void {
    if (this.userForm.valid) {
      const user: UsuarioCreateDTO | UsuarioUpdateDTO = this.userForm.value;
      if (this.isEditMode && this.userId !== null) {
        this.userService.updateUsuario(this.userId, user as UsuarioUpdateDTO).subscribe(() => {
          this.router.navigate(['/list-user']);
        });
      } else {
        this.userService.createUsuario(user as UsuarioCreateDTO).subscribe(() => {
          this.router.navigate(['/list-user']);
        });
      }
    }
  }

  onCancel(): void {
    this.router.navigate(['/list-user']);
  }
}
