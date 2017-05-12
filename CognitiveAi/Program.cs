using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CSHttpClientSample
{
	static class Program
	{
		static void Main()
		{
			Console.Write("Enter image file path: ");
			string imageFilePath = Console.ReadLine();

			MakeAnalysisRequest(imageFilePath);

			Console.WriteLine("\n\n\nHit ENTER to exit...");
			Console.ReadLine();
		}

		static byte[] GetImageAsByteArray(string imageFilePath)
		{
			FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			return binaryReader.ReadBytes((int)fileStream.Length);
		}

		static async void MakeAnalysisRequest(string imageFilePath)
		{
			var client = new HttpClient();

			// Request headers - replace this example key with your valid subscription key.
			client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "2227a7525eed48e084b5f33578441cc7");

			// Request parameters. A third optional parameter is "details".
			string requestParameters = "visualFeatures=Description&language=en";
			string uri = "https://eastus2.api.cognitive.microsoft.com/vision/v1.0/analyze?" + requestParameters;
			Console.WriteLine(uri);

			HttpResponseMessage response;

			// Request body. Try this sample with a locally stored JPEG image.
			byte[] byteData = GetImageAsByteArray(imageFilePath);

			using (var content = new ByteArrayContent(byteData))
			{
				// This example uses content type "application/octet-stream".
				// The other content types you can use are "application/json" and "multipart/form-data".
				content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
				response = await client.PostAsync(uri, content);
			}
		}
	}
}