using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Diagnostics;

namespace WordOfTheDay.PerformanceCheck
{
    class PerformanceCheck
    {
        static async Task Main()
        {
            Request request = new(20, @"C:\Users\Илья\OneDrive\Документы\check.txt");
            await request.MinMaxAvgTime();
        }
    }
}
