﻿using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotDemo
{
    [Serializable]
    public class IntakeReportDialog : IDialog<object>
    {
        IncidentTypes incidentTypes;
        string location;
        bool attachmentNeeded;
        //Attachment attachment;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(CoversationStartedAsync);
        }

        public async Task CoversationStartedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            IMessageActivity activity = await argument;
            await context.PostAsync(activity.Text);
            PromptDialog.Choice(
                context: context,
                resume: ResumeAndPromptLocationAsync,
                options: Enum.GetValues(typeof(IncidentTypes)).Cast<IncidentTypes>().ToArray(),
                prompt: "1. Please select the incidenttype",
                retry: "I didn't understand. Please try again."
                );
        }

        public async Task ResumeAndPromptLocationAsync(IDialogContext context, IAwaitable<IncidentTypes> argument)
        {
            if (argument != null)
            {
                incidentTypes = await argument;
            }
            PromptDialog.Text(
                context: context,
                resume: ResumeAndHandleDrp,
                prompt: "2. Give me dienstregelpunt afkorting (bv 'Ut'):",
                retry: "I didn't understand. Please try again."

                );
        }

        public async Task ResumeAndHandleDrp(IDialogContext context, IAwaitable<string> argument)
        {
            var drp = await argument;
            var upper = char.ToUpper(drp[0]) + drp.Substring(1);
            var root = ProrailServices.GetDrp(upper);
            if (root.features.Count() > 0)
            {
                await context.PostAsync($"Dientregelpunt gevonden :-) " + root.features[0].attributes.NAAM);
                location = root.features[0].attributes.NAAM;
            }
            else
            {
                location = "";
                await context.PostAsync($"Dienstregelpunt niet gevonden :-( Probeer het nog eens...");
                // context.Wait(ResumeAndPromptLocationAsync(context);
            }
            await ResumeAndPromptAttachmentNeeded(context);
        }

        public async Task ResumeAndPromptAttachmentNeeded(IDialogContext context)
        {
            // location = await argument;
            PromptDialog.Confirm(
                context: context,
                resume: ResumeAndHandleAttachmentNeededAsync,
                prompt: $"3. Do you want to add attachment (like a picture?)",
                retry: "I didn't understand. Please try again.");
        }

        public async Task ResumeAndHandleAttachmentNeededAsync(IDialogContext context, IAwaitable<bool> argument)
        {

            attachmentNeeded = await argument;

            if (attachmentNeeded)
            {
                PromptDialog.Attachment(
                    context: context,
                    resume: ResumeAndHandleAttachmentAsync,
                    prompt: $"3a Please add attachment",
                    retry: "I didn't understand. Please try again.");
            }
            else
            {
                await ResumeAndPromptSummaryAsync(context);
            }

        }

        public async Task ResumeAndHandleAttachmentAsync(IDialogContext context, IAwaitable<IEnumerable<Attachment>> argument)
        {
            // attachment = (await argument).ToList()[0];
            await ResumeAndPromptSummaryAsync(context);
        }


        public async Task ResumeAndPromptSummaryAsync(IDialogContext context)
        {
            PromptDialog.Confirm(
                context: context,
                resume: ResumeAndHandleConfirmAsync,
                prompt: $"4. You entered type: '{incidentTypes}', location: '{location}', attachment: {attachmentNeeded} Is that correct?",
                retry: "I didn't understand. Please try again.");
        }

        public async Task ResumeAndHandleConfirmAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            bool choicesAreCorrect = await argument;

            if (choicesAreCorrect)
                await context.PostAsync("5. Your incident intake has been submitted. Thanks for the feedback!");
            else
                await context.PostAsync("I see. You're welcome to try again.");

            context.Wait(CoversationStartedAsync);
        }
    }
}