namespace CNF.Domain.Entity.Core;

/// <summary>
/// 软删除
/// </summary>
public interface ISoftDelete
{
    bool IsDeleted { get; }
    void SoftDelete();
}