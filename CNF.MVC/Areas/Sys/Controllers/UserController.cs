using AutoMapper;
using CNF.Caches;
using CNF.Common.Core;
using CNF.Domain.ValueObjects;
using CNF.Infrastructure;
using CNF.MVC.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace CNF.MVC.Areas.Sys.Controllers;

[Area("sys")]
public class UserController : Controller
{
    private readonly IDistributedCache _cacheHelper;
    private readonly ICurrentUserContext _currentUserContext;
    private readonly IMapper _mapper;

    public UserController(IDistributedCache cacheHelper, ICurrentUserContext currentUserContext)
    {
        _cacheHelper = cacheHelper;
        // _mapper = mapper;
        _currentUserContext = currentUserContext;
    }

    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> Login()
    {
        var rsaKey = RSACrypt.GetKey();
        var number = Guid.NewGuid().ToString();
        if (rsaKey.Count <= 0 || rsaKey == null)
        {
            throw new ArgumentNullException("获取登录的公钥和私钥为空");
        }

        ViewBag.RsaKey = rsaKey[0];
        ViewBag.Number = number;
        //获得公钥和私钥
        _cacheHelper.Set($"{SysCacheKey.EncryLoginKey}:{number}", rsaKey);
        var value = await SysSetting.ReadAsync();
        return View(value);
    }

    /// <summary>
    /// 获取验证码
    /// </summary>
    [HttpGet, AllowAnonymous]
    public FileResult OnGetVCode()
    {
        var vCode = VerifyCode.CreateRandomCode(4);
        HttpContext.Session.SetString("vcode", vCode);
        var img = VerifyCode.CreateRandomCodeImage(vCode);
        var bytes = img.ToArray();
        return File(bytes, "image/gif");
    }
}