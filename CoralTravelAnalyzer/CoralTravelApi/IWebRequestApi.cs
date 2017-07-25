using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoralTravelAnalyzer.CoralTravelApi
{
    interface IWebRequestApi<T>
    {
        Task<T> GetDataAsync(bool instant);

        void SetRequestParameters(params string[] parameters);
    }
}
