using Aplicacao.Interfaces;
using Entidades.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Token;
using Entidades.Enums;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsuarioController : ControllerBase
    {
        private readonly IAplicacaoUsuario _IAplicationUsuario;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public UsuarioController(IAplicacaoUsuario IAplicationUsuario, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _IAplicationUsuario = IAplicationUsuario;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        // qualquer um pode acessar esse token que tiver na config do cors
        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/CriarToken")]
        public async Task<IActionResult> CriarToken([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
            {
                return Unauthorized();
            }

            var resultado = await _IAplicationUsuario.ExisteUsuario(login.email, login.senha);

            if (resultado)
            {
                var idUsuario = await _IAplicationUsuario.RetornaIdUsuario(login.email);

                var token = new TokenJWTBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
                .AddSubject("teste")
                .AddIssuer("Teste.Securiry.Bearer")
                .AddAudience("Teste.Securiry.Bearer")
                .AddClaim("idUsuario", idUsuario)
                .AddExpiry(5)
                .Builder();


                return Ok(token.value);
            } else
            {
                return Unauthorized();
            }

        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/AdicionaUsuario")]
        public async Task<IActionResult> AdicionaUsuario([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
            {
                return Unauthorized();
            }

            var resultado = await _IAplicationUsuario.AdicionarUsuario(login.email, login.senha, login.idade, login.celular);

            if (resultado)
            {
                return Ok("Usuário adicionado com sucesso!");
            } else
            {
                return Ok("Erro ao adicionar usuário!");
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/CriarTokenIdentity")]
        public async Task<IActionResult> CriarTokenIdentity([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
            {
                return Unauthorized();
            }

            

            var resultado = await _signInManager.PasswordSignInAsync(login.email, login.senha, false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                var idUsuario = await _IAplicationUsuario.RetornaIdUsuario(login.email);

                var token = new TokenJWTBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
                .AddSubject("teste")
                .AddIssuer("Teste.Securiry.Bearer")
                .AddAudience("Teste.Securiry.Bearer")
                .AddClaim("idUsuario", idUsuario)
                .AddExpiry(5)
                .Builder();

                return Ok(token.value);
            }
            else
            {
                return Unauthorized();
            }
        }
        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/AdicionaUsuarioIdentity")]
        public async Task<IActionResult> AdicionaUsuarioIdentity([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
            {
                return Unauthorized();
            }

            var user = new ApplicationUser { 
                UserName = login.email, 
                Email = login.email, 
                Celular = login.celular, 
                Tipo = ETipoUsuario.Comum 
            };

            //identity criptograva a senha
            var resultado = await _userManager.CreateAsync(user, login.senha);

            if (resultado.Errors.Any())
            {
                return Ok(resultado.Errors);
            }

            // Geração de email de Confirmação de cadastro
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // retorno do email
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var resultado2 = await _userManager.ConfirmEmailAsync(user, code);
            var StatusMessage = resultado2.Succeeded;

            if(resultado2.Succeeded)
            {
                return Ok("Usuário Adicionado com Sucesso!");  
            } else
            {
                return Ok("Falha ao confirmar usuário!");
            }
        }

    }
}
