using CesarDev.Api.ViewModels;
using CesarDev.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CesarDev.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(INotificador notificador,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager) : base(notificador)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true,
            };

            var resultado = await _userManager.CreateAsync(user, registerUser.Password);
            if (resultado.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return CustomResponse(registerUser);
            }

            foreach (var error in resultado.Errors)
                NotificarErro(error.Description);

            return CustomResponse(registerUser);
        }

        [HttpPost("entrar")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

           var resultado = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (resultado.Succeeded)
                return CustomResponse(loginUser);

            if (resultado.IsLockedOut)
            {
                NotificarErro("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse(loginUser);
            }

            NotificarErro("Usuário ou Senha incorretos");
            return CustomResponse(loginUser);
        }
    }
}
