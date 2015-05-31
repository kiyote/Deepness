
namespace Model.Map
{
    using System;

    /*
    [Flags]
    public enum TileCompass : byte
    {
        None = 0,

        TopLeft = 1,
        Top = 2,
        TopRight = 4,

        Left = 8,
        Right = 16,

        BottomLeft = 32,
        Bottom = 64,
        BottomRight = 128
    }
     */

    [Flags]
    public enum TileCompass : byte
    {
        None = 0,
        Bottom = 1,
        Left = 2,
        Top = 4,
        Right = 8
    }
}