using AdaptiveCards;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using BotAthenas.Models;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Internals.Fibers;

namespace BotAthenas.Dialogs
{
	public class Cards
	{

		public static Attachment AgendarCard()
		{
			

			string idAdm = "5ff8cb9e-4cb8-4e5a-9c23-766926628ff9";
            string idServico = "0fe0d37d-05fe-47ff-b0c3-deeb3c21f3b8";
			string idCat = "65719170-be04-420d-9aa0-72f3eaf6c13d";

			List<Profissional> profissionais = AsyncHelpers.RunSync<List<Profissional>>(() => DocumentDBRepository<Profissional>.GetItemAsyncProfissional(idAdm, idServico));
			//List<Servico> servicos = AsyncHelpers.RunSync<List<Servico>>(() => DocumentDBRepository<Servico>.GetItemAsyncServico(idAdm, idCat));
			
			List<Choice> carregandoListaProfissionais()
			{
				List<Choice> escolhas = new List<Choice>();
				foreach (Profissional p in profissionais)
				{
					escolhas.Add(new Choice() { Title = p.NomeCompleto.ToString(), Value = p.NomeCompleto.ToString(), IsSelected = true });
				}
				return escolhas;
			}

			/*List<Choice> carregandoListaServicos()
			{
				List<Choice> escolhas = new List<Choice>();
				foreach (Servico s in servicos)
				{
					escolhas.Add(new Choice() { Title = s.Nome.ToString(), Value = s.Nome.ToString(), IsSelected = true });
				}
				return escolhas;
			}*/


			var card = new AdaptiveCard
			{

			};
			card.Body.Add(new TextBlock()
			{
				Text = "Informe o seu nome"
			});
			card.Body.Add(new TextInput()
			{
				Id = "nome"
			});
			card.Body.Add(new TextBlock()
			{
				Text = "Informe o seu cpf"
			});
			card.Body.Add(new TextInput()
			{
				Id = "cpf"
			});
			card.Body.Add(new TextBlock()
			{
				Text = "Informe o funcionário"
			});

			card.Body.Add(new ChoiceSet()
			{
				Id = "funcionario",
				Style = ChoiceInputStyle.Compact,
				Choices = carregandoListaProfissionais()
		});

			

			/*card.Body.Add(new TextBlock()
			{
				Text = "Informe o serviço"
			});
			card.Body.Add(new ChoiceSet()
			{
				Id = "servico",
				Style = ChoiceInputStyle.Compact,
				Choices = carregandoListaServicos()
			});*/


			card.Body.Add(new TextBlock()
            {
                Text = "Informe o dia escolhido"
            });
            card.Body.Add(new ChoiceSet()
            {
                Id = "dia",
                Style = ChoiceInputStyle.Compact,
                Choices = new List<Choice>()
                {
                    new Choice() { Title = "23/11", Value = "23", IsSelected = true },
                    new Choice() { Title = "26/11", Value = "26" },
                    new Choice() { Title = "27/11", Value = "27" },
                    new Choice() { Title = "28/11", Value = "28" },
                    new Choice() { Title = "29/11", Value = "29" },
                    new Choice() { Title = "30/11", Value = "30" }

                }
            });
            card.Body.Add(new TextBlock()
            {
                Text = "Informe o horário escolhido"
            });
            card.Body.Add(new ChoiceSet()
            {
                Id = "horario",
                Style = ChoiceInputStyle.Compact,
                Choices = new List<Choice>()
                {
                    new Choice() { Title = "12:00h", Value = "12:00h", IsSelected = true },
                    new Choice() { Title = "12:30h", Value = "12:30h" },
                    new Choice() { Title = "13:00h", Value = "13:00h" },
                    new Choice() { Title = "13:30h", Value = "13:30h" },
                    new Choice() { Title = "14:00h", Value = "14:00h" },
                    new Choice() { Title = "14:30h", Value = "14:30h" },
                    new Choice() { Title = "15:00h", Value = "15:00h" },
                    new Choice() { Title = "15:30h", Value = "15:30h" },
                    new Choice() { Title = "16:00h", Value = "16:00h" },
                    new Choice() { Title = "16:30h", Value = "16:30h" },
                    new Choice() { Title = "17:30h", Value = "17:00h" },
                    new Choice() { Title = "17:30h", Value = "17:30h" },
                    new Choice() { Title = "18:00h", Value = "18:00h" },
                }
            });
            card.Actions.Add(new SubmitAction
            {
                Title = "Enviar",
                Data = new { id = 1, nome = "xpto" }
            });
            var  attachment = new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment ConsultarCard()
        {
            var card = new AdaptiveCard
            {

            };
            card.Body.Add(new TextBlock()
            {
                Text = "Informe o seu nome"
            });
            card.Body.Add(new TextInput()
            {
                Id = "nome"
            });
            card.Body.Add(new TextBlock()
            {
                Text = "Informe o seu cpf"
            });
            card.Body.Add(new TextInput()
            {
                Id = "CpfCliente"
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

        public static Attachment Servicos()
        {
            var card = new AdaptiveCard
            {

            };
            card.Body.Add(new TextBlock()
            {
                Text = "Informe o serviço desejado"
            });
            card.Body.Add(new TextInput()
            {
                Id = "servico"
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

        /*public static ReceiptCard ReciboCard()
        {
        
        }*/
    }
}