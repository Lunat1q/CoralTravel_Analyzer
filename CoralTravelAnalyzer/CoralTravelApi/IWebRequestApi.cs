using System.Threading.Tasks;

namespace CoralTravelAnalyzer.CoralTravelApi
{
    interface IWebRequestApi<T>
    {
        Task<T> GetDataAsync(bool instant, int delaySec);

        void SetRequestParameters(params string[] parameters);
    }
}
