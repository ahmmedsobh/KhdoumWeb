﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KhdoumWeb.Controllers
{
    public class RouteController : Controller
    {
        public IActionResult Categories()
        {
            return View();
        }


        public IActionResult States()
        {
            return View();
        }

        public IActionResult Members()
        {
            return View();
        }

        public IActionResult Items()
        {
            return View();
        }
    }
}