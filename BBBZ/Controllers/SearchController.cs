using BBBZ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity;

using NinjaNye;
using NinjaNye.SearchExtensions;


namespace BBBZ.Controllers
{
    public class SearchController : BaseController
    {
        public ActionResult Index(string query, string lookingFor = "content", int page = 1, int pageSize = 10)
        {
            // fix the pages count, later
            // and fix the result page template

            ViewBag.Query = query;
            ViewBag.pageSize = pageSize;
            ViewBag.page = page;
            ViewBag.lookingFor = lookingFor;

            if (page <= 0 || pageSize <= 0 || string.IsNullOrEmpty(lookingFor))
                return BadRequest();

            string[] queryTerms = null;
            if(string.IsNullOrEmpty(query) == false)
                queryTerms = query.MargeWith(query.Split(' ', ',', '+'));

            if (lookingFor.ToLower() == "content")
            {
                if (string.IsNullOrEmpty(query))
                    return View("SearchContents", null);

                var tmp = db.Contents
                        .Include(x => x.Access)
                        .Include(x => x.Category)
                        .Where(x => x.Published && x.Access != null && MyViewLevelIDs.Contains(x.Access.ID))
                        .Search(
                            x => x.Title,
                            x => x.IntroText,
                            x => x.FullText,
                            x => x.MetaDesc,
                            x => x.MetaKey)
                        .Containing(queryTerms)
                        .OrderByDescending(x=>x.CreatedTime);
                
                ViewBag.ItemCount = tmp.Count();
                ViewBag.PageCount =(int) Math.Ceiling(ViewBag.ItemCount * 1.0 / pageSize);

                var res = 
                        tmp.Skip(pageSize * (page - 1))
                        .Take(pageSize)
                        .ToList();

                return View("SearchContents", res);
            }
            else if (lookingFor.ToLower() == "user")
            {
                if (string.IsNullOrEmpty(query))
                    return View("SearchUser", null);

                var tmp = db.Profiles.Search(
                        x => x.username,
                        x => x.Name,
                        x => x.LastName,
                        x => x.Email)
                    .Containing(queryTerms)
                    .OrderBy(x => x.Name);

                ViewBag.ItemCount = tmp.Count();
                ViewBag.PageCount = Math.Ceiling(ViewBag.ItemCount * 1.0 / pageSize);

                var res =
                    tmp.Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .ToList();

                return View("SearchUsers", res);
            }

            return View("SearchContents", null);
        }
	}
}