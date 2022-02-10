using System.Threading.Tasks;

namespace WordOfTheDay.PerformanceCheck
{
    class PerformanceCheck
    {
        static async Task Main()
        {
            Requester request = new(10000, @"C:\Users\Илья\OneDrive\Документы\check.txt");
            await request.ShowTimeMeasuruments();
        }
    }
}
