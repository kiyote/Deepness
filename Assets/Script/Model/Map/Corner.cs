
namespace Model.Map
{
    using System;

    [Flags]
    public enum Corner
    {
        None = 0x00,
        BottomLeft = 0x01,
        TopLeft = 0x02,
        TopRight = 0x04,
        BottomRight = 0x08
    }
}
