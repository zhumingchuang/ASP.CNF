using System.Linq.Expressions;
using CNF.Repository.Extensions;
using CNF.Repository.Interface;
using SqlSugar;

namespace CNF.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
{
    private readonly ISqlSugarClient _db;

    public BaseRepository(ISqlSugarClient db)
    {
        this._db = db;
    }

    #region 同步

    /// <summary>
    /// 添加一条数据
    /// </summary>
    public int Add(T param)
    {
        return _db.Insertable(param).ExecuteReturnIdentity();
    }

    /// <summary>
    /// 批量添加数据
    /// </summary>
    public int AddList(List<T> param)
    {
        return _db.Insertable(param).ExecuteCommand();
    }

    /// <summary>
    /// 获得一条数据
    /// </summary>
    public T GetModel(Expression<Func<T, bool>> whereExpression)
    {
        return _db.Queryable<T>().WhereIF(whereExpression != null, whereExpression).First() ?? new T() { };
    }

    /// <summary>
    /// 获得一条数据
    /// </summary>
    public T GetModel(string param)
    {
        return _db.Queryable<T>().Where(param).First() ?? new T() { };
    }

    /// <summary>
    /// 分页
    /// </summary>
    public Page<T> GetPages(int page, int limit)
    {
        return _db.Queryable<T>().ToPage(page, limit);
    }

    /// <summary>
    /// 分页
    /// </summary>
    public Page<T> GetPages(int page, int limit, Expression<Func<T, bool>> whereExpression,
        Expression<Func<T, object>> orderExpression, bool isAsc)
    {
        var query = _db.Queryable<T>()
            .WhereIF(whereExpression != null, whereExpression)
            .OrderBy(orderExpression, isAsc == true ? OrderByType.Asc : OrderByType.Desc);
        return query.ToPage(page, limit);
    }

    /// <summary>
    /// 获得列表
    /// </summary>
    public List<T> GetList(Expression<Func<T, bool>> whereExpression,
        Expression<Func<T, object>> orderExpression, bool isAsc)
    {
        var query = _db.Queryable<T>()
            .WhereIF(whereExpression != null, whereExpression)
            .OrderBy(orderExpression, isAsc == true ? OrderByType.Asc : OrderByType.Desc);
        return query.ToList();
    }

    /// <summary>
    /// 获得列表
    /// </summary>
    public List<T> GetList(Expression<Func<T, bool>> whereExpression,
        Expression<Func<T, object>> orderExpression, bool isAsc, int take)
    {
        var query = _db.Queryable<T>()
            .WhereIF(whereExpression != null, whereExpression).Take(take)
            .OrderBy(orderExpression, isAsc == true ? OrderByType.Asc : OrderByType.Desc);
        return query.ToList();
    }

    /// <summary>
    /// 获得列表
    /// </summary>
    public List<T> GetList(Expression<Func<T, bool>> whereExpression)
    {
        return _db.Queryable<T>().WhereIF(whereExpression != null, whereExpression).ToList();
    }

    /// <summary>
    /// 获得列表
    /// </summary>
    /// <returns></returns>
    public List<T> GetList()
    {
        return _db.Queryable<T>().ToList();
    }

    /// <summary>
    /// 修改一条数据
    /// </summary>
    public int Update(T param)
    {
        return _db.Updateable(param).ExecuteCommand();
    }

    /// <summary>
    /// 批量修改
    /// </summary>
    public int Update(List<T> param)
    {
        return _db.Updateable(param).ExecuteCommand();
    }

    /// <summary>
    /// 修改一条数据
    /// </summary>
    public int Update(Expression<Func<T, T>> columnsExpression,
        Expression<Func<T, bool>> whereExpression)
    {
        return _db.Updateable<T>().SetColumns(columnsExpression).Where(whereExpression).ExecuteCommand();
    }

    /// <summary>
    /// 修改一条数据
    /// </summary>
    public int Update(Expression<Func<T, bool>> columnsExpression,
        Expression<Func<T, bool>> whereExpression)
    {
        return _db.Updateable<T>().SetColumns(columnsExpression).Where(whereExpression).ExecuteCommand();
    }

    /// <summary>
    /// 更新整体，指定忽略个别字段
    /// </summary>
    public int Update(T param, Expression<Func<T, object>> ignoreExpression)
    {
        return _db.Updateable(param).IgnoreColumns(ignoreExpression).ExecuteCommand();
    }

    /// <summary>
    /// 删除一条或多条数据
    /// </summary>
    public int Delete(List<string> param)
    {
        return _db.Deleteable<T>().In(param.ToArray()).ExecuteCommand();
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    public int Delete(Expression<Func<T, bool>> whereExpression)
    {
        return _db.Deleteable<T>().Where(whereExpression).ExecuteCommand();
    }

    /// <summary>
    /// 查询条数
    /// </summary>
    public int Count(Expression<Func<T, bool>> whereExpression)
    {
        return _db.Queryable<T>().Count(whereExpression);
    }

    /// <summary>
    /// 是否存在
    /// </summary>
    public bool IsExist(Expression<Func<T, bool>> whereExpression)
    {
        return _db.Queryable<T>().Any(whereExpression);
    }

    #endregion

    #region 异步

    /// <summary>
    /// 添加一条数据
    /// </summary>
    public async Task<int> AddAsync(T param)
    {
        return await _db.Insertable(param).ExecuteReturnIdentityAsync();
    }

    /// <summary>
    /// 批量添加数据
    /// </summary>
    public async Task<int> AddListAsync(List<T> param)
    {
        return await _db.Insertable(param).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获得一条数据
    /// </summary>
    public async Task<T> GetModelAsync(Expression<Func<T, bool>> whereExpression)
    {
        return await _db.Queryable<T>().WhereIF(whereExpression != null, whereExpression).FirstAsync() ?? new T() { };
    }

    /// <summary>
    /// 获得一条数据
    /// </summary>
    public async Task<T> GetModelAsync(string param)
    {
        return await _db.Queryable<T>().Where(param).FirstAsync() ?? new T() { };
    }

    /// <summary>
    /// 分页
    /// </summary>
    public async Task<Page<T>> GetPagesAsync(int page, int limit)
    {
        return await _db.Queryable<T>().ToPageAsync(page, limit);
    }

    public async Task<Page<T>> GetPagesAsync(int page, int limit, Expression<Func<T, object>> orderExpression,
        bool isAsc)
    {
        return await _db.Queryable<T>().OrderBy(orderExpression, isAsc == true ? OrderByType.Asc : OrderByType.Desc)
            .ToPageAsync(page, limit);
    }

    /// <summary>
    /// 分页
    /// </summary>
    public async Task<Page<T>> GetPagesAsync(int page, int limit, Expression<Func<T, bool>> whereExpression,
        Expression<Func<T, object>> orderExpression, bool isAsc)
    {
        var query = _db.Queryable<T>()
            .WhereIF(whereExpression != null, whereExpression)
            .OrderBy(orderExpression, isAsc == true ? OrderByType.Asc : OrderByType.Desc);
        return await query.ToPageAsync(page, limit);
    }


    /// <summary>
    /// 获得列表
    /// </summary>
    public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression,
        Expression<Func<T, object>> orderExpression, bool isAsc)
    {
        var query = _db.Queryable<T>()
            .WhereIF(whereExpression != null, whereExpression)
            .OrderBy(orderExpression, isAsc == true ? OrderByType.Asc : OrderByType.Desc);
        return await query.ToListAsync();
    }

    /// <summary>
    /// 获得列表
    /// </summary>
    public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression,
        Expression<Func<T, object>> orderExpression, bool isAsc, int take)
    {
        var query = _db.Queryable<T>()
            .WhereIF(whereExpression != null, whereExpression).Take(take)
            .OrderBy(orderExpression, isAsc == true ? OrderByType.Asc : OrderByType.Desc);
        return await query.ToListAsync();
    }

    /// <summary>
    /// 获得列表
    /// </summary>
    public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression)
    {
        return await _db.Queryable<T>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
    }

    /// <summary>
    /// 获得列表
    /// </summary>
    public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression,
        Expression<Func<T, T>> selectExpression)
    {
        return await _db.Queryable<T>().WhereIF(whereExpression != null, whereExpression).Select(selectExpression)
            .ToListAsync();
    }

    /// <summary>
    /// 获得列表
    /// </summary>
    public async Task<List<T>> GetListAsync()
    {
        return await _db.Queryable<T>().ToListAsync();
    }


    /// <summary>
    /// 修改一条数据
    /// </summary>
    public async Task<int> UpdateAsync(T param)
    {
        return await _db.Updateable(param).ExecuteCommandAsync();
    }

    /// <summary>
    /// 批量修改
    /// </summary>
    public async Task<int> UpdateAsync(List<T> param)
    {
        return await _db.Updateable(param).ExecuteCommandAsync();
    }

    /// <summary>
    /// 修改一条数据，可用作假删除
    /// </summary>
    public async Task<int> UpdateAsync(Expression<Func<T, T>> columnsExpression,
        Expression<Func<T, bool>> whereExpression)
    {
        return await _db.Updateable<T>().SetColumns(columnsExpression).Where(whereExpression).ExecuteCommandAsync();
    }

    /// <summary>
    /// 修改一条数据，可用作假删除
    /// </summary>
    public async Task<int> UpdateAsync(Expression<Func<T, bool>> columnsExpression,
        Expression<Func<T, bool>> whereExpression)
    {
        return await _db.Updateable<T>().SetColumns(columnsExpression).Where(whereExpression).ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新整体，指定忽略个别字段
    /// </summary>
    public async Task<int> UpdateAsync(T param, Expression<Func<T, object>> ignoreExpression)
    {
        return await _db.Updateable(param).IgnoreColumns(ignoreExpression).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除一条或多条数据
    /// </summary>
    public async Task<int> DeleteAsync(List<int> param)
    {
        return await _db.Deleteable<T>().In(param.ToArray()).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    public async Task<int> DeleteAsync(Expression<Func<T, bool>> whereExpression)
    {
        return await _db.Deleteable<T>().Where(whereExpression).ExecuteCommandAsync();
    }

    /// <summary>
    /// 查询多少条
    /// </summary>
    public async Task<int> CountAsync(Expression<Func<T, bool>> whereExpression)
    {
        return await _db.Queryable<T>().CountAsync(whereExpression);
    }

    /// <summary>
    /// 是否存在
    /// </summary>
    public async Task<bool> IsExistAsync(Expression<Func<T, bool>> whereExpression)
    {
        return await _db.Queryable<T>().AnyAsync(whereExpression);
    }

    /// <summary>
    /// 多表查询
    /// </summary>
    public async Task<List<TResult>> QueryMuch<T1, T2, T3, TResult>(
        Expression<Func<T1, T2, T3, object[]>> joinExpression,
        Expression<Func<T1, T2, T3, TResult>> selectExpression,
        Expression<Func<T1, T2, T3, bool>> whereExpression = null) where T1 : class, new()
    {
        if (whereExpression == null)
        {
            return await _db.Queryable(joinExpression).Select(selectExpression).ToListAsync();
        }

        return await _db.Queryable(joinExpression).Where(whereExpression).Select(selectExpression).ToListAsync();
    }

    #endregion
}