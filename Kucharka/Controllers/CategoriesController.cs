using Semestralka.DatabaseModels;
using Semestralka.DataObjects;
using Semestralka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Semestralka.Controllers
{
    public class CategoriesController : Controller
    {
        //
        // GET: /Categories/
        public async Task<ActionResult> Index(bool? error)
        {
            if(error == true)
            {
                ModelState.AddModelError(string.Empty, "Kategorie se používá u některého z receptů");
            }

            List<CategoryDO> categories = await CategoryDO.GetCategoriesAsync();

            ViewBag.Categories = categories;

            return View();
        }

        /**
        *  Method for inserting new category into database
        **/
        [HttpPost]
        public async Task<ActionResult> Index(Category category, string add)
        {
            await Task.Delay(0);

            try
            {
                using (Entities kucharkaEntities = new Entities())
                {
                    //sends data into database
                    kucharkaEntities.Categories.Add(category);
                    //save changes in database
                    kucharkaEntities.SaveChanges();
                    //initialization variable for error message
                    string message = string.Empty;

                    //storing information message into variable depending on value returned from databse procedure
                    if (category.id_category == -1)
                    {
                        ModelState.AddModelError(string.Empty, "Kategorie už existuje");
                    }
                }
            } 
            catch(Exception ex)
            {
                ModelState.AddModelError("name_category", "Název kategorie musí být vyplněný");
            }
            List<CategoryDO> categories = await CategoryDO.GetCategoriesAsync();

            ViewBag.Categories = categories;

            return View("Index");

            //return RedirectToAction("Index", "Categories");
        }

        public async Task<ActionResult> Remove(int id)
        {
            await Task.Delay(0);

            try
            {
                using (Entities context =
                    new Entities())
                {
                    //find category
                    var category = context.Categories.Single(x => x.id_category == id);
                    //remove category in db
                    context.Categories.Remove(category);
                    //save change in db
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                if(ex.Message.Equals("An error occurred while updating the entries. See the inner exception for details."))
                {
                    return RedirectToAction("Index", "Categories", new { error = true });
                }
            }

            return RedirectToAction("Index", "Categories");
        }

        public async Task<ActionResult> Edit(int? id, string add, CategoryModel model) 
        {
            try
            {
                Category category = await CategoryDO.GetCategoryAsync((int)id);

                @ViewBag.Category = category;

                //Control if button for saving change was pressed. If not edit page is normally loaded 
                if (add != null)
                {
                    using (Entities context = new Entities())
                    {
                        //find category
                        var categoryRecord = context.Categories.Single(x => x.id_category == id);
                        //set new category name
                        categoryRecord.name_category = model.name_category;
                        //save change in db
                        context.SaveChanges();

                        return RedirectToAction("Index", "Categories");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.StackTrace.Contains("GetCategoryAsync") || id == null)
                {
                    return RedirectToAction("Index", "Categories");
                }
            }
            return View();   
        }
	}
}