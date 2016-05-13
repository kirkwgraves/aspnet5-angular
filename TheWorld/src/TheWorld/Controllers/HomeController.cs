using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TheWorld.ViewModels;
using TheWorld.Services;
using TheWorld.Models;

namespace TheWorld.Controllers
{
    public class HomeController : Controller
    {
        private IMailService _mailService;
        private IWorldRepository _repository;

        public HomeController(IMailService service, IWorldRepository repository)
        {
            _mailService = service;
            _repository = repository;
        }

        public IActionResult Index()
        {
            var trips = _repository.GetAllTrips();

            return View(trips);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = Startup.Configuration["AppSettings: SiteEmailAddress"];

                if (string.IsNullOrWhiteSpace(email))
                {
                    ModelState.AddModelError("", "Cound not send email, configuration problem.");
                }

                if (_mailService.SendMail(email,
                    email,
                    $"Contact Page from {model.Name} ({model.Email})",
                    model.Message))
                {
                    ModelState.Clear();

                    ViewBag.Message = "Mail sent. Thanks!";
                }
            }
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
