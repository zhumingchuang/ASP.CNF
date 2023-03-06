using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace CNF.TransferJob;

public class BackgroundRunService:IBackgroundRunService
{
            private readonly SemaphoreSlim _slim;
        private readonly ConcurrentQueue<LambdaExpression> queue;
        private readonly IServiceProvider _serviceProvider;
        public BackgroundRunService(IServiceProvider serviceProvider)
        {
            _slim = new SemaphoreSlim(1);
            _serviceProvider = serviceProvider;
            queue = new ConcurrentQueue<LambdaExpression>();
        }
        public async Task Execute(CancellationToken cancellationToken)
        {
            try
            {
                await _slim.WaitAsync(cancellationToken);
                if (queue.TryDequeue(out var job))
                {
                    using (var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var action = job.Compile();
                        var isTask = action.Method.ReturnType == typeof(Task);
                        var parameters = job.Parameters;
                        var pars = new List<object>();
                        if (parameters.Any())
                        {
                            var type = parameters[0].Type;
                            var param = scope.ServiceProvider.GetRequiredService(type);
                            pars.Add(param);
                        }
                        if (isTask)
                        {
                            await (Task)action.DynamicInvoke(pars.ToArray());
                        }
                        else
                        {
                            action.DynamicInvoke(pars.ToArray());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
        public void Transfer<T>(Expression<Func<T, Task>> expression)
        {
            queue.Enqueue(expression);
            _slim.Release();
        }
        public void Transfer(Expression<Action> expression)
        {
            queue.Enqueue(expression);
            _slim.Release();
        }
}