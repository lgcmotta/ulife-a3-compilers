using System.Collections;

namespace Msh.StandardLibrary.Types;

public sealed class ListType : Variant<ListType>, IVariant, IEnumerable<IVariant>
{
    public ListType(IEnumerable<IVariant> items)
    {
        Items = (List<IVariant>)[..items];
    }

    public Kind Kind => Kind.List;

    public List<IVariant> Items { get; }

    public IVariant this[int index] => Items[index];

    public int Count => Items.Count;

    public static IVariant Empty => new ListType([]);

    public void Add(IVariant variant) => Items.Add(variant);

    public void RemoveAt(int index) => Items.RemoveAt(index);

    public void Insert(int index, IVariant variant) => Items.Insert(index, variant);

    public void Clear() => Items.Clear();

    public static ListType operator +(ListType left, ListType right) =>
        new([..left.Items, ..right.Items]);

    public static ListType operator -(ListType left, ListType right) =>
        new([..left.Items.Except(right.Items)]);

    public static bool operator ==(ListType? left, ListType? right)
        => left?.Equals(right) ?? right is null;

    public static bool operator !=(ListType? left, ListType? right)
        => !(left == right);

    public static bool operator <(ListType? left, ListType? right)
        => left is null || left.CompareTo(right) < 0;

    public static bool operator >(ListType? left, ListType? right)
        => left is not null && left.CompareTo(right) > 0;

    public static bool operator <=(ListType? left, ListType? right)
        => left is null || left.CompareTo(right) <= 0;

    public static bool operator >=(ListType? left, ListType? right)
        => left is not null && left.CompareTo(right) >= 0;

    public override int CompareTo(ListType? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (other is null)
        {
            return 1;
        }

        if (Items.Count > other.Items.Count)
        {
            return 1;
        }

        if (Items.Count < other.Items.Count)
        {
            return -1;
        }

        return Items.Select((variant, index) => variant.CompareTo(other[index])).FirstOrDefault(result => result != 0);
    }

    int IComparable<IVariant>.CompareTo(IVariant? other)
    {
        return other is not ListType listType
            ? 1
            : CompareTo(listType);
    }

    public override bool Equals(ListType? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other is null || Items.Count != other.Items.Count)
        {
            return false;
        }

        return Items.Select((t, index) => t.Equals(other.Items[index])).All(result => result);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is ListType listType && this.Equals(listType);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var item in Items)
        {
            hash.Add(item);
        }

        return hash.ToHashCode();
    }

    public IEnumerator<IVariant> GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}