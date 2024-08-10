import { Component, OnInit, ViewChild } from '@angular/core';
import { PageEvent, MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { Router } from '@angular/router';
import { UserService } from '../../../Core/Services/user.service';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-list-user',
  standalone: true,
  imports: [MatTableModule, MatPaginatorModule, MatButtonModule],
  templateUrl: './list-user.component.html',
  styleUrl: './list-user.component.css'
})
export class ListUserComponent implements OnInit {
    users: any[] = [];
    dataSource = new MatTableDataSource<any>(this.users);
    displayedColumns: string[] = ['id', 'nombre', 'correo', 'acciones'];

    constructor(private userService: UserService, private router: Router) {}

    ngOnInit(): void {
      this.loadUsers();
    }

    loadUsers(): void {
      this.userService.getUsuarios().subscribe((data: any) => {
        this.users = data;
        this.dataSource.data = this.users;
      });
    }

    editUser(id: number): void {
      this.router.navigate([`/edit-user/${id}`]);
    }

    deleteUser(id: number): void {
      this.userService.deleteUsuario(id).subscribe(() => {
        this.loadUsers(); // Recargar la lista de usuarios después de eliminar
      });
    }
  }
