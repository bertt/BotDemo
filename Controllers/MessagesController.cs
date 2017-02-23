using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using BotDemo;
using System.Linq;

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
                    if (activity.MembersAdded != null && activity.MembersAdded.Any())
                    {
                        foreach (var newMember in activity.MembersAdded)
                        {
                            if (newMember.Id != activity.Recipient.Id)
                            {
                                Activity reply = activity.CreateReply("Welcome at the Spoorweb chat bot.Type something to start the intake.");
                                var attachment = new Attachment();
                                attachment.ContentType = "image/png";
                                attachment.ContentUrl = "http://slideplayer.nl/46/11688212/big_thumb.jpg";
                                reply.Attachments.Add(attachment);
                                await connector.Conversations.ReplyToActivityAsync(reply);
                            }
                        }
                    }
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}