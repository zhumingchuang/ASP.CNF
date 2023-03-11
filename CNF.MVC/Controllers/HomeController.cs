using System.Diagnostics;
using CNF.Domain.ValueObjects;
using CNF.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using CNF.MVC.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using SqlSugar;

namespace CNF.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ICurrentUserContext _currentUserContext;
    private readonly ISqlSugarClient _db;

    public HomeController(ISqlSugarClient db, ICurrentUserContext currentUserContext)
    {
        _db = db;
        _currentUserContext = currentUserContext;
    }

    [Route("/")]
    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        if (_currentUserContext.IsAuthenticated())
        {
            Console.WriteLine("asdasd");
            // await _menuRepository.GetCurrentAuthMenus(_currentUserContext.Id);
        }
        var value = await SysSetting.ReadAsync();
        return View(value);
    }
    
    // public async Task<IActionResult> Report()
    // {
    //     var articleCount = await _db.Queryable<Article>().Where(d => !d.IsDeleted).CountAsync();
    //     var columnCount = await _db.Queryable<Column>().Where(d => !d.IsDeleted).CountAsync();
    //     var goodsCount = await _db.Queryable<Goods>().Where(d => !d.IsDeleted).CountAsync();
    //     var categoryCount = await _db.Queryable<Category>().Where(d => !d.IsDeleted).CountAsync();
    //     var orderCount = await _db.Queryable<Order>().Where(d => !d.IsDeleted).CountAsync();
    //     ViewBag.articleCount = articleCount;
    //     ViewBag.columnCount = columnCount;
    //     ViewBag.goodsCount = goodsCount;
    //     ViewBag.categoryCount = categoryCount;
    //     ViewBag.orderCount = orderCount;
    //     return View();
    // }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}