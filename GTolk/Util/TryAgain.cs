using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTolk.Util
{
    public static class TryAgain
    {
        public static void Run(Action code)
        {
            Run(5, code);
        }

        public static void Run(int howManyTimesToTry, Action code)
        {
            Run(howManyTimesToTry, 500, code);
        }

        public static void Run(int howManyTimesToTry, int howManyMilisecondsToWait, Action code)
        {
            Exception exception = null;
            int runningTimes = 0;

            while (runningTimes < howManyTimesToTry)
            {
                try
                {
                    runningTimes++;
                    code();
                    exception = null;
                    break;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    System.Threading.Thread.Sleep(howManyMilisecondsToWait);
                }
            }

            if (exception != null)
            {
                throw exception;
            }
        }
    }
}