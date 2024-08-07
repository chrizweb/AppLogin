using Microsoft.AspNetCore.Mvc;
using AppLogin.Data;
using AppLogin.Models;
using AppLogin.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace AppLogin.Controllers {
	public class AccessController : Controller {

		private readonly AppDbContext dbContext;
		public AccessController(AppDbContext appDbContext) {
			dbContext = appDbContext;
		}

		[HttpGet]
		public IActionResult Register() {
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(UserViewModel model ) {

			if(model.Password != model.ConfirmPassword) {
				ViewData["Message"] = "¡Passwords are different!";
				return View();
			}
			
			User user = new User() {
				Name = model.Name,
				Email = model.Email,
				Password = model.Password,
			};

			/*Creando usuario*/
			await dbContext.Users.AddAsync(user);
			await dbContext.SaveChangesAsync();
			
			if(user.UserId != 0) {
				return RedirectToAction("Login", "Access");
			}
			else {
				ViewData["Message"] = "¡Error create user!";
			}

			return View();
		}

		[HttpGet]
		public IActionResult Login() {

			if (User.Identity.IsAuthenticated) {
				return RedirectToAction("Index", "Home");
			}

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model) {
			User? user_found = await dbContext.Users
				.Where(u =>
					u.Email == model.Email && u.Password == model.Password
				).FirstOrDefaultAsync();

			if(user_found == null) {
				ViewData["Message"] = "!No matches found!";
				return View();
			}

			/*Guardar informacion de usuario en autenticacion cookies*/
			/****************************************************************/
			List<Claim> claims = new List<Claim>() {

				new Claim(ClaimTypes.Name, user_found.Name)
			};

			ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			AuthenticationProperties properties = new AuthenticationProperties() {
				/*Refrescar*/
				AllowRefresh = true
			};

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity),
				properties
			);
			/****************************************************************/

			return RedirectToAction("Index", "Home");
		}
	}
}
