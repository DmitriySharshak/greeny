using System.Diagnostics;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Greeny.WebApi.Controllers
{
    /// <summary>
    /// Контроллер для проведения нагрузочного тестирования
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/perfomance")]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiVersion("1.0")]

    public class PerfomanceController : Controller
    {
        private static readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        /// <summary>
        /// Получить статический контент
        /// </summary>
        /// <returns></returns>
        [HttpGet("static")]
        public string GetStatic()
        {
            return "Hello World";
        }

        /// <summary>
        /// Получить статический контент
        /// </summary>
        /// <returns></returns>
        [HttpGet("staticAsync")]
        public async Task<string> GetStaticAsync()
        {
            return await Task.Run(() => "Hello world" );
        }

        /// <summary>
        /// Нагрузить cpu 
        /// </summary>
        /// <param name="cpuTime">Время исполнения, мс.
        /// Эмулируем нагрузку заданное количесто времени</param>
        [HttpGet("cpu")]
        public async Task<int> Cpu(long cpuTime)
        {
            var result = await Task.Run(() =>
            {
                var processingTime = cpuTime == 0 ? 1000 : cpuTime;
                var lastIterationTime = 0L;
                var startIterationTime = GetTime();
                var threadId = Thread.CurrentThread.ManagedThreadId;

                Console.WriteLine($"Thread [{threadId}]: start");

                ulong i = 9999999999999999999;

                while (lastIterationTime - startIterationTime < processingTime)
                {
                    i = i * 9999999999999999999;
                    i++;

                    lastIterationTime = GetTime();
                }

                Console.WriteLine($"Thread [{threadId}]: finish");

                return threadId;
            });

            return result;
        }

        ///// <summary>
        ///// Эмуляция операции IO
        ///// </summary>
        ///// <param name="cpuTime">Время исполнения. Нагрузка на CPU</param>
        ///// <param name="sleep">Время ожидания. Эмулирует IO операции</param>
        //[HttpGet("cpu")]
        //public void Sleep(int sleep)
        //{
        //    Task.Run(() =>
        //    {



        //    });


        //}

        private long GetTime()
        {
            return _stopwatch.ElapsedMilliseconds;
        }
    }
}
