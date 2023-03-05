namespace CNF.Domain.Entity.Core;

public interface IHasDeleteTime
{
    DateTime? DeleteTime { get; }
}