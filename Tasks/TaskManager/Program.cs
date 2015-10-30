using System;
using Microsoft.Win32.TaskScheduler;

namespace TaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1 - Start service.");
            Console.WriteLine("2 - Stop service.");
            
            var a = Console.Read();

            if (a == '1')
            {
                using (TaskService ts = new TaskService())
                {
                    // Create a new task definition and assign properties
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = "Does something";
                    TimeTrigger tt = new TimeTrigger();
                    tt.StartBoundary = DateTime.Now.AddMinutes(1);
                    tt.Repetition.Interval = TimeSpan.FromMinutes(1);
                    td.Triggers.Add(tt);
                    td.Actions.Add(new ExecAction(@"c:\Users\Nikk\Documents\Projects\Medcenter\Tasks\SyncronizationTask\bin\Debug\SyncronizationTask.exe", "c:\\logs\\test.log", null));

                    // Register the task in the root folder
                    ts.RootFolder.RegisterTaskDefinition("Test", td);
                }
                Console.WriteLine("Task has been started.");
            }
            else if (a == '2')
            {
                using (TaskService ts = new TaskService())
                {
                    ts.RootFolder.DeleteTask("Test");
                }
                Console.WriteLine("Task has been terminated.");
            }

            Console.ReadKey();
        }
    }
}
