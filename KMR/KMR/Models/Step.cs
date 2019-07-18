using System;

namespace KMR.Models
{
    class Step
    {
        public uint ButtonClick { get; set; }
        public int MouseDeltaWheel { get; set; }
        public Point MousePostion { get; set; }
        public int TimeToNextMove { get; set; }
        public uint ExtraInfo { get; set; }


        public override string ToString()
        {
            return String.Format("Button: {0},  MouseDeltaWheel: {1}, MousePosition X:{2} Y:{3}, TimeToNextMove: {4}ms", (MouseMessages)ButtonClick, MouseDeltaWheel, MousePostion.x, MousePostion.y, TimeToNextMove);
        }

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_MOUSEMIDDLEDOWN = 0X0207,
            WM_MOUSEMIDDLEUP = 0x0208,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }
    }
}
