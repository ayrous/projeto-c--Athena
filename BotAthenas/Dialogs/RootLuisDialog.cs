using AdaptiveCards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Web.Mvc;
using BotAthenas.Models;
using System.ComponentModel.DataAnnotations;

namespace BotAthenas.Dialogs
{
    /*[LuisModel("Application ID", "keys and endpoint")]*/
    [LuisModel("69978bf1-030b-44a5-b975-ed4bf4fb7bb0", "55e6b94f207e47879bfa7a2da77d9385")]
    [Serializable]
    public class RootLuisDialog : LuisDialog<object>
    {
        // CONSTANTS        
        // Entity
        public const string Entity_Device = "HomeAutomation.Device";
        public const string Entity_Room = "HomeAutomation.Room";
        public const string Entity_Operation = "HomeAutomation.Operation";

        public const string Entity_Critica = "Critica";
        public const string Entity_Dia = "Dia";
        public const string Entity_Elogio = "Elogio";
        public const string Entity_Funcionario = "Funcionario";
        public const string Entity_Horario = "Horario";
        public const string Entity_Local = "Local";
        public const string Entity_Servico = "Servico";

        // Intents
        public const string Intent_TurnOn = "HomeAutomation.TurnOn";
        public const string Intent_TurnOff = "HomeAutomation.TurnOff";
        public const string Intent_None = "None";

        public const string Cadastro = "CadastroBanco";

        public const string Intent_ConsultarC = "ConsultarCliente";
        public const string Intent_ConsultarF = "ConsultarFuncionario";
        public const string Intent_DesmarcarC = "DesmarcarCliente";
        public const string Intent_DesmarcarF = "DesmarcarFuncionario";
        public const string Intent_MarcarC = "MarcarCliente";
        public const string Intent_RemarcarC = "RemarcarCliente";
        public const string Intent_RemarcarF = "RemarcarFuncionario";

        public const string Intent_Agradecimento = "Agradecimento";
        public const string Intent_Criticar = "Criticar";
        public const string Intent_Cumprimentar = "Cumprimentar";
        public const string Intent_Despedir = "Despedir";
        public const string Intent_Elogiar = "Elogiar";

        [LuisIntent(Cadastro)]
        public async Task Cadastrar(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Chamado Cadastrar");
            context.Wait(MessageReceived);
        }
        /*------------------------------------------------------------------------------------------------------------------------------*/
        /*------------------------------------------------------------------------------------------------------------------------------*/
        /*------------------------------------------------------------------------------------------------------------------------------*/
        /*------------------------------------------------------------------------------------------------------------------------------*/
        /*------------------------------------------------------------------------------------------------------------------------------*/
        /*------------------------------------------------------------------------------------------------------------------------------*/
        [LuisIntent(Intent_Agradecimento)]
        public async Task Agradecimento(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Por nada, se eu puder ajudar em mais alguma coisa pode me chamar.");
            context.Wait(MessageReceived);
        }

        [LuisIntent(Intent_ConsultarC)]
        public async Task ConsultarCliente(IDialogContext context, IAwaitable<object> result, LuisResult resultLuis)
        {
            //await context.PostAsync("Chamado ConsultarCliente");

            var activity = await result as Activity;

            var message = activity.CreateReply();
            
            var adaptive = Cards.ConsultarCard();
            message.Attachments.Add(adaptive);
            

            await context.PostAsync(message);
            context.Wait(TrazerDados);

            //context.Wait(MessageReceived);
        }

        private async Task TrazerDados(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            var response = activity.Value as JObject;
			var nome = response["nome"].Value<string>();
			var CpfCliente = response["CpfCliente"].Value<string>();
			
			if (activity.Value != null) {
                AgendamentoBot agendamento = await DocumentDBRepository<AgendamentoBot>.GetAgendamentoBotAsync(CpfCliente);

                if (agendamento != null)
                {
                await context.PostAsync($"{agendamento.NomeCliente}, seu agendamento esta marcado pro dia {agendamento.Dia}, as {agendamento.Horario}" +
                $" com {agendamento.Funcionario}!! Até la!!");
                }
                else
                {
                    await context.PostAsync("Você não possui agendamentos");
                }

                if (activity.Value == null)
                {
                    await context.PostAsync("Insira os dados, por favor");
                }
                
                
            }
            context.Wait(MessageReceived);
            
        }

        [LuisIntent(Intent_ConsultarF)]
        public async Task ConsultarFuncionario(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Chamado ConsultarFuncionario");
            context.Wait(MessageReceived);
        }

        [LuisIntent(Intent_DesmarcarC)]
        public async Task DesmarcarCliente(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Chamado DesmarcarCliente");
            context.Wait(MessageReceived);
        }

        [LuisIntent(Intent_DesmarcarF)]
        public async Task DesmarcarFuncionario(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Chamado DesmarcarFuncionario");
            context.Wait(MessageReceived);
        }

        private async Task DevolverDados(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            var response = activity.Value as JObject;

            var nome = response["nome"].Value<string>();
            var cpf = response["cpf"].Value<string>();
            var dia = response["dia"].Value<string>();
            var horario = response["horario"].Value<string>();
            var funcionario = response["funcionario"].Value<string>();
			//var servico = response["servico"].Value<string>();

            AgendamentoBot agendamentoBanco = await DocumentDBRepository<AgendamentoBot>.GetAgendamentoBotAsync(cpf);
			if (nome.ToString() == null || dia.ToString() == null || horario.ToString() == null || funcionario.ToString() == null || cpf.ToString() == null)
			{

				if (nome == null && dia == null && horario == null && funcionario == null)
				{
					context.PostAsync("Todos os dados são requeridos!");

				}
				context.PostAsync("Todos os dados são requeridos!");
			} else if (nome != null && dia != null && horario != null && funcionario != null) {
				
				/*TODO: SE O CPF FOR IGUAL AO DO BANCO N DEIXAR SALVAR!
				 * if (cpf == null)
				{
					context.PostAsync("Este CPF ja foi usado. Por favor, verifique o dado inserido!!");

				}*/ if (activity.Value != null)
				{
					AgendamentoBot agendamento = new AgendamentoBot();
					agendamento.NomeCliente = nome;
					agendamento.Dia = dia;
					agendamento.Horario = horario;
					//agendamento.Servico = servico;
					agendamento.CpfCliente = cpf;
					agendamento.Funcionario = funcionario;

					await DocumentDBRepository<AgendamentoBot>.CreateItemAsync(agendamento);



					await context.PostAsync($"Marcado com sucesso!!");
				}
			}

            context.Wait(MessageReceived);
            
        }

        [LuisIntent(Intent_MarcarC)]
        private async Task MarcarCliente(IDialogContext context, IAwaitable<object> result, LuisResult resultLuis)
        {
            /*await context.PostAsync("Chamado MarcarCliente");
            context.Wait(MessageReceived);*/

            var activity = await result as Activity;

            var message = activity.CreateReply();

            var adaptive = Cards.AgendarCard();
            message.Attachments.Add(adaptive);
            
            await context.PostAsync(message);
            context.Wait(DevolverDados);
        }



        [LuisIntent(Intent_RemarcarC)]
        public async Task RemarcarCliente(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Chamado RemarcarCliente");
            context.Wait(MessageReceived);
        }

        [LuisIntent(Intent_RemarcarF)]
        public async Task RemarcarFuncionario(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Chamado RemarcarFuncionario");
			context.Wait(MessageReceived);
        }

        [LuisIntent(Intent_Criticar)]
        public async Task Criticar(IDialogContext context, IAwaitable<object> result, LuisResult luisResult)
        {

            await context.PostAsync("Desculpe, irei melhorar meus serviços!!!!");
            context.Wait(MessageReceived);
            //var activity = await result as Activity;

            //var message = activity.CreateReply();

            /*var adaptive = Cards.Servicos();
            message.Attachments.Add(adaptive);

            await context.PostAsync(message);
            context.Wait(DevolverCriticar);*/

        }

        /*public async Task DevolverCriticar(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            var response = activity.Value as JObject;

            var servico = response["servico"].Value<string>();

            if (activity.Value != null)
            {
                Servico servicoBanco = await DocumentDBRepository<Servico>.GetServicoAsync(servico);

                await context.PostAsync($"{servicoBanco.Profissional}");
            }

        }*/

        int cont = 0;
		[LuisIntent(Intent_Cumprimentar)]
		public async Task Cumprimentar(IDialogContext context, LuisResult result)
		{

			if (cont == 0) {
				await context.PostAsync("Olá, tudo bem?");
				context.Wait(MessageReceived);
				cont++;
			} else if (cont == 1) {

				await context.PostAsync("Estou muito bem! O que gostaria hoje?");
				context.Wait(MessageReceived);
				cont++;

			} else if (cont > 1) {
				await context.PostAsync("O que gostaria hoje?");
				context.Wait(MessageReceived);
				cont++;

			}
		}


        [LuisIntent(Intent_Despedir)]
        public async Task Despedir(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Tchau, até mais!!");
            context.Wait(MessageReceived);
        }

        [LuisIntent(Intent_Elogiar)]
        public async Task Elogiar(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Muito obrigado, continuarei evoluindo para melhor atendê-los!!");
            context.Wait(MessageReceived);
        }

        /*------------------------------------------------------------------------------------------------------------------------------*/
        /*------------------------------------------------------------------------------------------------------------------------------*/
        /*------------------------------------------------------------------------------------------------------------------------------*/
        /*------------------------------------------------------------------------------------------------------------------------------*/
        /*------------------------------------------------------------------------------------------------------------------------------*/
        /*------------------------------------------------------------------------------------------------------------------------------*/
        [LuisIntent(Intent_None)]
        public async Task NoneAsync(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Desculpe, eu não entendi...");
            context.Wait(MessageReceived);
        }

        [LuisIntent(Intent_TurnOn)]
        public async Task OnIntent(IDialogContext context, LuisResult result)
        {
            //await this.ShowLuisResult(context, result);

            /*-------------------------------------------------------------------------------------------------------------*/
            //await context.PostAsync($"Looking for reviews of coisa...");

            var resultMessage = context.MakeMessage();
            resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            resultMessage.Attachments = new List<Attachment>();

            //var heroCard = GetHeroCard();

            var adaptive = Cards.AgendarCard();


            resultMessage.Attachments.Add(adaptive);


            await context.PostAsync(resultMessage);

            context.Wait(this.MessageReceived);
            /*-------------------------------------------------------------------------------------------------------------*/

            //await context.PostAsync("Deu turn on");

            //var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            //var activity = await result as Activity;

            //var message = activity.CreateReply($"Mensagem");

            //////////////var card = GetAdaptiveCard();
            //////////////message.Attachments.Add(card);

            ////var hero = GetHeroCard();
            ////message.Attachments.Add(hero);

            ////await context.PostAsync(message);

            ////context.Wait(OnIntent);

            //var adaptive = GetAdaptiveCard();
            //message.Attachments.Add(adaptive);

            //await context.PostAsync("Deu turn on");

            //context.Wait(MessageReceived);

            //await context.PostAsync(message);

            //await connector.Conversations.ReplyToActivityAsync(message);

            //await connector.Conversations.SendToConversationAsync((Activity)message);


        }

        private IMessageActivity GetMessage(IDialogContext context, AdaptiveCard card, string cardName)
        {
            var message = context.MakeMessage();
            if (message.Attachments == null)
                message.Attachments = new List<Attachment>();
            var attachment = new Attachment()
            {
                Content = card,
                ContentType = "application/vnd.microsoft.card.adaptive",
                Name = cardName
            };
            message.Attachments.Add(attachment);
            return message;
        }



        [LuisIntent(Intent_TurnOff)]
        public async Task OffIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        private async Task ShowLuisResult(IDialogContext context, LuisResult result)
        {
            // get recognized entities
            string entities = this.BotEntityRecognition(result);

            // round number
            string roundedScore = result.Intents[0].Score != null ? (Math.Round(result.Intents[0].Score.Value, 2).ToString()) : "0";

            await context.PostAsync($"**Query**: {result.Query}, **Intent**: {result.Intents[0].Intent}, **Score**: {roundedScore}. **Entities**: {entities}");
            context.Wait(MessageReceived);
        }
        // Entities found in result
        public string BotEntityRecognition(LuisResult result)
        {
            StringBuilder entityResults = new StringBuilder();

            if (result.Entities.Count > 0)
            {
                foreach (EntityRecommendation item in result.Entities)
                {
                    // Query: Turn on the [light]
                    // item.Type = "HomeAutomation.Device"
                    // item.Entity = "light"
                    entityResults.Append(item.Type + "=" + item.Entity + ",");
                }
                // remove last comma
                entityResults.Remove(entityResults.Length - 1, 1);
            }

            return entityResults.ToString();
        }

        private Attachment GetHeroCard()
        {
            var heroCard = new HeroCard
            {
                Title = "Teste Card",
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.OpenUrl, "Google", null, "http://www.google.com")
                }
            };

            return heroCard.ToAttachment();
        }

        

    }

    public class AgendamentoBot
    {
        [Key]
        [JsonProperty("cpfCliente")]
        public string CpfCliente { get; set; }
        [JsonProperty("dia")]
        public string Dia { get; set; }
        [JsonProperty("horario")]
        public string Horario { get; set; }
        [JsonProperty("funcionario")]
        public string Funcionario { get; set; }
		//[JsonProperty("servico")]
		//public string Servico { get; set; }
		[JsonProperty("nomeCliente")]
        public string NomeCliente { get; set; }
    }
}