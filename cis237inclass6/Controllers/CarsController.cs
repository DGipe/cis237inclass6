using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237inclass6.Models;

namespace cis237inclass6.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
               private CarsDGipeEntities db = new CarsDGipeEntities();

        // GET: Cars
        public ActionResult Index()
        {
            //stup a variable to hold the cars data
            DbSet<Car> CarsToFilter = db.Cars;

            //etup some strings to gold the datathat might be in the session
            //IF ther is nothing in the sessin w3 can still use these varaiabls
            //as a defualt vaue
            string filterMake = "";
            string filterMin = "";
            string filterMax = "";

            //Define a min and mac for the cylinders
            int min = 0;
            int max = 16;

            //Check to see if ther is a valie in the sessin, and if ther eis, assign
            //it to the variable that we setup to hold the value.
            if (Session["make"] != null && !String.IsNullOrWhiteSpace((string)Session["make"]))
            {
                filterMake = (string)Session["make"];
            }

            if (Session["min"] != null && !String.IsNullOrWhiteSpace((string)Session["min"]))
            {
                filterMin = (string)Session["min"];
                min = Int32.Parse(filterMin);
            }

            if (Session["max"] != null && !String.IsNullOrWhiteSpace((string)Session["max"]))
            {
                filterMax = (string)Session["max"];
                max = Int32.Parse(filterMax);
            }

            //Do the filter on the CarsToFilter Dataset.  Use the where that we used begore
            //wehn doing the last in calsls, only this time send in more lamda expressions to
            //narrow it down further.  Since we seup the default values for each of the filter
            //parameters, min, max, and filterMae we can cound no this alwats running with
            //no errors\

            IEnumerable<Car> filtered = CarsToFilter.Where(car => car.cylinders >= min &&
                                                                  car.cylinders <= max &&
                                                                  car.make.Contains(filterMake));
            //Convert the dataset to a list now that thhat the quert work is done on it.
            //Te view is expecting a list, so we conver the databaseset to a list
            IEnumerable<Car> finalFiltered = filtered.ToList();

            //Place the string representation fo the valuies that are in the session into
            //the viewbag so that they can be retrieved and diesplayed on the view
            ViewBag.filtermake = filterMake;
            ViewBag.filterMin = filterMin;
            ViewBag.filterMax = filterMax;

            //Return the view with the filtered selection of cars.
            return View(finalFiltered);


            //return View(db.Cars.ToList());
        }

        // GET: Cars/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,year,make,model,type,horsepower,cylinders")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(car);
        }

        // GET: Cars/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,year,make,model,type,horsepower,cylinders")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(car);
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Mark the method as POST since it is reached from the form submit
        //Make sure to validate the antiforgeytoken to since we included it in the form.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter()
        {
            //Get the form data that we sent ou of the request obkec
            //The string that is used as a key to ge the data matces the
            //name property of the form control
            string make = Request.Form.Get("make");
            string min = Request.Form.Get("min");
            string max = Request.Form.Get("max");

            //Now that we have the data pulled out from the requesat objet
            // lets put it into the session so that other methocs can have 
            // access too it.
            Session["make"] = make;
            Session["min"] = min;
            Session["max"] = max;

                        
            //Redirect to the index page
            return RedirectToAction("Index");
        }

        public ActionResult Json()
        {
            return Json(db.Cars.ToList(), JsonRequestBehavior.AllowGet);
        }

    }
}
