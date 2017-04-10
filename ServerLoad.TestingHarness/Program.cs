using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ServerLoad.Domain.DataModel;

namespace ServerLoad.TestingHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.Clear();
            Console.WriteLine("Test Started");
            testBegan = DateTime.Now;
            Task.Run(()=>Run5DayTest());
            Console.WriteLine("Waiting On Test.");
            Console.ReadKey();
            GetLoadsAsync("My-Server");
            Console.ReadKey();

        }

        private static int CompleteThreads = 0;
        private static DateTime testBegan;

        static void Run5DayTest()
        {
            var start = DateTime.Now.AddDays(-5);
            var end = DateTime.Now;

            for (var i = -5; i < 0; i++)
            {
                processHours(i, end);
            }
        }

        static async Task processMinutes(int hour, int day, DateTime end)
        {
            
            Console.WriteLine($"Working on Day {day} Hour {hour}");
            for (var m = -60; m < 0; m++)
            {
                DateTime begin = DateTime.Now;
                for (var s = -6; s < 0; s++)
                {
                    await PostCheckinAsync("My-Server", JsonConvert.SerializeObject(new Checkin()
                    {
                        SampleTime =
                            end.AddDays(day + 1)
                                .AddHours(hour + 1)
                                .AddMinutes(m + 1)
                                .AddSeconds((s + 1) * 10),
                        CpuUtilization = new Random(s + m + hour * 12).NextDouble(),
                        RamUtilization = new Random(s + m + hour * 12).NextDouble()
                    }));
                }

                DateTime finish = DateTime.Now;
                var dif = finish - begin;
                var fullDif = finish - testBegan;

                CompleteThreads += 1;
                Console.Clear();
                Console.WriteLine($"{GetPercentComplete()} Percent Complete.");
                Console.WriteLine($"{CompleteThreads} of {5*24*60} Minutes Completed");
                Console.WriteLine($"Completed in {dif.TotalMilliseconds} Milliseconds.");
                Console.WriteLine($"Test Running Time : {fullDif.TotalMinutes}:{fullDif.TotalSeconds}:{fullDif.TotalMilliseconds}");
            }
    }

        static async Task processHours(int day, DateTime end)
        {
            Console.WriteLine($"Woring on Day {day}");
            for (var h = -24; h < 0; h++)
            {
                processMinutes(h, day, end);

            }
        }

        static double GetPercentComplete()
        {
            var top = 5 * 24 * 60;
            return (double)((100 / (double) top) * CompleteThreads);
        }

        static async Task PostCheckinAsync(string json, string serverName)
        {
            var req = WebRequest.Create($"http://localhost:36254/api/server/{serverName}/checkin");
            var enc = new UTF8Encoding(false);
            var data = enc.GetBytes(json);

            req.Method = "POST";
            req.ContentType = "application/json";

            using (var sr = await req.GetRequestStreamAsync())
            {
                sr.Write(data, 0, data.Length);
            }
            var res = await req.GetResponseAsync();
            var rv = new StreamReader(res.GetResponseStream()).ReadToEnd();

            
        }

        static async Task GetLoadsAsync(string serverName)
        {
            var req = WebRequest.Create($"http://localhost:36254/api/server/{serverName}/loads");
            var enc = new UTF8Encoding(false);

            req.Method = "GET";

            var res = await req.GetResponseAsync();
            var rv = new StreamReader(res.GetResponseStream()).ReadToEnd();
            Console.WriteLine($"Response: {rv}");
        }
    }

    public static class AsyncHelpers
    {
        /// <summary>
        /// Execute's an async Task<T> method which has a void return value synchronously
        /// </summary>
        /// <param name="task">Task<T> method to execute</param>
        public static void RunSync(Func<Task> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            synch.Post(async _ =>
            {
                try
                {
                    await task();
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();

            SynchronizationContext.SetSynchronizationContext(oldContext);
        }

        /// <summary>
        /// Execute's an async Task<T> method which has a T return type synchronously
        /// </summary>
        /// <typeparam name="T">Return Type</typeparam>
        /// <param name="task">Task<T> method to execute</param>
        /// <returns></returns>
        public static T RunSync<T>(Func<Task<T>> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            T ret = default(T);
            synch.Post(async _ =>
            {
                try
                {
                    ret = await task();
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();
            SynchronizationContext.SetSynchronizationContext(oldContext);
            return ret;
        }

        private class ExclusiveSynchronizationContext : SynchronizationContext
        {
            private bool done;
            public Exception InnerException { get; set; }
            readonly AutoResetEvent workItemsWaiting = new AutoResetEvent(false);

            readonly Queue<Tuple<SendOrPostCallback, object>> items =
                new Queue<Tuple<SendOrPostCallback, object>>();

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("We cannot send to our same thread");
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                lock (items)
                {
                    items.Enqueue(Tuple.Create(d, state));
                }
                workItemsWaiting.Set();
            }

            public void EndMessageLoop()
            {
                Post(_ => done = true, null);
            }

            public void BeginMessageLoop()
            {
                while (!done)
                {
                    Tuple<SendOrPostCallback, object> task = null;
                    lock (items)
                    {
                        if (items.Count > 0)
                        {
                            task = items.Dequeue();
                        }
                    }
                    if (task != null)
                    {
                        task.Item1(task.Item2);
                        if (InnerException != null) // the method threw an exeption
                        {
                            throw new AggregateException("AsyncHelpers.Run method threw an exception.", InnerException);
                        }
                    }
                    else
                    {
                        workItemsWaiting.WaitOne();
                    }
                }
            }

            public override SynchronizationContext CreateCopy()
            {
                return this;
            }
        }
    }
}