using System.Security.Claims;
using AutoMapper;
using CNF.API.Controllers;
using CNF.Caches;
using CNF.Common;
using CNF.Common.Core;
using CNF.Domain.Data.Input.Sys;
using CNF.Domain.Data.Output.Sys;
using CNF.Domain.Entity;
using CNF.Domain.ValueObjects;
using CNF.Infrastructure;
using CNF.MVC.Common;
using CNF.Repository.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.IdentityModel.Tokens.Jwt;
using CNF.Common.Logger;
using CNF.Domain.ValueObjects.Enums.Sys;
using Microsoft.OpenApi.Extensions;

namespace CNF.MVC.Areas.Sys.Controllers;

[Area("sys")]
public class UserController : Controller
{
    private readonly IDistributedCache _cacheHelper;
    private readonly ICurrentUserContext _currentUserContext;
    private readonly IMapper _mapper;
    private readonly IBaseRepository<User> _userRepository;

    public UserController(IDistributedCache cacheHelper, IMapper mapper, ICurrentUserContext currentUserContext)
    {
        _cacheHelper = cacheHelper;
        _mapper = mapper;
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
    /// 登录
    /// </summary>  
    [HttpPost, AllowAnonymous]
    public async Task<ApiResult<LoginOutput>> Login([FromBody] LoginInput loginInput)
    {
        try
        {
            var rsaKey = _cacheHelper.Get<List<string>>($"{SysCacheKey.EncryLoginKey}:{loginInput?.NumberGuid}");
            if (rsaKey == null)
            {
                throw new ArgumentException("登录失败，请刷新浏览器再次登录!");
            }

            var ras = new RSACrypt(rsaKey[0], rsaKey[1]);
            loginInput.Password = ras.Decrypt(loginInput.Password);

            loginInput.Password = Md5Crypt.Encrypt(loginInput.Password);
            var loginModel = await _userRepository.GetModelAsync(d =>
                d.UserName.Equals(loginInput.UserName) && d.Password.Equals(loginInput.Password));
            if (loginModel?.Id == 0)
            {
                DebugLog.DatabaseLog(loginModel?.UserName, LoggerEnum.Login.GetDisplayName(),
                    $"{loginModel?.UserName}登陆失败，用户名或密码错误！",NLog.LogLevel.Info);
                return new ApiResult<LoginOutput>("用户名或密码错误", 500);
            }

            if (!loginInput.ConfirmLogin)
            {
                if (loginModel.IsLogin)
                {
                    return new ApiResult<LoginOutput>($"该用户【{loginInput.UserName}】已经登录，此时强行登录，其他地方会被挤下线！", 200);
                }
            }

            loginModel.ModifyLoginInfo();
            await _userRepository.UpdateAsync(loginModel);
            var result = _mapper.Map<LoginOutput>(loginModel);

            if (loginInput.ConfirmLogin)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                _cacheHelper.Remove($"{SysCacheKey.AuthMenu}:{_currentUserContext.Id}");
            }

            var identity = new ClaimsPrincipal(
                new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sid, result.Id.ToString()),
                    new Claim(ClaimTypes.Name, result.UserName),
                    new Claim(ClaimTypes.WindowsAccountName, result.UserName),
                    new Claim(ClaimTypes.UserData, result.LastLoginTime.ToString()),
                    // new Claim(ClaimTypes.MobilePhone,result.Mobile),
                    // new Claim("trueName",result.TrueName)
                }, CookieAuthenticationDefaults.AuthenticationScheme)
            );
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, identity,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddHours(24),
                    IsPersistent = true,
                    AllowRefresh = false
                });
            _cacheHelper.Remove($"{SysCacheKey.EncryLoginKey}:{loginInput.NumberGuid}");
            DebugLog.DatabaseLog(loginInput.UserName,LoggerEnum.Login.GetDisplayName(),$"登陆成功",NLog.LogLevel.Info);
            return new ApiResult<LoginOutput>(result);
        }
        catch (Exception exception)
        {
            ApiResult<LoginOutput> result = new ApiResult<LoginOutput>(msg: $"登陆失败！{exception.Message}");
            DebugLog.DatabaseLog(loginInput.UserName,LoggerEnum.Login.GetDisplayName(),$"登陆失败:{exception.Message}",NLog.LogLevel.Error,exception);
            return result;
        }
    }

    /// <summary>
    /// 获取验证码
    /// </summary>
    [HttpGet, AllowAnonymous]
    public FileResult OnGetVCode()
    {
        var vCode = VerifyCode.CreateRandomCode(4);
        HttpContext.Session.SetString("vCode", vCode);
        var img = VerifyCode.CreateRandomCodeImage(vCode);
        var bytes = img.ToArray();
        return File(bytes, "image/gif");
    }
}