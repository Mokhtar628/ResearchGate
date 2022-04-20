﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;
namespace ResearchGate.Controllers
{
    public class SearchController : Controller
    {
        ResearchGateDatabaseEntities db = new ResearchGateDatabaseEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OUT(string searching)
        {
            searching = searching.ToLower();
            return View(db.Authors.Where(x => x.Fname.ToLower().Contains(searching) || x.email.ToLower().Contains(searching) || x.university.ToLower().Contains(searching)).ToList());
        }
    }
}