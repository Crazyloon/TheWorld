using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private IWorldRepository _repository;
        private ILogger<AppController> _logger;

        public AppController(IMailService mailService, 
            IConfigurationRoot config, 
            IWorldRepository context,
            ILogger<AppController> logger)
        {
            _mailService = mailService;
            _config = config;
            _repository = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize] // Requires authentication to get into this method
        public IActionResult Trips()
        {
            //// Try to get all the trips and display to view, if unable to log the error.
            //try
            //{
            //    var data = _repository.GetAllTrips();

            //    return View(data);
            //}
            //catch (Exception ex)
            //{
            //    // Logger can be used to save information about what went wrong for later review
            //    _logger.LogError($"Failed to get trips in the index page: {ex.Message}");
            //    return Redirect("/error");
            //}
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            // Property Error
            if (model.Email.Contains("aol.com")) ModelState.AddModelError("Email", "We don't support AOL addresses");
            // Model Error
            //if (model.Email.Contains("aol.com")) ModelState.AddModelError("", "We don't support AOL addresses");


            if (ModelState.IsValid)
            {
                _mailService.SendMail(_config["MailSettings:ToAddress"], model.Email, "From The World", model.Message);

                ModelState.Clear();
                ViewBag.UserMessage = "Message Sent";
            }
            
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
