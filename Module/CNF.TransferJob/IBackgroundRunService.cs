using System.Linq.Expressions;

namespace CNF.TransferJob;

public interface IBackgroundRunService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Execute(CancellationToken cancellationToken);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="expression"></param>
    /// <typeparam name="T"></typeparam>
    void Transfer<T>(Expression<Func<T, Task>> expression);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="expression"></param>
    void Transfer(Expression<Action> expression);
}