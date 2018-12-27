using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BotAthenas.Dialogs;
using BotAthenas.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace BotAthenas
{
    public static class DocumentDBRepository<T> where T : class
    {

        /*		private static readonly string Endpoint = "https://localhost:8081";
                private static readonly string Key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
                private static readonly string DatabaseId = "Athena";
                private static string CollectionId = "Collection";
                private static DocumentClient client;*/

        private static readonly string Endpoint = "https://sn-athena-dev2.documents.azure.com:443/";
		private static readonly string Key = "3NR1lbh0SBxXgq2F64ZRRvlpXdUsXxrjnJJ4ZqOqQEG28gALXUxjjWBbcaeZU6PpUXcgWpeBTtu68m5rLsIm5w==";
		private static readonly string DatabaseId = "Athena";
		private static string CollectionId = "Collection";
		private static DocumentClient client;
		

		public static async Task<AgendamentoBot> GetAgendamentoBotAsync(string cpf)
        {

            AgendamentoBot agendamentoBot = new AgendamentoBot();

            agendamentoBot = client.CreateDocumentQuery<AgendamentoBot>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), 
                    new FeedOptions
                    {
                        MaxItemCount = -1,
                        EnableCrossPartitionQuery = true
                    })
                    .Where(x => x.CpfCliente == cpf)
                    .AsEnumerable()
                    .FirstOrDefault();
                // Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, cpf));
                //return (T)(dynamic)document;
                return agendamentoBot;


           
        }

        public static async Task<Servico> GetServicoAsync(string nome)
        {

            Servico servico = new Servico();

            servico = client.CreateDocumentQuery<Servico>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                    new FeedOptions
                    {
                        MaxItemCount = -1,
                        EnableCrossPartitionQuery = true
                    })
                    .Where(x => x.Nome == nome)
                    .AsEnumerable()
                    .FirstOrDefault();
            // Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, cpf));
            //return (T)(dynamic)document;
            return servico;


        }




		/****************************************************SERVICO*********************************************************/

		public static async Task<List<Servico>> GetItemAsyncServico(string idAdm, string idCat)
		{
			try
			{
				Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, idAdm));
				Administrador adm = (Administrador)(dynamic)document;

				if (adm != null)
				{
					List<Servico> results = new List<Servico>();
					foreach (Pessoajuridica p in adm.PessoaJuridica)
					{
						foreach (Categoria c in p.Categoria)
						{
							foreach (Servico s in c.Servico)
							{
								results.Add(s);
							}
						}
					}

					return results.Where(x => x.IdCategoria == idCat).ToList();
				}
				else
				{
					return null;
				}
			}
			catch (DocumentClientException e)
			{
				if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
				{
					return null;
				}
				else
				{
					return null;
				}
			}
		}




		public static async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }



        public static async Task<Document> CreateItemAsync(T t)
        {
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), t);
        }



        public static async Task<Document> UpdateItemAsync(string id, T t)
        {
            return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), t);
        }



        public static async Task DeleteItemAsync(string id, string category)
        {
            await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        }




		/***********************************************PROFISSIONAIS***************************************************/
		public static async	Task<List<Profissional>> GetItemAsyncProfissional(string idAdm, string idServ)
		{
		        try
                {
                    client = new DocumentClient(new Uri(Endpoint), Key, new ConnectionPolicy { EnableEndpointDiscovery = false });

                    var acesso = UriFactory.CreateDocumentUri(DatabaseId, CollectionId, idAdm);
                    Document document = await client.ReadDocumentAsync(acesso);
                    Administrador adm = (Administrador)(dynamic)document;

                    List<Profissional> results = new List<Profissional>();
                    foreach (Pessoajuridica p in adm.PessoaJuridica)
                    {
                        foreach (Categoria c in p.Categoria)
                        {
                            foreach (Servico s in c.Servico)
                            {
                                foreach (Profissional pro in s.Profissional)
                                {
                                    results.Add(pro);
                                }
                            }
                        }
                    }

                    return results.Where(x => x.IdServico == idServ).ToList();
                }
                catch (Exception e)
                {
                    return null;
                }
		}




		//Inicializa as collections especificadas (método chamado na classe Startup)
		public static void Initialize(string collectionId)
		{
			CollectionId = collectionId;
			client = new DocumentClient(new Uri(Endpoint), Key, new ConnectionPolicy { EnableEndpointDiscovery = false });
			CreateDatabaseIfNotExistsAsync().Wait();
			CreateCollectionIfNotExistsAsync().Wait();
		}



		private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }



        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection
                        {
                            Id = CollectionId
                        },
                        new RequestOptions { OfferThroughput = 400 });
                }
                else
                {
                    throw;
                }
            }
        }






    }
}