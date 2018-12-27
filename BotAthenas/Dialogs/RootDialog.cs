using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdaptiveCards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;

namespace BotAthenas.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Value != null)
            {
                var response = activity.Value as JObject;
                //var name = response["nome"].Value<string>();
                var dia = response["dia"].Value<string>();
                var horario = response["horario"].Value<string>();

                await context.PostAsync($"Dia: { dia}, horario: { horario}");
                context.Wait(MessageReceivedAsync);
                return;
            }
            var message = activity.CreateReply();

            if (activity.Text.Equals("form", StringComparison.InvariantCultureIgnoreCase))
            {
                var adaptive = GetAdaptiveCard();
                message.Attachments.Add(adaptive);
            }

            await context.PostAsync(message);
            context.Wait(MessageReceivedAsync);
        }

        private Attachment GetAdaptiveCard()
        {
            var card = new AdaptiveCard
            {

            };

            card.Body.Add(new TextBlock()
            {
                Text = "Escolha o dia"
            });

            card.Body.Add(new ChoiceSet()
            {
                Id = "dia",
                Style = ChoiceInputStyle.Compact,
                Choices = new List<Choice>()
                {
                    new Choice() { Title = "01/11", Value = "1", IsSelected = true },
                    new Choice() { Title = "02/11", Value = "2" },
                    new Choice() { Title = "03/11", Value = "3" }
                }
            });

            card.Body.Add(new TextBlock()
            {
                Text = "Escolha o horário"
            });

            card.Body.Add(new ChoiceSet()
            {
                Id = "horario",
                Style = ChoiceInputStyle.Compact,
                Choices = new List<Choice>()
                {
                    new Choice() { Title = "12:00h", Value = "12:00h", IsSelected = true },
                    new Choice() { Title = "12:30h", Value = "12:30h" },
                    new Choice() { Title = "13:00h", Value = "13:00h" }
                }
            });

            card.Actions.Add(new SubmitAction
            {
                Title = "Enviar",
                Data = new { id = 1, nome = "xpto" }
            });

            var attachment = new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            return attachment;
        }
    }
}