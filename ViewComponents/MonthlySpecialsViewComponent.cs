using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvc1.Models;

namespace mvc1.ViewComponents
{
    [ViewComponent]
    public class MonthlySpecialsViewComponent : ViewComponent
    {
        private readonly BlogDataContext db;

        public MonthlySpecialsViewComponent(BlogDataContext db)
        {
            this.db = db;
        }

        public IViewComponentResult Invoke()
        {
            var specials = db.MonthlySpecials.ToArray();
            return View(specials);
        }
    }
}
