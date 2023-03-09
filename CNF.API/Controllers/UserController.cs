using AutoMapper;
using CNF.Caches;
using CNF.Common;
using CNF.Common.Core;
using CNF.Domain.Entity;
using CNF.API.Controllers.Core;
using CNF.Common.Logger;
using CNF.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using CNF.Domain.Data.Input.Sys;
using CNF.Domain.Data.Output.Sys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;

namespace CNF.API.Controllers;

/// <summary>
/// 用户控制器
/// </summary>
public class UserController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cacheHelper;
    private readonly IBaseRepository<User> _userRepository;


    public UserController(IBaseRepository<User> userRepository, IMapper mapper, IDistributedCache cacheHelper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _cacheHelper = cacheHelper;
    }

    /// <summary>
    /// 获取登录信息
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public ApiResult LoadLoginInfo()
    {
        var rsaKey = RSACrypt.GetKey();
        var number = Guid.NewGuid().ToString();
        if (rsaKey.Count <= 0 || rsaKey == null)
        {
            throw new ArgumentNullException("获取登录的公钥和私钥为空");
        }

        _cacheHelper.Set(SysCacheKey.LoginRsaCrypt + number, rsaKey);
        return new ApiResult(data: new { RsaKey = rsaKey, Number = number });
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <returns></returns>
    public async Task<ApiResult<LoginOutput>> SignIn([FromBody] LoginInput loginInput)
    {
        var rsaKey = _cacheHelper.Get<List<string>>(SysCacheKey.LoginRsaCrypt + loginInput.NumberGuid);
        if (rsaKey == null)
        {
            return new ApiResult<LoginOutput>("登录失败，请刷新浏览器再次登录!");
        }

        //解密
        var ras = new RSACrypt(rsaKey[0], rsaKey[1]);
        loginInput.Password = ras.Decrypt(loginInput.Password);
        loginInput.Password = Md5Crypt.Encrypt(loginInput.Password);

        var identityType = IdentityTypeHelp.GetIdentityType(loginInput.UserName);
        Domain.Entity.User loginModel;
        if (identityType == EIdentityType.Phone)
        {
            //手机登录
            loginModel = new User();
        }
        else if (identityType == EIdentityType.Email)
        {
            //邮箱登录
            loginModel = new User();
        }
        else
        {
            //用户名登录
            loginModel = await _userRepository.GetModelAsync(d =>
                d.UserName.Equals(loginInput.UserName) && d.Password.Equals(loginInput.Password));

            if (loginModel.Id == 0)
            {
                return new ApiResult<LoginOutput>("用户名或密码错误", ConstStatusCode.InternalError);
            }
        }

        if (!loginInput.ConfirmLogin)
        {
            if (loginModel.IsDeleted)
            {
                return new ApiResult<LoginOutput>($"该用户【{loginInput.UserName}】已经登录，此时强行登录，其他地方会被挤下线！", ConstStatusCode.Success);
            }
        }
        
        loginModel.ModifyLoginInfo();
        await _userRepository.UpdateAsync(loginModel);
        var result = _mapper.Map<LoginOutput>(loginModel);
        return new ApiResult<LoginOutput>(result);
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    public async void ModfiyPwd()
    {
        
    }
    
    /// <summary>
    /// 软删除
    /// </summary>
    public async void SoftDelete()
    {

    }
}