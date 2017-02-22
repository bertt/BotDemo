using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using BotDemo;

namespace Bot_Application1
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            if (activity?.Type == ActivityTypes.Message)
                await Conversation.SendAsync(activity, () => new IntakeReportDialog());
            else
            {
                if (activity?.Type == ActivityTypes.ConversationUpdate)
                {
                    Activity reply = activity.CreateReply("Welcom at the Spoorweb chat bot.Type something to start the intake.");
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}