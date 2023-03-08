using AutoMapper;
using CNF.Common;
using CNF.Common.Core;
using CNF.Domain.Repository;
using CNF.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Logging;
using SqlSugar;

namespace CNF.API.Controllers.Core;

[SugarTable("Sys_User")]
public class User
{
    public int id { get; private set; }
}

public class UserControll:ApiControllerBase//UserControllerBase<User>
{
    private IBaseRepository<User> _repository;
    private ILogger<UserControll> _log;
    public UserControll(IBaseRepository<User> userRepository,ILogger<UserControll> log)
    {
        _repository = userRepository;
        _log = log;
    }
    
    
    [HttpPost]
    [AllowAnonymous]
    public async void SignIn()
    {
        User s = new();
        var loginModel = await _repository.GetListAsync();

        
        _log.LogError("日志类型阿斯顿");
        Console.WriteLine("SignIn --------");
        
        // for (int i = 0; i < loginModel.Count; i++)
        // {
        //     Console.WriteLine(loginModel[i].id);   
        // }

    }
}