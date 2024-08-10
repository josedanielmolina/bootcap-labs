using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCrypt.Net;
using Api.Models;
using DTO.DTO;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
        var usuariosDto = usuarios.Select(u => new UsuarioDTO
        {
            Id = u.Id,
            Nombre = u.Nombre,
            Correo = u.Correo
        }).ToList();

        return Ok(usuariosDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioDTO>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        var usuarioDto = new UsuarioDTO
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Correo = usuario.Correo
        };

        return Ok(usuarioDto);
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioDTO>> CreateUsuario(UsuarioCreateDTO usuarioCreateDto)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(usuarioCreateDto.Contrasena);

        var usuario = new Usuario
        {
            Nombre = usuarioCreateDto.Nombre,
            Correo = usuarioCreateDto.Correo,
            Contrasena = hashedPassword
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var usuarioDto = new UsuarioDTO
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Correo = usuario.Correo
        };

        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuarioDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUsuario(int id, UsuarioUpdateDTO usuarioUpdateDto)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        usuario.Nombre = usuarioUpdateDto.Nombre;
        usuario.Correo = usuarioUpdateDto.Correo;

        if (!string.IsNullOrEmpty(usuarioUpdateDto.Contrasena))
        {
            usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(usuarioUpdateDto.Contrasena);
        }

        _context.Entry(usuario).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

