// File: CursorCharTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XecronixCursor;

namespace XecronixCursorTest;

[TestClass]
public class CursorCharTests
{
    private static Cursor<char> CreateCursor(string source)
    {
        return new Cursor<char>(source.ToCharArray());
    }

    [TestMethod]
    public void Constructor_ThrowsWhenSourceIsNull()
    {
        try
        {
            _ = new Cursor<char>(null!);
            Assert.Fail("Expected ArgumentNullException was not thrown.");
        }
        catch (ArgumentNullException)
        {
        }
    }

    [TestMethod]
    public void Constructor_ThrowsWhenSourceIsEmpty()
    {
        try
        {
            _ = new Cursor<char>(Array.Empty<char>());
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
        }
    }

    [TestMethod]
    public void Current_StartsAtFirstCharacter()
    {
        var cursor = CreateCursor("abc");

        Assert.AreEqual('a', cursor.Current);
        Assert.AreEqual(0, cursor.Position);
    }

    [TestMethod]
    public void TryPeek_ReturnsNextCharacter()
    {
        var cursor = CreateCursor("abc");

        bool found1 = cursor.TryPeek(out char value1);
        bool found2 = cursor.TryPeek(out char value2, 2);

        Assert.IsTrue(found1);
        Assert.AreEqual('b', value1);

        Assert.IsTrue(found2);
        Assert.AreEqual('c', value2);
    }

    [TestMethod]
    public void Next_MovesPositionForward()
    {
        var cursor = CreateCursor("abc");

        bool moved = cursor.Next();

        Assert.IsTrue(moved);
        Assert.AreEqual(1, cursor.Position);
        Assert.AreEqual('b', cursor.Current);
    }

    [TestMethod]
    public void HasMore_IsTrueWhenAnotherCharacterExists()
    {
        var cursor = CreateCursor("abc");
        Assert.IsTrue(cursor.HasMore());
    }

    [TestMethod]
    public void HasMore_IsFalseWhenNoNextCharacterExists()
    {
        var cursor = CreateCursor("a");
        Assert.IsFalse(cursor.HasMore());
    }

    [TestMethod]
    public void HasLess_IsTrueWhenPreviousCharacterExists()
    {
        var cursor = CreateCursor("ab");
        cursor.Next();
        Assert.IsTrue(cursor.HasLess());
    }

    [TestMethod]
    public void HasLess_IsFalseWhenNoPreviousCharacterExists()
    {
        var cursor = CreateCursor("a");
        Assert.IsFalse(cursor.HasLess());
    }

    [TestMethod]
    public void Back_PositionIsZeroAfterMovingForwardAndBackward()
    {
        var cursor = CreateCursor("abc");
        cursor.Next();

        Assert.AreEqual(1, cursor.Position);

        bool moved = cursor.Back();

        Assert.IsTrue(moved);
        Assert.AreEqual(0, cursor.Position);
        Assert.AreEqual('a', cursor.Current);
    }

    [TestMethod]
    public void TryRecall_ReturnsPreviousChar()
    {
        var cursor = CreateCursor("abc");
        cursor.Next();

        bool found = cursor.TryRecall(out char value);

        Assert.IsTrue(found);
        Assert.AreEqual('a', value);
    }

    [TestMethod]
    public void TryRecall_ReturnsFalseAtBeginning()
    {
        var cursor = CreateCursor("a");

        bool found = cursor.TryRecall(out char value);

        Assert.IsFalse(found);
    }

    [TestMethod]
    public void Next_ReturnsFalseWhenAlreadyAtEnd()
    {
        var cursor = CreateCursor("a");

        Assert.IsFalse(cursor.Next());
    }

    [TestMethod]
    public void Back_ReturnsFalseWhenAlreadyAtBeginning()
    {
        var cursor = CreateCursor("a");

        Assert.IsFalse(cursor.Back());
        Assert.AreEqual(0, cursor.Position);
    }

    [TestMethod]
    public void Rewind_MovesTheCursorPositionToZero()
    {
        var cursor = CreateCursor("abc");
        while (cursor.Next()) ; // move to the end of the cursor.
        Assert.AreNotEqual(0, cursor.Position);
        cursor.Rewind();
        Assert.AreEqual(0, cursor.Position);
    }


    [TestMethod]
    public void Rewind_CreatesAFreshCopyWithPositionEqZero()
    {
        var cursor1 = CreateCursor("abc");
        while (cursor1.Next()) ; // move to the end of the cursor.
        var cursor2 = cursor1.FreshCopy();
        Assert.AreEqual(0, cursor2.Position);
        Assert.AreNotEqual(cursor1.Position, cursor2.Position);
    }

    [TestMethod]
    public void FreshCopy_DoesNotChangeOriginalCursorPosition()
    {
        var cursor1 = CreateCursor("abc");
        while (cursor1.Next()) ; // move to the end of the cursor.

        int originalPosition = cursor1.Position;

        var cursor2 = cursor1.FreshCopy();

        Assert.AreEqual(originalPosition, cursor1.Position);
        Assert.AreEqual(0, cursor2.Position);
    }

    [TestMethod]
    public void TryPeek_ReturnsFalseWhenOffsetIsOutOfRange()
    {
        var cursor = CreateCursor("a");

        bool found1 = cursor.TryPeek(out char value1);
        bool found2 = cursor.TryPeek(out char value2, 2);

        Assert.IsFalse(found1);
        Assert.IsFalse(found2);
    }

    [TestMethod]
    public void TryRecall_ReturnsFalseWhenOffsetIsOutOfRange()
    {
        var cursor = CreateCursor("abc");

        bool found1 = cursor.TryRecall(out char value1);

        cursor.Next();
        bool found2 = cursor.TryRecall(out char value2, 2);

        Assert.IsFalse(found1);
        Assert.IsFalse(found2);
    }

    [TestMethod]
    public void TryPeek_ThrowsWhenOffsetIsNegative()
    {
        var cursor = CreateCursor("abc");

        try
        {
            cursor.TryPeek(out char value, -1);
            Assert.Fail("Expected ArgumentOutOfRangeException was not thrown.");
        }
        catch (ArgumentOutOfRangeException)
        {
        }
    }

    [TestMethod]
    public void TryRecall_ThrowsWhenOffsetIsNegative()
    {
        var cursor = CreateCursor("abc");

        try
        {
            cursor.TryRecall(out char value, -1);
            Assert.Fail("Expected ArgumentOutOfRangeException was not thrown.");
        }
        catch (ArgumentOutOfRangeException)
        {
        }
    }
}