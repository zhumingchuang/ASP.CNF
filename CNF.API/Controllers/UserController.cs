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
using CNF.Infrastructure;
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
    private readonly ICurrentUserContext _currentUserContext;


    public UserController(IBaseRepository<User> userRepository, ICurrentUserContext currentUserContext, IMapper mapper,
        IDistributedCache cacheHelper)
    {
        _mapper = mapper;
        _cacheHelper = cacheHelper;
        _userRepository = userRepository;
        _currentUserContext = currentUserContext;
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
        User loginModel;
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
                return new ApiResult<LoginOutput>($"该用户【{loginInput.UserName}】已经登录，此时强行登录，其他地方会被挤下线！",
                    ConstStatusCode.Success);
            }
        }

        loginModel.ModifyLoginInfo();
        await _userRepository.UpdateAsync(loginModel);
        var result = _mapper.Map<LoginOutput>(loginModel);
        return new ApiResult<LoginOutput>(result);
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    public async Task<ApiResult> AddUser([FromBody] UserRegisterInput input)
    {
        var model = await _userRepository.GetModelAsync(d => !d.IsDeleted && d.UserName.Equals(input.UserName));
        if (model != null && model.Id > 0)
        {
            return new ApiResult($"已经存在[{input.UserName}]该用户名了");
        }
        var userModel = _mapper.Map<User>(input);
        userModel.CreateUser();
        var i = await _userRepository.AddAsync(userModel);
        return i > 0 ? new ApiResult(i) : new ApiResult("用户添加失败！");
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    public async Task<ApiResult> ModfiyPwd([FromBody] ModifyPwdInput input)
    {
        input.ModifyPassword(_currentUserContext.Id);
        var model = await _userRepository.GetModelAsync(d => d.Id == input.Id && d.IsDeleted == false);
        if (model?.Id < 0)
        {
            throw new ArgumentException("用户信息为空");
        }

        model.IsEquaPassword(input.OldPassword, input.ConfirmPassword);
        var i = await _userRepository.UpdateAsync(model);
        return i > 0 ? new ApiResult(i) : new ApiResult("用户密码修改失败！");
    }

    /// <summary>
    /// 软删除
    /// </summary>
    public async void SoftDelete()
    {
    }
}