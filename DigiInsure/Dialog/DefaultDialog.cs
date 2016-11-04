using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.Bot.Connector;

namespace DigiInsure.Dialog
{
    [LuisModel("924e7c0b-9db7-4307-bfac-0c9b984d1985", "18192dcda51840a48530d8a772cadd77")]
    [Serializable]
    public class DefaultDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message;
            if (result.Query.ToUpper().Contains("THANK"))
            {
                message = "";
            }
            else if (result.Query.ToUpper().Contains("1"))
            {
                message = "Thank you, you want to find your home address?";               
            }
            else
            {
                message = $"Hello, Welcome to DigiInsure, how can I help you?";
            }
            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("First Name")]
        public async Task FirstName(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Please provide your first name?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Accidental damage cover intent")]
        public async Task AccidentalDamageOption(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Please provide your first name?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Number of Years Insurance Held")]
        public async Task NumberOfInsuranceyears(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Please provide your first name?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Smoke at property")]
        public async Task SmokeAtProperty(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Please provide your first name?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Cover Start Date Intent")]
        public async Task CoverStartDate(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Please provide your first name?");
            context.Wait(MessageReceived);
        }


        [LuisIntent("Home insurance Intent")]
        public async Task HomeInsurance(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hello, Let's get you some great insurance. This will only take a minute");

            await context.PostAsync("Please provide your personnummer");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Personnummer")]
        public async Task Personnummer(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hello, Let's get you some great insurance. This will only take a minute");

            await context.PostAsync("Please provide your personnummer");
        }

        [LuisIntent("Number Of Adults")]
        public async Task NumberOfAdults(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hello, Let's get you some great insurance. This will only take a minute");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Number of children")]
        public async Task NumberOfChildren(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hello, Let's get you some great insurance. This will only take a minute");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Property Ownership Intent")]
        public async Task PropertyOwnerShipOption(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hello, Let's get you some great insurance. This will only take a minute");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Home Address")]
        [LuisIntent("Address")]
        [LuisIntent("Yes")]
        public async Task ConfirmHomeAddress(IDialogContext context, LuisResult result)
        {
            PromptDialog.Confirm(
                         context: context,
                         resume: SelectACover,
                         prompt: $"I see that your adddress is Vasagatan 14, Stockholm 11448, Please confirm?",
                         retry: "I didn't understand. Please try again.");
        }

        public async Task SelectACover(IDialogContext context, IAwaitable<bool> result)
        {
            PromptDialog.Choice(
                context: context,
                resume: AfterCoverSelection,
                options: Enum.GetValues(typeof(CoverOptions)).Cast<CoverOptions>().ToArray(),
                prompt: "Please select the cover",
                retry: "I didn't understand. Please try again.");
        }

        [LuisIntent("Hi")]
        public async Task Welcome(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hello, Welcome to DigiInsure, how can I help you?");
            context.Wait(MessageReceived);
        }

        public async Task AfterCoverSelection(IDialogContext context, IAwaitable<CoverOptions> choice)
        {
            CoverOptions option = await choice;
            string message = $"Thank you for selecting '{option}', few more questions";
            await context.PostAsync(message);
            PromptDialog.Choice(
                         context: context,
                         resume: AfterPropertyTypeSelection,
                         options: Enum.GetValues(typeof(Ownership)).Cast<Ownership>().ToArray(),
                         prompt: "Please tell about property type?",
                         retry: "I didn't understand. Please try again.");
        }

        private async Task AfterPropertyTypeSelection(IDialogContext context, IAwaitable<Ownership> choice)
        {
            Ownership option = await choice;

            PromptDialog.Confirm(
                    context: context,
                    resume: ConfirmSmoker,
                    prompt: "Does any of the tenants smoke?",
                    retry: "I didn't understand. Please try again.");
        }

        private async Task ConfirmSmoker(IDialogContext context, IAwaitable<bool> result)
        {
            bool message = await result;
            PromptDialog.Choice(
                         context: context,
                         resume: AccidentalDamagerCover,
                         options: Enum.GetValues(typeof(InsuranceHeldInYears)).Cast<InsuranceHeldInYears>().ToArray(),
                         prompt: "How many consecutive years have you held insurance (in years)?",
                         retry: "I didn't understand. Please try again.");
        }

        private async Task ConfirmAddress(IDialogContext context, IAwaitable<bool> result)
        {
            bool message = await result;
            PromptDialog.Choice(
                         context: context,
                         resume: AccidentalDamagerCover,
                         options: Enum.GetValues(typeof(InsuranceHeldInYears)).Cast<InsuranceHeldInYears>().ToArray(),
                         prompt: "How many consecutive years have you held insurance (in years)?",
                         retry: "I didn't understand. Please try again.");
        }

        private async Task AccidentalDamagerCover(IDialogContext context, IAwaitable<InsuranceHeldInYears> result)
        {
            InsuranceHeldInYears message = await result;
            PromptDialog.Choice(
                         context: context,
                         resume: CoverStartDate,
                         options: Enum.GetValues(typeof(AccidentalDamageCover)).Cast<AccidentalDamageCover>().ToArray(),
                         prompt: "Would you like accidental damage cover for your building?",
                         retry: "I didn't understand. Please try again.");
        }

        private async Task CoverStartDate(IDialogContext context, IAwaitable<AccidentalDamageCover> result)
        {
            AccidentalDamageCover option = await result;
            DateTime dt = DateTime.Now;
            string formatted = dt.ToString("dd-MMM-yyyy");
            string message = $"Your cover start date is : {formatted}, do you want to change it? Please note Cover start must be within the next 90 days.";
            PromptDialog.Confirm(
                           context: context,
                           resume: ConfirmCoverStartDate,
                           prompt: message,
                           retry: "I didn't understand. Please try again.");
        }

        private async Task ConfirmCoverStartDate(IDialogContext context, IAwaitable<bool> result)
        {
            bool option = await result;
            string message = $"Thank you for details, generating premium...";
            await context.PostAsync(message);
            Random r = new Random();
            int n = r.Next(52);
            DateTime dt = DateTime.Now;
            DateTime answer = dt.AddDays(90);
            string quoteExpiryDate = answer.ToString("dd-MMM-yyyy");
            string quoteMessage = $"Your quote reference: QTE066756{n}, (Valid for 90 days until {quoteExpiryDate})";
            await context.PostAsync(quoteMessage);
            PromptDialog.Confirm(
                          context: context,
                          resume: ConfirmQuote,
                          prompt: $"Your annual premium is SEK 1200. Would you like to buy it?",
                          retry: "I didn't understand. Please try again.");
        }

        private async Task ConfirmQuote(IDialogContext context, IAwaitable<bool> result)
        {
            bool option = await result;
            PromptDialog.Confirm(
                         context: context,
                         resume: ThankYou,
                         prompt: $"Please validate your payment via preconfigured account",
                         retry: "I didn't understand. Please try again.");
            //Activity replyToConversation = _message.CreateReply("Should go to conversation, with a carousel");
            //replyToConversation.Recipient = _message.From;
            //replyToConversation.Type = "message";
            //replyToConversation.AttachmentLayout = "carousel";
            //await context.PostAsync(replyToConversation);
            //context.Wait(MessageReceived);          
        }

        private async Task ThankYou(IDialogContext context, IAwaitable<bool> result)
        {
            bool option = await result;
            string message = $"Thank you for Insuring with us. The policy documents sent to your email";
            await context.PostAsync(message);
        }

        public enum CoverOptions
        {
            BuildingsandContents,
            BuildingsOnly,
            ContentsOnly
        }

        public enum Ownership
        {
            Rented,
            Owned
        }
        public enum InsuranceHeldInYears
        {
            None,
            One,
            Two,
            Three,
            Four,
            FivePlus
        }
        public enum AccidentalDamageCover
        {
            None,
            OneMil,
            FiveMil
        }
    }
}