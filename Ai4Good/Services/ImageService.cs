using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ai4Good.Models;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;

namespace Ai4Good.Services
{
	public class ImageService
	{
		public List<ImageModel> ImageModels { get; set; }
		public ImageModel ImageAnswer { get; set; }
	

		public ImageService() {
			ImageModels = Initialize();
		}




		private List<ImageModel> Initialize()
		{
			ImageModels = new List<ImageModel>();

			ImageModels.Add(new ImageModel(1, "lion", "http://elelur.com/data_images/mammals/lion/lion-01.jpg"));
			ImageModels.Add(new ImageModel(2, "tiger", "http://elelur.com/data_images/mammals/lion/lion-01.jpg"));
			ImageModels.Add(new ImageModel(3, "car", "http://elelur.com/data_images/mammals/lion/lion-01.jpg"));

			return ImageModels;
		}

		public ImageModel GiveQuestion()
		{

			ImageModel ImageAnswer = null;
			//PromptOptions<string> options = new PromptOptions<string>("Select which of is a lion", "Sorry please try again", "I give up on you.", goodTeams, 2);
			return ImageAnswer;

		}

		public List<string> GetThreeImages()
		{
			List<string> results = new List<string>();

			var images = ImageModels.Take(3);

			foreach (var image in images)
			{
				results.Add(image.Name);
			}

			return results;
		}






	}
}