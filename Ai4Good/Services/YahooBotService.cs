using System.Net;
using System.Threading.Tasks;


namespace Ai4Good
{
	public class YahooBotService
	{
		public static async Task<double?> GetStockRateAsync(string StockSymbol)
		{
			try {
				string ServiceURL = $"http://finance.yahoo.com/d/quotes.csv?s={StockSymbol}&f=sl1d1nd";

				string ResultInCSV;

				using (WebClient client = new WebClient())
				{
					ResultInCSV = await client.DownloadStringTaskAsync(ServiceURL).ConfigureAwait(false);
				}

				var FirstLine = ResultInCSV.Split('\n')[0];

				var Price = FirstLine.Split(',')[1];

				if (Price != null && Price.Length >= 0)
				{
					double result;

					if (double.TryParse(Price, out result))
					{
						return result;
					}
				}
				return null;
			}
			catch (WebException ex)
			{
				throw ex;
			}
		}


	}
}