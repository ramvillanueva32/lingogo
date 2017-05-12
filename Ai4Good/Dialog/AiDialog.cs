using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Threading.Tasks;

namespace Ai4Good.Dialog
{
	public class AiDialog : IDialog<object>
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
				await context.PostAsync("Please send me an image.");
				context.Wait(MessageReceivedStart);
			}
			else
			{

			}

		}
	}
}