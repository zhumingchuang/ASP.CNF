// using CNF.Repository.Interface;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Logging;
//
// namespace CNF.Infrastructure.TimedTask;
//
// public class TimedBackgroundService : BackgroundService
// {
//     private readonly ILogger _logger;
//     private Timer _timer;
//
//     private readonly ICurrentUserContext _currentUserContext;
//     private readonly IBaseRepository<User> _userRepository;
//
//     public TimedBackgroundService(ILogger<TimedBackgroundService> logger, IServiceProvider serviceProvider)
//     {
//         _logger = logger;
//         _currentUserContext = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ICurrentUserContext>();
//         _userRepository = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IBaseRepository<User>>();
//     }
//
//     protected override Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
//         return Task.CompletedTask;
//     }
//
//     private void DoWork(object state)
//     {
//         try
//         {
//             if (!_currentUserContext.IsAuthenticated())
//             {
//                 var userList = _userRepository.GetList(d => d.IsDeleted == false && d.IsLogin == true);
//                 foreach (var item in userList)
//                 {
//                     item.IsLogin = false;
//                     item.NotifyModified();
//                     _userRepository.Update(item);
//                 }
//             }
//         }
//         catch
//         {
//
//
//         }
//
//         //给指定人推送消息	
//         //  _hubContext.Clients.All.SendAsync("ReceiveMessage",  1);
//
//         // _logger.LogInformation($"Hello World! - {DateTime.Now}");
//     }
//
//     public override void Dispose()
//     {
//         base.Dispose();
//         _timer?.Dispose();
//     }
// }