using Api.Empresa.Models;
using DTO.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public AuthController(IConfiguration configuration, AppDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO loginDto)
    {
        var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.Correo == loginDto.Correo);
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Contrasena, usuario.Contrasena))
        {
            return Unauthorized();
        }

        var (token, expiration) = GenerateJwtToken(usuario);
        return Ok(new { Token = token, Expiration = expiration });
    }

    [HttpGet("validar-correo/{correo}")]
    public async Task<IActionResult> ValidateCorreo(string correo)
    {
        // Validar el correo
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
        if (usuario == null)
        {
            return BadRequest("Correo no existe");
        }

        // Generar el codigo de verificacion
        usuario.CodigoValidacion = new Random().Next(1000, 9999).ToString();
        usuario.FechaExpiracionCodigo = DateTime.Now.AddMinutes(5);
        await _context.SaveChangesAsync();
        // Enviar el correo con el codigo de verificacion

        return Ok();
    }

    [HttpGet("validar-codigo/{correo}/{codigo}")]
    public async Task<IActionResult> ValidateCodigo(string correo, string codigo)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
        if (usuario == null)
        {
            return BadRequest("Correo no existe");
        }

        if (usuario.CodigoValidacion != codigo || usuario.FechaExpiracionCodigo < DateTime.Now)
        {
            return BadRequest("Codigo invalido o expirado");
        }

        return Ok();
    }

    // cambiar contrasñea
    [HttpPost("cambiar-contrasena")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == changePasswordDto.Correo);
        if (usuario == null)
        {
            return BadRequest("Correo no existe");
        }

        if (usuario.CodigoValidacion != changePasswordDto.Codigo || usuario.FechaExpiracionCodigo < DateTime.Now)
        {
            return BadRequest("Codigo invalido o expirado");
        }

        usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.Contrasena);
        await _context.SaveChangesAsync();

        var (token, expiration) = GenerateJwtToken(usuario);
        return Ok(new { Token = token, Expiration = expiration });
    }

    private (string token, DateTime expiration) GenerateJwtToken(Usuario usuario)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings.GetValue<string>("SecretKey");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Correo),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expiration = DateTime.Now.AddMinutes(30);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.GetValue<string>("Issuer"),
            audience: jwtSettings.GetValue<string>("Audience"),
            claims: claims,
            expires: expiration,
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return (tokenString, expiration);
    }
}


