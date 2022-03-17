using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace GTolk.Util
{
    public static class FlashWindowHelper
    {
        private static object LOCK = new object();
        private static bool blinkingActivated = false;
        public static bool BlinkingActivated
        {
            get
            {
                lock (LOCK)
                {
                    return blinkingActivated;
                }
            }
            private set
            {
                lock (LOCK) { blinkingActivated = value; }
            }
        }
        
        [DllImport("user32.dll")]
        private static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

        public static void FlashWindow(System.Windows.Forms.Form window)
        {
            if (window != null && !window.IsDisposed)
            {
                FlashWindow(window.Handle, true);
            }
        }

        public static void StopBlinking()
        {
            blinkingActivated = false;
        }

        public static void StartBlinking(System.Windows.Forms.Form window, int miliseconds = 5000)
        {
            blinkingActivated = true;

            BackgroundWorker BlinkWorker = new System.ComponentModel.BackgroundWorker();

            BlinkWorker.ProgressChanged += (object sender, ProgressChangedEventArgs e) =>
            {
                try
                {
                    lock (LOCK)
                    {
                        if (window != null && !window.IsDisposed)
                        {
                            FlashWindow(window);
                        }
                    }
                }
                catch { }
            };

            BlinkWorker.DoWork += (object sender, DoWorkEventArgs e) =>
            {
                try
                {
                    lock (LOCK)
                    {
                        if (!blinkingActivated) return;
                        BlinkWorker.ReportProgress(0);
                    }

                    while (true)
                    {
                        Thread.Sleep(miliseconds);

                        if (window == null || window.IsDisposed)
                        {
                            blinkingActivated = false;
                            return;
                        }

                        lock (LOCK)
                        {
                            if (!blinkingActivated) break;
                            BlinkWorker.ReportProgress(0);
                        }
                    }
                }
                catch { }
            };

            BlinkWorker.WorkerReportsProgress = true;
            BlinkWorker.RunWorkerAsync();
        }
    }
}