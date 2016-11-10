using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntireData.DTO;

namespace EntireData
{
    public interface IProvider
    {
        List<Station> GetStations(string riverName);
        Task<StationResults> GetMeasureAsync(string station);
    }
}
