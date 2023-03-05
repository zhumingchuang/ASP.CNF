using SqlSugar;

namespace CNF.Domain.Entity.Core;

public class BaseEntity : IEntity, IHasModifyTime, IHasCreateTime, IHasDeleteTime, ISoftDelete
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; protected set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? ModifyTime { get; protected set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; protected set; } = DateTime.Now;

    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime? DeleteTime { get; set; }

    /// <summary>
    /// 软删除
    /// </summary>
    public void SoftDelete()
    {
        this.IsDeleted = true;
        this.DeleteTime = DateTime.Now;
    }
}