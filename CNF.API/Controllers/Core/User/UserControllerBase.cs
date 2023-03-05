using AutoMapper;
using CNF.Repository.Interface;
using Microsoft.Extensions.Caching.Distributed;

namespace CNF.API.Controllers.Core;

/// <summary>
/// 用户控制器基类
/// </summary>
public class UserControllerBase<TUser> : ApiControllerBase, IUser
    where TUser : class
{
    private readonly IBaseRepository<TUser> _userRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cacheHelper;

    public UserControllerBase(IBaseRepository<TUser> userRepository, IMapper mapper, IDistributedCache cacheHelper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _cacheHelper = cacheHelper;
    }
}