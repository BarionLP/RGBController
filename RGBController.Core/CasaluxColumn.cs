namespace RGBController.Core;

public sealed class CasaluxColumn(IRController irController)
{
    private readonly IRController _irController = irController;

    public static class Commands{
        public const byte BRIGHTEN = 0x0;
        public const byte DIMM = 0x1;
        public const byte SWITCH = 0x2;
        public const byte TOGGLE_ON_OFF = 0x3;
        public const byte COOL_WHITE = 0x7;
        public const byte WARM_WHITE = 0xB;
        public const byte COOLER_WHITE = 0xF;
        public const byte WARMER_WHITE = 0x13;
        public const byte MODE = 0x17;
    }

    public static class Colors{
        public const byte RED = 0x4;
        public const byte RED_ORANGE = 0x8;
        public const byte ORANGE = 0xC;
        public const byte ORANGE_YELLOW= 0x10;
        public const byte YELLOW = 0x14;
        public const byte GREEN = 0x5;
        public const byte GREEN_CYAN = 0x9;
        public const byte CYAN = 0xD;
        public const byte CYAN_BLUE = 0x11;
        public const byte LIGHT_BLUE = 0x15;
        public const byte BLUE = 0x6;
        public const byte BLUE_PURPLE = 0xA;
        public const byte PURPLE = 0xE;
        public const byte PURPLE_MAGENTA = 0x12;
        public const byte MAGENTA = 0x16;
    }
}
