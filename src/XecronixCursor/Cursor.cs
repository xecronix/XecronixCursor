namespace XecronixCursor;

public sealed class Cursor<T>
{
    private readonly IReadOnlyList<T> _items;

    public int Position { get; private set; }

    public int Count => _items.Count;

    public T Current => _items[Position];

    public Cursor(IReadOnlyList<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        if (items.Count == 0)
            throw new ArgumentException("Sequence must not be empty.", nameof(items));

        _items = items;
        Position = 0;
    }

    public Cursor<T> FreshCopy()
    {
        return new Cursor<T>(_items);
    }

    public void Rewind()
    {
        Position = 0;
    }

    public bool HasMore()
    {
        return Position < _items.Count - 1;
    }

    public bool HasLess()
    {
        return Position > 0;
    }

    public bool TryPeek(out T retval, int offset = 1)
    {
        retval = default!;
        if (offset < 0) { throw new ArgumentOutOfRangeException(nameof(offset)); }

        int index = Position + offset;
        if (index < 0 || index >= _items.Count) { return false; }
        retval = _items[index];
        return true;    
    }

    public bool TryRecall(out T retval, int offset = 1)
    {
        retval = default!;
        if (offset < 0) { throw new ArgumentOutOfRangeException(nameof(offset)); }

        int index = Position - offset;
        if (index < 0 || index >= _items.Count) { return false; }
        retval = _items[index];
        return true;
    }
    public bool Next()
    {
        if (!HasMore())
            return false;

        Position++;
        return true;
    }

    public bool Back()
    {
        if (!HasLess())
            return false;

        Position--;
        return true;
    }
}