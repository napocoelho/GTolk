using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GTolk.Util
{
    public static class InterfaceWorker
    {
        public static InterfaceWorkerStatus Execute(Action code)
        {
            BackgroundWorker worker = new BackgroundWorker();
            InterfaceWorkerStatus status = new InterfaceWorkerStatus();

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            worker.DoWork += (object sender, DoWorkEventArgs e) =>
                {
                    worker.ReportProgress(0);
                };

            worker.ProgressChanged += (object sender, ProgressChangedEventArgs e) =>
                {
                    code();
                    status.WorkIsDone = true;
                };

            worker.RunWorkerAsync();

            return status;
        }
    }

    public class InterfaceWorkerStatus
    {
        public bool WorkIsDone { get; set; }

        public InterfaceWorkerStatus()
        {
            this.WorkIsDone = false;
        }
    }
}