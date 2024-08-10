// user.model.ts
export interface UsuarioDTO {
    id: number;
    nombre: string;
    correo: string;
  }

  export interface UsuarioCreateDTO {
    nombre: string;
    correo: string;
    contrasena: string;
  }

  export interface UsuarioUpdateDTO {
    nombre: string;
    correo: string;
    contrasena?: string;
  }
