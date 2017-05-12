using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ai4Good.Models
{
	public class ImageModel
	{
		public int Id { get; set; } 
		public string Name { get; set; }
		public string Url { get; set; }

		public string ContentType { get; set; }


		public ImageModel() { }

		public ImageModel(int id, string name, string url) {
			this.Id = id;
			this.Name = name;
			this.Url = url;
			this.ContentType = "image/png";
		}

	}



}