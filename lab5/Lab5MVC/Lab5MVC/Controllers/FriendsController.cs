﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lab5MVC.Models;

namespace Lab5MVC.Controllers
{
    public class FriendsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Friends
        public ActionResult Index()
        {
            return View(db.Friends.ToList());
        }

        // GET: Friends/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Friend friend = db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }

            return View(friend);
        }

        // GET: Friends/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            var model = new Friend();
            ViewBag.ErrorMessageFriendId = string.Empty;
            return View(model);
        }

        // POST: Friends/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FriendId,Name,Hometown")]
            Friend friend)
        {
            if (ModelState.IsValid)
            {
                var target = db.Friends.FirstOrDefault(f => f.FriendId == friend.FriendId);
                if (target != null)
                {
                    ViewBag.ErrorMessageFriendId = "That Friend Id already exists";
                    return View(friend);
                }
                ViewBag.ErrorMessageFriendId = string.Empty;
                db.Friends.Add(friend);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ErrorMessageFriendId = string.Empty;
            return View(friend);
        }

        // GET: Friends/Edit/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Friend friend = db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }

            return View(friend);
        }

        // POST: Friends/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator, Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FriendId,Name,Hometown")]
            Friend friend)
        {
            if (ModelState.IsValid)
            {
                db.Entry(friend).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(friend);
        }

        // GET: Friends/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Friend friend = db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }

            return View(friend);
        }

        // POST: Friends/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Friend friend = db.Friends.Find(id);
            db.Friends.Remove(friend);
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
    }
}