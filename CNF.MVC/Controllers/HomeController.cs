using System.Diagnostics;
using CNF.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using CNF.MVC.Models;
using Google.Protobuf.WellKnownTypes;
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

    public async Task<IActionResult> Index()
    {
        if (_currentUserContext.IsAuthenticated())
        {
            // await _menuRepository.GetCurrentAuthMenus(_currentUserContext.Id);
        }

        return View();
    }

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