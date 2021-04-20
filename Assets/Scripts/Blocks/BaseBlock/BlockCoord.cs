using System;

public readonly struct BlockCoord : IEquatable<BlockCoord>
{
    public readonly byte X;
    public readonly byte Y;
    public readonly bool IsValid;

    public BlockCoord(byte x, byte y) //valid coord
    {
        X = x;
        Y = y;
        IsValid = true;
    }

    public BlockCoord(bool isValid) //invalid coord
    {
        IsValid = false;
        X = 0;
        Y = 0;
    }

    public override bool Equals(object obj)
    {
        return obj is BlockCoord overrides && Equals(overrides);
    }

    public bool Equals(BlockCoord other)
    {
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode() //via https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-overriding-gethashcode
    {
        int hash = 17;
        hash = hash * 23 + X.GetHashCode();
        hash = hash * 23 + Y.GetHashCode();
        
        return hash;
    }

    public static bool operator ==(BlockCoord left, BlockCoord right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(BlockCoord left, BlockCoord right)
    {
        return !(left == right);
    }
}