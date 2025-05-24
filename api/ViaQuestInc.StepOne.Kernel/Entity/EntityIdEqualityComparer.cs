namespace ViaQuestInc.StepOne.Kernel.Entity;

public class EntityIdEqualityComparer<TId>  : IEqualityComparer<EntityBase<TId>>
{
    public bool Equals(EntityBase<TId> x, EntityBase<TId> y)
    {
        if (ReferenceEquals(x, y)) return true;
        
        if (ReferenceEquals(x, null)) return false;
        
        if (ReferenceEquals(y, null)) return false;
        
        if (x.GetType() != y.GetType()) return false;
        
        return Equals(x.Id, y.Id);
    }

    public int GetHashCode(EntityBase<TId> obj)
    {
        return EqualityComparer<TId>.Default.GetHashCode(obj.Id);
    }
}