using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntireData.DTO;

namespace EntireData
{
    class Program
    {
        static void Main(string[] args)
        {
            var provider = new Provider();
            var stations = provider.GetStations("Stort").ToArray();


            var tasks = new Task<StationResults>[stations.Count()];
            for (var i = 0; i < stations.Count(); i++)
            {
                Task<StationResults> t = provider.GetMeasureAsync(stations[i].stationReference);
                tasks.SetValue(t, i);
            }
            Task.Factory.ContinueWhenAll(tasks, (ret) =>
            {
                ret.ToList().ForEach(x =>
                {
                    Console.WriteLine("station=" + x.Result.RLOIid + ", Max="
                        + x.Result.Measures.Max(z => z.value) + ", Min=  "
                        + x.Result.Measures.Min(z => z.value.ToString()) + ", Aver=  "
                        + x.Result.Measures.Sum(z => z.value) / x.Result.Measures.Count());
                });
            }).Wait();

            Console.ReadKey();
        }
    }
}
