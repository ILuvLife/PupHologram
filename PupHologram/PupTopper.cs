using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PupHologram
{
    //TODO - add logging

    [Guid("885b02df-1f4e-4993-a807-3758079af302")]
    public interface AxTopper
    {
        [DispId(1)]
        void GameChanged(string game, string manufacturer, string defaultName);
    }

    [ComVisible(true)]
    [Guid("8084179a-5a6a-4169-a4d1-ce3e905a7516")]
    [ClassInterface(ClassInterfaceType.None)]
    public class PupTopper : AxTopper
    {
        private static DateTime gameChangedTime = DateTime.MinValue;
        private static Timer timer;
        private static object lockObject = new object();
        private static string lastGame = "";
        private static string lastManufacturer = "";
        private static string lastDefaultName = "";
        private static bool lastWritten = false;


        //Can't have static constructor with com
        //static PupTopper()
        //{ }

        public PupTopper()
        {
            GetNewTimer();
        }

        private void GetNewTimer()
        {
            timer = new Timer(1000);
            timer.AutoReset = false;
            timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
            timer.Enabled = false;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            lock (lockObject)
            {
                timer.Stop();
                timer.Dispose();
                if(!lastWritten)
                {
                    lastWritten = true;
                    DoIt(lastGame, lastManufacturer, lastDefaultName);
                }
            }
        }

        [ComVisible(true)]
        public void GameChanged(string game, string manufacturer, string defaultName)
        {

            try
            {
                lock(lockObject){
                    timer.Stop();
                    timer.Dispose();
                    lastGame = game;
                    lastManufacturer = manufacturer;
                    lastDefaultName = defaultName;
                    lastWritten = false;
                    GetNewTimer();
                    timer.Enabled = true;
                }
            }
            catch (Exception)
            {
            }

        }

        private static void DoIt(string game, string manufacturer, string defaultName)
        {
            game = AddDoubleQuotes(game);
            manufacturer = AddDoubleQuotes(manufacturer);
            defaultName = AddDoubleQuotes(defaultName);


            //Test code to write a log file instead of trying to call PlayHologram. Great for debugging the PupMenuScript.pup file
            //string path = Path.Combine(@"c:\temp\", "logit.txt");
            //using (FileStream fs = new FileStream(path, FileMode.Append))
            //{
            //    using (StreamWriter sw = new StreamWriter(fs))
            //    {
            //        sw.WriteLine(string.Format("{0} game {1} manufacturer {2} default {3}", DateTime.Now, game, manufacturer, defaultName));
            //    }
            //}

            //TODO - Make the path to the file exposed and set it in script.
            string path = Path.Combine(@"C:\HologramSoftware\PlayHologramFiles\", "PlayHologramFiles.exe");
            string cParams = string.Format("-machinename {0} -manufacturer {1} -default {2} -loop", game, manufacturer, defaultName);

            //TODO - log errors.
            ProcessStartInfo startInfo = new ProcessStartInfo(path);
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = cParams;
            startInfo.UseShellExecute = false;
            var proc = new Process();
            proc.StartInfo = startInfo;
            proc.Start();

        }

        private static string AddDoubleQuotes(string value)
        {
            return "\"" + value + "\"";
        }
    }
}
