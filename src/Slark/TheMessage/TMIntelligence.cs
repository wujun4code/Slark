using System;
namespace TheMessage
{
    public class TMIntelligence
    {
        public TMIntelligence()
        {
        }

        public bool IsReal
        {
            get => !IsBlack;
        }

        public bool IsRed
        {
            get => Color == TMColor.Red || Color == TMColor.RedBlack;
        }

        public bool IsBlue
        {
            get => Color == TMColor.Blue || Color == TMColor.BlueBlack;
        }

        public bool IsBlack
        {
            get => Color == TMColor.Black || Color == TMColor.RedBlack || Color == TMColor.BlueBlack;
        }

        public TMColor Color { get; set; }

        public TMTransmittalMode TransmittalMode { get; set; }

        public enum TMColor
        {
            Black,
            Red,
            Blue,
            BlueBlack,
            RedBlack
        }

        public enum TMTransmittalMode
        {
            Directly,
            Secretly,
            Plainly
        }
    }
}
