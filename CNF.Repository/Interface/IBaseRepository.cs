using System.Linq.Expressions;

namespace CNF.Repository.Interface;

/// <summary>
/// 数据库操作接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBaseRepository<T> where T : class
{
    #region 同步

    /// <summary>
    /// 添加一条数据
    /// </summary>
    int Add(T param);

    /// <summary>
    /// 批量添加数据
    /// </summary>
    int AddList(List<T> param);

    /// <summary>
    /// 获得列表
    /// </summary>
    List<T> GetList(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderExpression, bool isAsc);

    /// <summary>
    /// 获得列表
    /// </summary>
    List<T> GetList(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderExpression, bool isAsc, int take);

    /// <summary>
    /// 获得列表
    /// </summary>
    List<T> GetList(Expression<Func<T, bool>> whereExpression);

    /// <summary>
    /// 获得列表
    /// </summary>
    List<T> GetList();

    /// <summary>
    /// 获得列表——分页
    /// </summary>
    Page<T> GetPages(int page, int limit);

    /// <summary>
    /// 分页
    /// </summary>
    Page<T> GetPages(int page, int limit, Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderExpression, bool isAsc);

    /// <summary>
    /// 获得一条数据
    /// </summary>
    T GetModel(string param);

    /// <summary>
    /// 获得一条数据
    /// </summary>
    T GetModel(Expression<Func<T, bool>> whereExpression);

    /// <summary>
    /// 修改一条数据
    /// </summary>
    int Update(T param);

    /// <summary>
    /// 修改一条数据
    /// </summary>
    int Update(List<T> param);

    /// <summary>
    /// 修改一条数据
    /// </summary>
    int Update(Expression<Func<T, T>> columnsExpression, Expression<Func<T, bool>> whereExpression);

    /// <summary>
    /// 修改一条数据
    /// </summary>
    int Update(Expression<Func<T, bool>> columnsExpression, Expression<Func<T, bool>> whereExpression);

    /// <summary>
    /// 更新整体，指定忽略个别字段
    /// </summary>
    int Update(T param, Expression<Func<T, object>> ignoreExpression);

    /// <summary>
    /// 删除一条或多条数据
    /// </summary>
    int Delete(List<string> param);

    /// <summary>
    /// 删除一条或多条数据
    /// </summary>
    int Delete(Expression<Func<T, bool>> whereExpression);

    /// <summary>
    /// 查询条数
    /// </summary>
    int Count(Expression<Func<T, bool>> whereExpression);

    /// <summary>
    /// 是否存在
    /// </summary>
    bool IsExist(Expression<Func<T, bool>> whereExpression);

    #endregion

    #region 异步

    /// <summary>
    /// 添加一条数据
    /// </summary>
    Task<int> AddAsync(T param);

    /// <summary>
    /// 批量添加数据
    /// </summary>
    Task<int> AddListAsync(List<T> param);

    /// <summary>
    /// 获得列表
    /// </summary>
    Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderExpression, bool isAsc);

    /// <summary>
    /// 获得列表
    /// </summary>
    Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderExpression, bool isAsc, int take);

    /// <summary>
    /// 获得列表
    /// </summary>
    Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression);


    /// <summary>
    /// 获得列表
    /// </summary>
    Task<List<T>> GetListAsync();

    /// <summary>
    /// 分页
    /// </summary>
    Task<Page<T>> GetPagesAsync(int page, int limit);

    Task<Page<T>> GetPagesAsync(int page, int limit, Expression<Func<T, object>> orderExpression, bool isAsc);

    /// <summary>
    /// 分页
    /// </summary>
    Task<Page<T>> GetPagesAsync(int page, int limit, Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderExpression, bool isAsc);

    /// <summary>
    /// 获得一条数据
    /// </summary>
    Task<T> GetModelAsync(string param);

    /// <summary>
    /// 获得一条数据
    /// </summary>
    Task<T> GetModelAsync(Expression<Func<T, bool>> whereExpression);

    /// <summary>
    /// 修改一条数据
    /// </summary>
    Task<int> UpdateAsync(T param);

    /// <summary>
    /// 修改一条数据
    /// </summary>
    Task<int> UpdateAsync(List<T> param);

    /// <summary>
    /// 修改一条数据，可用作假删除
    /// </summary>
    Task<int> UpdateAsync(Expression<Func<T, T>> columnsExpression, Expression<Func<T, bool>> whereExpression);

    /// <summary>
    /// 修改一条数据，可用作假删除
    /// </summary>
    Task<int> UpdateAsync(Expression<Func<T, bool>> columnsExpression, Expression<Func<T, bool>> whereExpression);

    /// <summary>
    /// 更新整体，指定忽略个别字段
    /// </summary>
    Task<int> UpdateAsync(T param, Expression<Func<T, object>> ignoreExpression);

    /// <summary>
    /// 删除一条或多条数据
    /// </summary>
    Task<int> DeleteAsync(List<int> param);

    /// <summary>
    /// 删除一条或多条数据
    /// </summary>
    Task<int> DeleteAsync(Expression<Func<T, bool>> whereExpression);

    /// <summary>
    /// 查询Count
    /// </summary>
    Task<int> CountAsync(Expression<Func<T, bool>> whereExpression);


    /// <summary>
    /// 是否存在
    /// </summary>
    Task<bool> IsExistAsync(Expression<Func<T, bool>> whereExpression);

    /// <summary>
    /// 多表查询
    /// </summary>
    public Task<List<TResult>> QueryMuch<T1, T2, T3, TResult>(
        Expression<Func<T1, T2, T3, object[]>> joinExpression,
        Expression<Func<T1, T2, T3, TResult>> selectExpression,
        Expression<Func<T1, T2, T3, bool>> whereExpression = null) where T1 : class, new();

    #endregion
}