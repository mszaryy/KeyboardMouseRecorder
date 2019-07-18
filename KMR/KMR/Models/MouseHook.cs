using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Threading;

namespace KMR.Models
{
    class MouseHook
    {

        private static ObservableCollection<Step> record { get; set; }

        private LowLevelHookMouse proc = HookCallBack;
        private int TypeOfHook = 14;
        private static IntPtr HookMouseID = IntPtr.Zero;
        private static bool HookStop = false;
        private static Stopwatch stopwatch;
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelHookMouse lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);


        private static Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        private delegate IntPtr LowLevelHookMouse(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (HookStop == true)
            {
                UnhookWindowsHookEx(HookMouseID);
                return new IntPtr();
            }
            else
            {
                if (nCode >= 0)
                {
                    stopwatch.Stop();

                    MouseHookData hookStruct = (MouseHookData)Marshal.PtrToStructure(lParam, typeof(MouseHookData));
                    Step step = new Step()
                    {
                        ButtonClick = (uint)wParam,
                        MousePostion = hookStruct.point,
                        MouseDeltaWheel = GetWheelDelta(hookStruct.mouseData),
                        TimeToNextMove = (int)stopwatch.ElapsedMilliseconds == 0 ? 1 : (int)stopwatch.ElapsedMilliseconds
                    };

                    dispatcher.Invoke(() => record.Add(step));
                   // record.Add(step);
                    Console.WriteLine(step.ToString());
                    // Console.WriteLine("ButotnClick" + wParam + " Postion " + hookStruct.point.x + "," + hookStruct.point.y + " WheelMouve: " + GetWheelDelta(hookStruct.mouseData) + " Time:" + stopwatch.ElapsedMilliseconds + "ms");


                    stopwatch.Reset();
                    stopwatch.Start();

                }

                return CallNextHookEx(HookMouseID, nCode, wParam, lParam);
            }

        }

        private static int GetWheelDelta(uint mouseData)
        {
            if (mouseData / 65534 == 120)
            {
                return 120;
            }
            else if (mouseData / 65534 == 65417)
            {
                return -120;
            }
            else
            {
                return 0;
            }
        }

        private IntPtr HookInstall(LowLevelHookMouse proc)
        {
            using (Process currentProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule currentModule = currentProcess.MainModule)
                {
                    return SetWindowsHookEx(TypeOfHook, proc, GetModuleHandle(currentModule.ModuleName), 0);
                }
            }
        }


        public MouseHook(ObservableCollection<Step> recordd)
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
            record = recordd;
        }


        public void Start()
        {
            HookMouseID = HookInstall(proc);
            Application.Run();
        }

        public void Stop()
        {
            HookStop = true;
        }
        private struct MouseHookData
        {
            public Point point;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
    }
}

