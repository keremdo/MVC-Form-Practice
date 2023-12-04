using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FormsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormsApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index(string query, string category)
    {
        var _productList = Repository.Products;
        var _categories = Repository.Categories;
        if(!String.IsNullOrEmpty(query))
        {
            _productList = _productList.Where(p => p.Name!.ToLower().Contains(query)).ToList();
        }
        if(!String.IsNullOrEmpty(category) && category != "0")
        {
            _productList = _productList.Where(p => p.CategoryId == int.Parse(category)).ToList();
        }

        
        var model = new ProductViewModel()
        {
            Products = _productList,
            Categories = _categories,
            SelectedCategory = category
        };
        return View(model);
    }
    [HttpGet]
    public IActionResult CreateProduct()
    {
        ViewBag.Categories =  new SelectList(Repository.Categories,"Id","Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product,IFormFile imageFile)
    {   
        var extension ="";
        if(imageFile != null)
        {   var allowExtension = new [] {".jpg",".jpeg",".png"};
            extension = Path.GetExtension(imageFile.FileName);
            if(!allowExtension.Contains(extension))
            {
                ModelState.AddModelError("","Geçerli Bir Resim Girin");
            }
        }

        if(ModelState.IsValid)
        {
            var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
            var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",randomFileName);
            using(var stream = new FileStream(path,FileMode.Create))
            {
                await imageFile!.CopyToAsync(stream);
            }
            product.Image=randomFileName;
            Repository.Create(product);
            return RedirectToAction("Index","Home");
        }
        ViewBag.Categories =  new SelectList(Repository.Categories,"Id","Name");
        return View();
        
    }
    // ürün güncelle 
    public IActionResult EditProduct(int? id)
    {
        if(id == null && id == 0)
        {
            return NotFound();
        }
        var entitiy = Repository.Products.FirstOrDefault(p => p.Id==id);
        if(entitiy == null)
        {
            return NotFound();
        }
        ViewBag.Categories =  new SelectList(Repository.Categories,"Id","Name");
        return View(entitiy);
    }

    [HttpPost]
    public async Task<IActionResult> EditProduct(int id,Product product,IFormFile? imageFile)
    {
        if(id != product.Id)
        {
            return NotFound();
        }
        if(ModelState.IsValid)
        {      if(imageFile != null)
            {
                var extension = Path.GetExtension(imageFile.FileName);
                var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",randomFileName);

                using(var stream = new FileStream(path,FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                product.Image=randomFileName;            
            }
            Repository.Edit(product);
            return RedirectToAction("Index"); 
        }
        ViewBag.Categories =  new SelectList(Repository.Categories,"Id","Name");
        return View(product);
    }
    [HttpGet]
    public IActionResult DeleteProduct(int? deletedId)
    {
        if(deletedId == null)
        {
            return NotFound();
        }
        var entitiy = Repository.Products.FirstOrDefault(p => p.Id == deletedId);
        if(entitiy == null)
        {
            return NotFound();
        }
        return View("DeleteConfirm");
    }

    [HttpPost]
    public IActionResult DeleteProduct(int id, int prouctId)
    {
        if(id != prouctId)
        {
            return NotFound();
        }
        var entitiy = Repository.Products.FirstOrDefault(p => p.Id == prouctId);
        if(entitiy == null)
        {
            return NotFound();
        }
        Repository.Delete(entitiy);
        return RedirectToAction("Index");

    }

    
    

}
