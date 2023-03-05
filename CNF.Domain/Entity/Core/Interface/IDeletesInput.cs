namespace CNF.Domain.Entity.Core;

public interface IDeletesInput
{
    public List<int> Ids { get; set; }
}