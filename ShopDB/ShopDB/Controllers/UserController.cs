using ShopDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ShopDB.Models.Vm;
using System.IO;
using System.Net;

namespace ShopDB.Controllers
{
    public class UserController : Controller
    {
        ShopDBEntities db = new ShopDBEntities();
        // GET: User
        public ActionResult Index()
        {
            var user = db.Users.Include(d=>d.Details.Select(p=>p.Product)).OrderByDescending(u=>u.UserId).ToList();
            return View(user);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Vmodel vmodel, int[]productId)
        {
            if(ModelState.IsValid)
            {
                User user = new User()
                {
                    UserName = vmodel.UserName,
                    Age = vmodel.Age,
                    UserDate = vmodel.UserDate,
                    IsAudil=vmodel.IsAudil,
                };
               
                HttpPostedFileBase file = vmodel.ImageFile;
                if (file != null)
                {
                    string fileName = Path.Combine("/image/" + DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(fileName));
                    user.ImagePath = fileName;
                }

                foreach (var p in productId)
                {
                    var product = db.Products.FirstOrDefault(f => f.ProductId == p);
                    Detail detail = new Detail()
                    {
                        User = user,
                        UserId = user.UserId,
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                    };
                    db.Details.Add(detail);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vmodel);
        }
        public ActionResult AddProduct(int? id)
        {
            ViewBag.product = new SelectList(db.Products, "productId", "ProductName", (id != null) ? id.ToString() : "");
            return PartialView("_addProduct");
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {            
            User user = db.Users.Find(id); 
            Vmodel vmodel = new Vmodel()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Age = user.Age,
                UserDate = user.UserDate,
                IsAudil = user.IsAudil,
                ImagePath = user.ImagePath,
            };
            return View(vmodel);
        }

        [HttpPost]
        public ActionResult Edit(Vmodel vmodel, int[] productId)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(vmodel.UserId);

                user.UserName = vmodel.UserName;
                user.Age = vmodel.Age;
                user.UserDate = vmodel.UserDate;
                user.IsAudil = vmodel.IsAudil;            
                HttpPostedFileBase file = vmodel.ImageFile;
                if (file != null)
                {
                    string fileName = Path.Combine("/image/" + DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(fileName));
                    user.ImagePath = fileName;
                }
                else
                {
                    user.ImagePath = user.ImagePath;
                }
                var dp = db.Details.Where(u => u.UserId == user.UserId).ToList();
                foreach(var d in dp)
                {
                    db.Details.Remove(d);
                }
                foreach (var p in productId)
                {
                    var product = db.Products.FirstOrDefault(f => f.ProductId == p);
                    Detail detail = new Detail()
                    {
                        User = user,
                        UserId = user.UserId,
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                    };
                    db.Details.Add(detail);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vmodel);
        }
        public ActionResult Delete(int ?id)
        {
            if(id != null)
            {
                var user = db.Users.FirstOrDefault(u => u.UserId == id);  
                var detail = db.Details.Where(u => u.UserId == id).ToList();
                foreach (var d in detail)
                { 
                    db.Details.Remove(d);
                }
                db.Users.Remove(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }

}