using System. Collections.Generic;
using System.Linq;
using System. Threading. Tasks;
using Microsoft.AspNetCore.Mvc;
using homework.Models;
using homework.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using homework.ViewModel;


namespace homework.Controllers
{   

public class CategoryController : Controller
{


private ApplicationDbContext _db;

public CategoryController(ApplicationDbContext db)

{
    _db = db;
}




public IActionResult Index(string searchKey)
{
    var categories = _db.Categories.Where(x => !x.IsDelete);

    if (!string.IsNullOrWhiteSpace(searchKey))
    {
        categories = categories.Where(x => x.Name.Contains(searchKey)
        );
    }
ViewData["msg"] = "welcome to category page";
   
    return View(categories.ToList());
}


[HttpGet]
public IActionResult Create()
{
    return View();


}

[HttpPost]

public IActionResult Create(CreateCategoryViewModel input)
{
    if (ModelState.IsValid)
    {
        var category = new Category();
        category.Name = input.Name;
        category.CreatedAt = DateTime.UtcNow;
        category.CreatedBy = User.Identity?.Name ?? "System";
        category.UpdatedBy = User.Identity?.Name ?? "System";
        TempData["msg"] = "category created successfully";
        _db.Categories.Add(category);
        _db.SaveChanges();

        return RedirectToAction("Index");
    }

    return View(input);
}


[HttpGet]
public IActionResult update(int id)
{
  
  var category = _db.Categories.SingleOrDefault(x => x.Id == id && !x.IsDelete);
  if (category == null)
  {
      return NotFound();
  }

  var VM = new UpdateCategoryViewModel();
  VM.Id = category.Id;
  VM.Name = category.Name;

  return View(VM);
}
[HttpPost]
public IActionResult update(int id, UpdateCategoryViewModel input)
{

   if (ModelState.IsValid){
        var category = _db.Categories.FirstOrDefault(x => x.Id == id && !x.IsDelete);
        if (category == null)
        {
            return NotFound();
        }

        category.Name = input.Name;
        _db.Categories.Update(category);
        _db.SaveChanges();

        TempData["msg"] = "category updated successfully";
        return RedirectToAction("Index");
    }

    return View(input);
}
[HttpGet]
public IActionResult Delete(int id)
{
    var category = _db.Categories.SingleOrDefault(x => x.Id == id && !x.IsDelete);
    if (category == null)
    {
        return NotFound();
    }
    category.IsDelete = true;

    _db.Categories.Remove(category);
    _db.SaveChanges();

    TempData["msg"] = "category deleted successfully";
    return RedirectToAction("Index");
}
}}