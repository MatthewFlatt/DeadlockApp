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
                string username = "";
                string password = "";
                try
                {
                    username = args[2];
                    password = args[3];
                }
                catch (Exception)
                {
                    // ignored
                }

                var setup = new DbSetup();
                setup.CreateTables(instance, database, username, password);
                setup.CreateSprocs(instance, database, username, password);

                if (args.Length == 2 || args.Length == 4)
                {
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        var runA = new SprocRunner();
                        runA.RunSProc(instance, database, "UserADeadlock", username, password);
                    }).Start();

                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        Thread.Sleep(5000);
                        var runB = new SprocRunner();
                        runB.RunSProc(instance, database, "UserBDeadlock", username, password);
                    }).Start();

                    Thread.Sleep(TimeSpan.FromMinutes(2));
                }
                else
                {
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        var runA = new SprocRunner();
                        runA.RunSProc(instance, database, "UserABlocking", username, password);
                    }).Start();

                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                        var runB = new SprocRunner();
                        runB.RunSProc(instance, database, "UserBBlocking", username, password);
                    }).Start();

                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                        var runC = new SprocRunner();
                        runC.RunSProc(instance, database, "UserCBlocking", username, password);
                    }).Start();

                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                        var runC = new SprocRunner();
                        runC.RunSProc(instance, database, "UserCBlocking", username, password);
                    }).Start();

                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    var runLast = new SprocRunner();
                    runLast.RunSProc(instance, database, "UserCBlocking", username, password);

                }
                
            }
            else
            {
                Console.WriteLine("2 or more arguments are required, instance, database, (blocking)");
            }

        }
    }
}
