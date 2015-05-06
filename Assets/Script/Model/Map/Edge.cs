using System;

[Flags]
public enum Edge
{
	None = 0x00,
	Bottom = 0x01,
	Left = 0x02,
	Top = 0x04,
	Right = 0x08
}
