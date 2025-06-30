using System.Text.Json;

namespace ViaQuestInc.StepOne.Core.Data.Entity;

public abstract class EntityBase<TId>
{
    public virtual TId Id { get; set; }

    protected virtual object Actual => this;

    public override bool Equals(object obj)
    {
        if (obj is not EntityBase<TId> other) return false;

        if (ReferenceEquals(this, other)) return true;

        if (Actual.GetType() != other.Actual.GetType()) return false;

        if (Id.Equals(default(TId)) || other.Id.Equals(default(TId))) return false;

        return Id.Equals(other.Id);
    }

    public static bool operator ==(EntityBase<TId> a, EntityBase<TId> b)
    {
        if (a is null && b is null) return true;

        if (a is null || b is null) return false;

        return a.Equals(b);
    }

    public static bool operator !=(EntityBase<TId> a, EntityBase<TId> b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (Actual.GetType()
            .ToString() + Id).GetHashCode();
    }

    public T Copy<T>()
        where T : EntityBase<TId>
    {
        return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(this as T));
    }

    public bool IsEquivalentTo<T>(T other)
        where T : EntityBase<TId>
    {
        if (other == null || !other.Id.Equals(Id)) return false;

        return ToJson()
            .Equals(other.ToJson());
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this, GetType());
    }
}