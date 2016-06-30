using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeadlockApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var setup = new DbSetup();
            setup.CreateTables(".","test");
            setup.CreateSprocs(".", "test");

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                var runA = new SprocRunner();
                runA.RunSProc(".", "test", "UserA");
            }).Start();
            
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Thread.Sleep(5000);
                var runB = new SprocRunner();
                runB.RunSProc(".", "test", "UserB");
            }).Start();

            Thread.Sleep(TimeSpan.FromMinutes(2));

        }
    }
}
