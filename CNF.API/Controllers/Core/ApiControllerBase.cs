using Microsoft.AspNetCore.Mvc;

namespace CNF.API.Controllers.Core;

[ApiController]
[Route("api/sys/[controller]/[action]")]
public abstract class ApiControllerBase : ControllerBase
{
    
}