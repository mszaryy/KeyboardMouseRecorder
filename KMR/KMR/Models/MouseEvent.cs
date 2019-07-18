using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using KMR.Models;

namespace KMR.Models
{
    abstract class MouseEvent
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        private static void DoMouseEvent(Step step)
        {
            System.Threading.Thread.Sleep((int)step.TimeToNextMove);
            Cursor.Position = new System.Drawing.Point(step.MousePostion.x, step.MousePostion.y);
            mouse_event(MouseEventCoverter((MouseMessages)step.ButtonClick), 0, 0, step.MouseDeltaWheel, 0);
        }


        private static uint MouseEventCoverter(MouseMessages msg)
        {
            return (uint)(MouseEvents)Enum.Parse(typeof(MouseEvents), msg.ToString());
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

        private enum MouseEvents
        {
            WM_LBUTTONDOWN = 0x02,
            WM_LBUTTONUP = 0x04,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x0800,
            WM_MOUSEMIDDLEDOWN = 0x0020,
            WM_MOUSEMIDDLEUP = 0x0040,
            WM_RBUTTONDOWN = 0x08,
            WM_RBUTTONUP = 0x010

        }
    }
}
