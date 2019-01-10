﻿using RoommatesExpensesManager.Dal;
using RoommatesExpensesManager.Models;
using RoommatesExpensesManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace RoommatesExpensesManager.Controllers
{
    public class ManagerController : Controller
    {
        public ActionResult ShowManagerPage()
        {
            return View();
        }

        public ActionResult AddGroup()
        {
            GroupDal grpDal = new GroupDal();
            VMGroups groupsVM = new VMGroups
            {
                Group = new Group(),
                Groups = grpDal.Groups.ToList<Group>()
            };
            return View(groupsVM);
        }

        public ActionResult AddGroupSubmit(Group grp)
        {
            //try use model binder
            ModelState.Clear();
            TryValidateModel(grp);
            VMGroups groupsVM = new VMGroups();
            Group newGroup = new Group();
            newGroup.city = Request.Form["Group.city"].ToString();
            newGroup.street = Request.Form["Group.street"].ToString();
            newGroup.aptNum = Int32.Parse(Request.Form["Group.aptNum"]);
            GroupDal grpDal = new GroupDal();
            if (ModelState.IsValid)
            {
                grp.managerUserName = ((User)(Session["CurrentUser"])).UserName;
                grpDal.Groups.Add(grp);
                grpDal.SaveChanges();
                groupsVM.Group = new Group();
            }
            else
            {
                groupsVM.Group = newGroup;
            }
            groupsVM.Groups = grpDal.Groups.ToList<Group>();
            return View("AddGroup", groupsVM);
        }

        public ActionResult AddCategory()
        {
            Category ctgy = new Category();
            return View(ctgy);
        }

        public ActionResult AddCategorySubmit()
        {
            return View("ShowManagerPage");
        }

        public ActionResult GetCategoriesByJson()
        {
            CategoryDal ctgyDal = new CategoryDal();
            List<Category> categories = ctgyDal.Categories.ToList<Category>();
            Thread.Sleep(2000);
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveNewCategory()
        {
            CategoryDal ctgyDal = new CategoryDal();
            Category newCategory = new Category
            {
                Type = Request.Form["Type"].ToString()
            };
            object x = Request.Form["Type"];
            if (ModelState.IsValid)
            {
                try
                {
                ctgyDal.Categories.Add(newCategory);
                ctgyDal.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    ViewBag.addNewCategoryError = e.InnerException.Message;
                }
            }
            List<Category> categories = ctgyDal.Categories.ToList<Category>();
            Thread.Sleep(2000);
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddUserToGroup()
        {
            UserDal usrDal = new UserDal();

            return View();
        }
    }
}