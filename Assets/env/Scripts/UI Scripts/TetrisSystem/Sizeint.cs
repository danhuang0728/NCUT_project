using System;

[Serializable]
public struct SizeInt
{
    public int width;
    public int height;

    public SizeInt(int width, int height)
    {
        this.width = width;
        this.height = height;
    }
}