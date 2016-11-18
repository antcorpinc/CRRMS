using CRRMS.Web.Models;
using CRRMS.Web.Services;
using CRRMS.Web.ViewModels;
//using Microsoft.AspNet.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace  CRRMS.Web.Controllers
    {
        public class  AppController:Controller
        {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private IWorldRepository _repository;
        private ILogger<AppController> _logger;

        public AppController(IMailService mailService,IConfigurationRoot config,
            IWorldRepository repository,
            ILogger<AppController> logger)
        {
            _mailService = mailService;
            _config = config;
            _repository = repository;
            _logger = logger;
        }

         public IActionResult  Index()
         {
            //try { 
            //var data = _repository.GetAllTrips();

            // return View(data);
            //}
            //catch(Exception ex)
            //{
            //    _logger.LogError($"Failed to get trips:{ex.Message} ");
            //    return View();
            //}
            return View();
         }

        [Authorize]
        public IActionResult Trips()
        {
            var trips = _repository.GetAllTrips();

            return View(trips);

        }

         public IActionResult Contact()
         {
           return View();
         }
        
        [HttpPost]
        public IActionResult  Contact(ContactViewModel model)
        {
            if(model.Email.Contains("aol.com"))
            {
                //Info : The first parameter has to be model property or "" if applicable to entire model
                ModelState.AddModelError("Email", "Do not support aol addresses");
            }

            if (ModelState.IsValid)
            {
                _mailService.SendEmail(_config["MailSettings:ToAddress"], model.Email, "From EZ Car Rental", model.Message);

                //Clear tht model data
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