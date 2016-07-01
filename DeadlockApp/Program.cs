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
            if (args.Length >= 2)
            {
                var instance = args[0];
                var database = args[1];
                var setup = new DbSetup();
                setup.CreateTables(instance, database);
                setup.CreateSprocs(instance, database);

                if (args.Length == 2)
                {
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        var runA = new SprocRunner();
                        runA.RunSProc(instance, database, "UserADeadlock");
                    }).Start();

                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        Thread.Sleep(5000);
                        var runB = new SprocRunner();
                        runB.RunSProc(instance, database, "UserBDeadlock");
                    }).Start();

                    Thread.Sleep(TimeSpan.FromMinutes(2));
                }
                else
                {
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        var runA = new SprocRunner();
                        runA.RunSProc(instance, database, "UserABlocking");
                    }).Start();

                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                        var runB = new SprocRunner();
                        runB.RunSProc(instance, database, "UserBBlocking");
                    }).Start();

                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                        var runC = new SprocRunner();
                        runC.RunSProc(instance, database, "UserCBlocking");
                    }).Start();

                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                        var runC = new SprocRunner();
                        runC.RunSProc(instance, database, "UserCBlocking");
                    }).Start();

                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    var runLast = new SprocRunner();
                    runLast.RunSProc(instance, database, "UserCBlocking");

                }
                
            }
            else
            {
                Console.WriteLine("2 or more arguments are required, instance, database, (blocking)");
            }

        }
    }
}
