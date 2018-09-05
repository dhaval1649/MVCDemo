using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoApp;

namespace DemoApp.Controllers
{
    public class EmployeeMastersController : Controller
    {
        private dbEmployeeEntities db = new dbEmployeeEntities();

        // GET: EmployeeMasters
        public ActionResult Index()
        {
            IEnumerable<EmployeeMaster> emp = db.EmployeeMasters.AsEnumerable()
              .OrderBy(n => n.Id)
                  .Select(n =>
                  new EmployeeMaster
                  {
                      Name = n.Name,
                      Surname = n.Surname,
                      Address = n.Address,
                      Qualification = n.Qualification,
                      Contact_Number = n.Contact_Number,
                      DepartmentName = db.DepartmentMasters.Find(n.DepartmentId).DepartmentName,
                      Id = n.Id
                  }).ToList();
            return View(emp);
        }

        // GET: EmployeeMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeMaster employeeMaster = db.EmployeeMasters.Find(id);
            if (employeeMaster == null)
            {
                return HttpNotFound();
            }
            return View(employeeMaster);
        }

        // GET: EmployeeMasters/Create
        public ActionResult Create()
        {
            EmployeeMaster employeeMaster = new EmployeeMaster();
            employeeMaster.Departments = GetDepartments();
            return View(employeeMaster);
        }

        // POST: EmployeeMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Surname,Address,Qualification,Contact_Number,DepartmentId")] EmployeeMaster employeeMaster)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeMasters.Add(employeeMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employeeMaster);
        }

        // GET: EmployeeMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeMaster employeeMaster = new EmployeeMaster();
            employeeMaster = db.EmployeeMasters.Find(id);
            employeeMaster.Departments = GetDepartments();
            if (employeeMaster == null)
            {
                return HttpNotFound();
            }
            return View(employeeMaster);
        }

        // POST: EmployeeMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,Address,Qualification,Contact_Number,DepartmentId")] EmployeeMaster employeeMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employeeMaster);
        }

        // GET: EmployeeMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeMaster employeeMaster = db.EmployeeMasters.Find(id);
            if (employeeMaster == null)
            {
                return HttpNotFound();
            }
            return View(employeeMaster);
        }

        // POST: EmployeeMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeMaster employeeMaster = db.EmployeeMasters.Find(id);
            db.EmployeeMasters.Remove(employeeMaster);
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

        public IEnumerable<SelectListItem> GetDepartments()
        {
            try
            {


                List<SelectListItem> departments = db.DepartmentMasters.AsNoTracking()
                    .OrderBy(n => n.DepartmentName)
                        .Select(n =>
                        new SelectListItem
                        {
                            Value = n.Id.ToString(),
                            Text = n.DepartmentName
                        }).ToList();
                var departmenttip = new SelectListItem()
                {
                    Value = null,
                    Text = "--- select Department ---"
                };
                departments.Insert(0, departmenttip);
                return new SelectList(departments, "Value", "Text");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
