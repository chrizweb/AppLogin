using Microsoft.AspNetCore.Mvc;
using AppLogin.Data;
using AppLogin.Models;
using AppLogin.ViewModels;
using Microsoft.EntityFrameworkCore;
 
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
		public async Task<IActionResult> Register(UserViewModel model) {

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
	}
}
