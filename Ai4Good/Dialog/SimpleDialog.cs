using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using FootBallData;
using System.Web.Http;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.ProjectOxford.Vision;

namespace Ai4Good
{
	[Serializable]
	public class SimpleDialog : IDialog
	{
	
	

		public async Task StartAsync(IDialogContext context)
		{
			context.Wait(MessageReceivedStart);
		}

		public async Task MessageReceivedStart(IDialogContext context, IAwaitable<IMessageActivity> argument)
		{
			await context.PostAsync("Do you want to play a game?");
			context.Wait(MessageReceivedOperationChoice);
		}

		public async Task MessageReceivedOperationChoice(IDialogContext context, IAwaitable<IMessageActivity> argument)
		{
			var message = await argument;

			if (message.Text.ToLower().Contains("yes"))
			{
				Random random = new Random();
				int shuffle = random.Next(1, 13);

				if(shuffle % 2 == 0)	
				{
					await context.PostAsync("Please send me an image of a lion");
					context.Wait(ValidateReceivedImage);
				}
				else
				{
					var activity = await argument as Activity;
					var reply = activity.CreateReply();
					reply.Attachments = new List<Attachment>();

		
					HeroCard hc = new HeroCard()
					{
						Title = "What Image is this? ",
						Subtitle = "It roars!!!"
					};
					

					List<CardImage> images = new List<CardImage>();
					CardImage ci = new CardImage("http://elelur.com/data_images/mammals/lion/lion-01.jpg");
					images.Add(ci);
					hc.Images = images;
					reply.Attachments.Add(hc.ToAttachment());
					await context.PostAsync(reply);
					context.Wait(ValidateReceivedText);
				}



			}
			else
			{
				await context.PostAsync("Nice playing with you. God bless.");
			}
		}

		public async Task ValidateReceivedText(IDialogContext context, IAwaitable<IMessageActivity> argument)
		{
			var message = await argument;
			string text = string.Empty;
			if (message.Text.ToLower().Contains("lion"))
			{
				text = "Correct! You got it right.";
				await context.PostAsync(text);
			}
			else
			{
				text = "Sorry! You are wrong. This is a lion.";
				await context.PostAsync(text);
			}

			await context.PostAsync("Do you want to play again?");
			context.Wait(MessageReceivedOperationChoice);
		}

		public async Task ValidateReceivedImage(IDialogContext context, IAwaitable<IMessageActivity> argument)
		{
			var activity = await argument as Activity;
			var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

			var imageAttachment = activity.Attachments?.FirstOrDefault(a => a.ContentType.Contains("image"));

			if (imageAttachment != null)
			{
				using (var stream = await GetImageStream(connector, imageAttachment))
				{
					var client = new VisionServiceClient("2227a7525eed48e084b5f33578441cc7", "https://eastus2.api.cognitive.microsoft.com/vision/v1.0");

					VisualFeature[] VisualFeatures = { VisualFeature.Description };
					var result = await client.AnalyzeImageAsync(stream, VisualFeatures);

					var validation = result.Description.Tags.Where(x => x == "lion").FirstOrDefault();

					string caption = result.Description.Captions[0].Text;

					if (validation != null)
					{					

						string text = "Correct! You got it right. Nice picture you have there. I see " + caption;
						await context.PostAsync(text);
						await context.PostAsync("Do you want to know more about lions?");
						context.Wait(KnowMore);
					}
					else
					{
						string text = "Sorry! I don't see any picture of a lion. I see " + caption;

						await context.PostAsync(text);
						await context.PostAsync("Do you want to play again?");
						context.Wait(MessageReceivedOperationChoice);
					}

				}
			}
			
		
		}

		public async Task KnowMore(IDialogContext context, IAwaitable<IMessageActivity> argument)
		{
			var message = await argument;

			if (message.Text.ToLower().Contains("yes"))
			{

				var client = new HttpClient();
				client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "6248d2caa0a744a3823f9df847d862a3");
				var queryString = HttpUtility.ParseQueryString(string.Empty);
				queryString["q"] = "what is a lion?";
				var query = "https://api.cognitive.microsoft.com/bing/v5.0/search?" + queryString;

				// Run the query
				HttpResponseMessage httpResponseMessage = client.GetAsync(query).Result;

				// Deserialize the response content
				var responseContentString = httpResponseMessage.Content.ReadAsStringAsync().Result;
				Newtonsoft.Json.Linq.JObject responseObjects = Newtonsoft.Json.Linq.JObject.Parse(responseContentString);

				Newtonsoft.Json.Linq.JToken snippet = responseObjects.SelectToken("webPages.value[0].snippet");

				string result = snippet.ToString();

				await context.PostAsync(result);

				
			}
			
			await context.PostAsync("Do you want to play again?");
			context.Wait(MessageReceivedOperationChoice);
		}

		public async Task PlayAgain(IDialogContext context, IAwaitable<IMessageActivity> argument)
		{		
			context.Wait(MessageReceivedOperationChoice);
		}


		private static async Task<Stream> GetImageStream(ConnectorClient connector, Attachment imageAttachment)
		{
			using (var httpClient = new HttpClient())
			{
				// The Skype attachment URLs are secured by JwtToken,
				// you should set the JwtToken of your bot as the authorization header for the GET request your bot initiates to fetch the image.
				// https://github.com/Microsoft/BotBuilder/issues/662
				var uri = new Uri(imageAttachment.ContentUrl);
				

				return await httpClient.GetStreamAsync(uri);
			}
		}
	}
}