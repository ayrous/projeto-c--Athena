using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Athenas.Controllers;
using Athenas.Domain;

namespace Athenas.Repository
{
    public class Repository<T> where T : class
    {
        private static readonly string Endpoint = "https://sn-athena-dev2.documents.azure.com:443/";
        private static readonly string Key = "3NR1lbh0SBxXgq2F64ZRRvlpXdUsXxrjnJJ4ZqOqQEG28gALXUxjjWBbcaeZU6PpUXcgWpeBTtu68m5rLsIm5w=="; //C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==
        private static readonly string DatabaseId = "Athena";
        private static string CollectionId = "Collection";
        private static DocumentClient client;

        // -------------------------------------------- BDO -------------------------------------
        //Inicializa as collections especificadas (método chamado na classe Startup)
        public static void Initialize(string collectionId)
        {
            CollectionId = collectionId;
            client = new DocumentClient(new Uri(Endpoint), Key, new ConnectionPolicy { EnableEndpointDiscovery = false });
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        //Verifica se determinado banco de dados existe e se não exisitr o cria
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

        //Verifica se uma collection existe e se não existir a cria
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

        // -------------------------------------------- TODOS -------------------------------------

        public static async Task<Document> CadastrarItem(T item)
        {
            try
            {
                return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public static async Task DeletarItem(string id)
        {
            try
            {
                await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
            }
            catch (Exception e)
            {
                throw null;
            }
        }

        // -------------------------------------------- ADMINISTRADOR -------------------------------------

        public static async Task<T> PegarAdm(string id)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return (T)(dynamic)document;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<IEnumerable<Administrador>> ListarAdm()
        {
            try
            {
                IDocumentQuery<Administrador> query = client.CreateDocumentQuery<Administrador>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                    new FeedOptions { MaxItemCount = -1 })
                    .AsDocumentQuery();

                List<Administrador> results = new List<Administrador>();
                while (query.HasMoreResults)
                {
                    results.AddRange(await query.ExecuteNextAsync<Administrador>());
                }

                return results;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        // Busca um administrador especificao por seu email
        public static async Task<Administrador> PegarAdmPorEmail(string email)
        {
            try
            {
                Administrador adm = new Administrador();

                adm = client.CreateDocumentQuery<Administrador>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                        .Where(x => x.Email == email)
                        .AsEnumerable()
                        .FirstOrDefault();
                return adm;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<Document> AtualizarAdm(string id, T item)
        {
            try
            {
                return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        // -------------------------------------------- PESSOA JURIDICA ------------------------------------- 

        // Busca pessoas juridicas de um determinado administrador
        public static async Task<IEnumerable<PessoaJuridica>> ListarPj(string idAdm)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, idAdm));
                Administrador adm = (Administrador)(dynamic)document;

                List<PessoaJuridica> results = new List<PessoaJuridica>();
                foreach (PessoaJuridica p in adm.PessoaJuridica)
                {
                    results.Add(p);
                }

                return results.Where(x => x.IdAdministrador == adm.Id).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        // Busca pessoas juridicas de um determinado administrador
        public static async Task<IEnumerable<PessoaJuridica>> ListarTodasPj(string idAdm)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, idAdm));
                Administrador adm = (Administrador)(dynamic)document;

                List<PessoaJuridica> results = new List<PessoaJuridica>();
                foreach (PessoaJuridica p in adm.PessoaJuridica)
                {
                    results.Add(p);
                }

                return results.ToList();
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static async Task<PessoaJuridica> PegarPj(string idAdm, string id)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, idAdm));
                Administrador adm = (Administrador)(dynamic)document;

                List<PessoaJuridica> results = new List<PessoaJuridica>();
                foreach (PessoaJuridica p in adm.PessoaJuridica)
                {
                    results.Add(p);
                }
                PessoaJuridica pessoaJuridica = results.Where(x => x.IdAdministrador == adm.Id && x.Id == id).ToList()[0];
                return pessoaJuridica;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        // Busca uma empresa especifica por seu cnpj
        public static async Task<PessoaJuridica> PegarPjPorCnpj(string cnpj)
        {
            try
            {
                PessoaJuridica pj = new PessoaJuridica();

                pj = client.CreateDocumentQuery<PessoaJuridica>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                        .Where(x => x.Cnpj == cnpj)
                        .AsEnumerable()
                        .FirstOrDefault();

                return pj;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        // Busca uma empresa especifica por seu cnpj
        public static async Task<PessoaJuridica> PegarPjPorCnpj2(Administrador adm, string cnpj)
        {
            try
            {
                PessoaJuridica pj = new PessoaJuridica();

                List<PessoaJuridica> results = new List<PessoaJuridica>();
                foreach (PessoaJuridica p in adm.PessoaJuridica)
                {
                    results.Add(p);
                }
                PessoaJuridica pessoaJuridica = results.Where(x => x.Cnpj == cnpj).ToList()[0];
                return pessoaJuridica;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        //Adiciona Empresa no Administrador
        public static async Task<Document> CadastrarPj(Administrador adm, PessoaJuridica pj)
        {
            try
            {
                if (adm.PessoaJuridica == null)
                {
                    adm.PessoaJuridica = new List<PessoaJuridica>();
                }
                if (adm.Id == pj.IdAdministrador)
                {
                    adm.PessoaJuridica.Add(pj);
                }

                return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, adm.Id), adm);
            }
            catch (Exception e)
            {
                return null;
            }

        }

        //Deleta a PessoaJuridica dentro de adm
        public static async Task<Document> DeletarPj(PessoaJuridica pj, Administrador adm)
        {
            try
            {
                ICollection<PessoaJuridica> pjs = new List<PessoaJuridica>(adm.PessoaJuridica);

                var item = pjs.SingleOrDefault(x => x.Id == pj.Id);
                if (item != null)
                {
                    pjs.Remove(item);
                    adm.PessoaJuridica = pjs;
                }

                return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, adm.Id), adm);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //Atualiza a PessoaJuridica dentro de adm
        public static async Task<Document> AtualizarPj(PessoaJuridica pj, Administrador adm)
        {
            try
            {
                foreach (PessoaJuridica p in adm.PessoaJuridica)
                {
                    if (p.Id == pj.Id)
                    {
                        p.Id = pj.Id;
                        p.NomeFantasia = pj.NomeFantasia;
                        p.Cnpj = pj.Cnpj;
                        p.Categoria = pj.Categoria;
                        p.Endereco = pj.Endereco;
                        p.HorarioInicial = pj.HorarioInicial;
                        p.HorarioFinal = pj.HorarioFinal;
                        p.IdAdministrador = pj.IdAdministrador;
                        p.TiposJuridico = pj.TiposJuridico;
                        break;
                    }
                }
                return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, adm.Id), adm);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        // -------------------------------------------- CATEGORIA -------------------------------------
        public static async Task<IEnumerable<Categoria>> ListarCategorias(string idAdm, string idPj)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, idAdm));
                Administrador adm = (Administrador)(dynamic)document;

                List<Categoria> results = new List<Categoria>();
                foreach (PessoaJuridica p in adm.PessoaJuridica)
                {
                    foreach (Categoria c in p.Categoria)
                    {
                        results.Add(c);
                    }
                }

                return results.Where(x => x.IdPessoaJuridica == idPj).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<Categoria> PegarCategoria(string idAdm, string id)
        {
            try
            {
                Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);
                List<Categoria> results = new List<Categoria>();

                foreach (PessoaJuridica pj in adm.PessoaJuridica)
                {
                    foreach (Categoria c in pj.Categoria)
                    {
                        results.Add(c);
                    }
                }
                Categoria categoria = results.Where(x => x.Id == id).ToList()[0];
                return categoria;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<Categoria> PegarCategoriaPorNome(string nome)
        {
            try
            {
                Categoria cat = new Categoria();

                cat = client.CreateDocumentQuery<Categoria>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                        .Where(x => x.Nome == nome)
                        .AsEnumerable()
                        .FirstOrDefault();

                return cat;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public static async Task<Categoria> PegarCategoriaPorNome2(Administrador adm, string nome)
        {
            try
            {
                List<Categoria> results = new List<Categoria>();

                foreach (PessoaJuridica pj in adm.PessoaJuridica)
                {
                    foreach (Categoria c in pj.Categoria)
                    {
                        results.Add(c);
                    }
                }
                Categoria categoria = results.Where(x => x.Nome == nome).ToList()[0];
                return categoria;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //Atualiza o adm a partir do Cadastro da categoria
        public static async Task<Document> CadastrarCategoria(Administrador adm, Categoria cat)
        {
            try
            {
                // recebe uma lista de pjs
                foreach (PessoaJuridica p in adm.PessoaJuridica)
                {
                    // verificar lista de categorias
                    if (p.Categoria == null)
                    {
                        p.Categoria = new List<Categoria>();
                    }
                    if (p.Id == cat.IdPessoaJuridica)
                    {
                        p.Categoria.Add(cat);
                    }
                }
                return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, adm.Id), adm);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //Atualiza uma categoria
        public static async Task<Document> AtualizarCategoria(Administrador adm, Categoria cat)
        {
            try
            {
                foreach (PessoaJuridica pj in adm.PessoaJuridica)
                {
                    foreach (Categoria c in pj.Categoria)
                    {
                        if (c.Id == cat.Id)
                        {
                            c.Id = cat.Id;
                            c.Nome = cat.Nome;
                            c.Descricao = cat.Descricao;
                            c.Servico = cat.Servico;
                            c.IdPessoaJuridica = cat.IdPessoaJuridica;
                            break;
                        }
                    }
                }
                return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, adm.Id), adm);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //Deleta a PessoaJuridica dentro de adm
        public static async Task<Document> DeletarCategoria(Categoria cat, Administrador adm)
        {
            try
            {
                foreach (PessoaJuridica pj in adm.PessoaJuridica)
                {
                    foreach (Categoria c in pj.Categoria)
                    {
                        if (c.Id == cat.Id)
                        {
                            ICollection<Categoria> cats = new List<Categoria>(pj.Categoria);

                            var item = cats.SingleOrDefault(x => x.Id == cat.Id);
                            if (item != null)
                            {
                                cats.Remove(item);
                                pj.Categoria = cats;
                            }
                        }
                    }
                }
                return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, adm.Id), adm);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        // -------------------------------------------- SERVIÇO -------------------------------------
        //Lista todos os serviços de uma determinada categoria
        public static async Task<IEnumerable<Servico>> ListarServicos(string idAdm, string idCat)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, idAdm));
                Administrador adm = (Administrador)(dynamic)document;

                List<Servico> results = new List<Servico>();
                foreach (PessoaJuridica p in adm.PessoaJuridica)
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
            catch (Exception e)
            {
                return null;
            }
        }

        //Lista todos os serviços cadastrados no sistema
        public static async Task<IEnumerable<Servico>> ListarTodosServicos(string idAdm)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, idAdm));
                Administrador adm = (Administrador)(dynamic)document;

                List<Servico> results = new List<Servico>();
                foreach (PessoaJuridica p in adm.PessoaJuridica)
                {
                    foreach (Categoria c in p.Categoria)
                    {
                        foreach (Servico s in c.Servico)
                        {
                            results.Add(s);
                        }
                    }
                }

                return results.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<Servico> PegarServicoPorNome(string nome)
        {
            try
            {
                Servico servico = new Servico();

                servico = client.CreateDocumentQuery<Servico>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                        .Where(x => x.Nome == nome)
                        .AsEnumerable()
                        .FirstOrDefault();

                return servico;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<Servico> PegarServicoPorNome2(Administrador adm, string nome)
        {
            try
            {
                List<Servico> results = new List<Servico>();
                foreach (PessoaJuridica pj in adm.PessoaJuridica)
                {
                    foreach (Categoria c in pj.Categoria)
                    {
                        foreach (Servico s in c.Servico)
                        {
                            results.Add(s);
                        }
                    }
                }
                Servico servico = results.Where(x => x.Nome == nome).ToList()[0];
                return servico;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<Servico> PegarServico(string idAdm, string id)
        {
            try
            {
                Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);
                List<Servico> results = new List<Servico>();
                foreach (PessoaJuridica pj in adm.PessoaJuridica)
                {
                    foreach (Categoria c in pj.Categoria)
                    {
                        foreach (Servico s in c.Servico)
                        {
                            results.Add(s);
                        }
                    }
                }
                Servico servico = results.Where(x => x.Id == id).ToList()[0];
                return servico;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //Atualiza a categoria a partir do Cadastro de Serviço
        public static async Task<Document> AtualizarServico(Administrador adm, Servico serv)
        {
            try
            {
                foreach (PessoaJuridica pj in adm.PessoaJuridica)
                {
                    foreach (Categoria c in pj.Categoria)
                    {
                        foreach (Servico s in c.Servico)
                        {
                            if (s.Id == serv.Id)
                            {
                                s.Id = serv.Id;
                                s.Nome = serv.Nome;
                                s.Descricao = serv.Descricao;
                                s.Profissional = serv.Profissional;
                                s.IdCategoria = serv.IdCategoria;
                                break;
                            }
                        }
                    }
                }
                return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, adm.Id), adm);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //Atualiza o adm a partir do Cadastro de um servico
        public static async Task<Document> CadastrarServico(Administrador adm, Servico serv)
        {
            try
            {
                // recebe uma lista de pjs
                foreach (PessoaJuridica p in adm.PessoaJuridica)
                {
                    foreach (Categoria c in p.Categoria)
                    {
                        // verificar lista de categorias
                        if (c.Servico == null)
                        {
                            c.Servico = new List<Servico>();
                        }
                        if (c.Id == serv.IdCategoria)
                        {
                            c.Servico.Add(serv);
                        }
                    }
                }
                return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, adm.Id), adm);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<Document> DeletarServico(Servico serv, Administrador adm)
        {
            try
            {
                foreach (PessoaJuridica pj in adm.PessoaJuridica)
                {
                    foreach (Categoria c in pj.Categoria)
                    {
                        foreach (Servico s in c.Servico)
                        {
                            if (s.Id == serv.Id)
                            {
                                ICollection<Servico> servs = new List<Servico>(c.Servico);

                                var item = servs.SingleOrDefault(x => x.Id == serv.Id);
                                if (item != null)
                                {
                                    servs.Remove(item);
                                    c.Servico = servs;
                                }
                            }
                        }
                    }
                }
                return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, adm.Id), adm);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        // -------------------------------------------- PROFISSIONAL -------------------------------------
        public static async Task<IEnumerable<Profissional>> ListarProfissionais(string idAdm, string idServ)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, idAdm));
                Administrador adm = (Administrador)(dynamic)document;

                List<Profissional> results = new List<Profissional>();
                foreach (PessoaJuridica p in adm.PessoaJuridica)
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

        public static async Task<Profissional> PegarProfissionalPorEmail(string email)
        {
            try
            {
                Profissional prof = new Profissional();

                prof = client.CreateDocumentQuery<Profissional>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                        .Where(x => x.Email == email)
                        .AsEnumerable()
                        .FirstOrDefault();

                return prof;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<Profissional> PegarProfissionalPorEmail2(Administrador adm, string email)
        {
            try
            {
                List<Profissional> results = new List<Profissional>();
                foreach (PessoaJuridica pj in adm.PessoaJuridica)
                {
                    foreach (Categoria c in pj.Categoria)
                    {
                        foreach (Servico s in c.Servico)
                        {
                            foreach (Profissional p in s.Profissional)
                            {
                                results.Add(p);
                            }
                        }
                    }
                }
                Profissional profissional = results.Where(x => x.Email == email).ToList()[0];
                return profissional;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<Profissional> PegarProfissional(string idAdm, string id)
        {
            try
            {
                Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);

                List<Profissional> results = new List<Profissional>();
                foreach (PessoaJuridica pj in adm.PessoaJuridica)
                {
                    foreach (Categoria c in pj.Categoria)
                    {
                        foreach (Servico s in c.Servico)
                        {
                            foreach (Profissional p in s.Profissional)
                            {
                                results.Add(p);
                            }
                        }
                    }
                }
                Profissional profissional = results.Where(x => x.Id == id).ToList()[0];
                return profissional;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //Atualiza o adm a partir do Cadastro de um servico
        public static async Task<Document> CadastrarProfissional(Administrador adm, Profissional prof)
        {
            try
            {
                // recebe uma lista de pjs
                foreach (PessoaJuridica p in adm.PessoaJuridica)
                {
                    foreach (Categoria c in p.Categoria)
                    {
                        foreach (Servico s in c.Servico)
                        {
                            // verificar lista de categorias
                            if (s.Profissional == null)
                            {
                                s.Profissional = new List<Profissional>();
                            }
                            if (s.Id == prof.IdServico)
                            {
                                s.Profissional.Add(prof);
                            }
                        }
                    }
                }
                return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, adm.Id), adm);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //Atualiza a categoria a partir do Cadastro de Serviço
        public static async Task<Document> AtualizarProfissional(Administrador adm, Profissional profissional)
        {
            try
            {
                foreach (PessoaJuridica pj in adm.PessoaJuridica)
                {
                    foreach (Categoria c in pj.Categoria)
                    {
                        foreach (Servico s in c.Servico)
                        {
                            foreach (Profissional p in s.Profissional)
                            {
                                if (p.Id == profissional.Id)
                                {
                                    p.Id = profissional.Id;
                                    p.NomeCompleto = profissional.NomeCompleto;
                                    p.Email = profissional.Email;
                                    p.Pin = profissional.Pin;
                                    p.Agendamento = profissional.Agendamento;
                                    p.IdServico = profissional.IdServico;
                                    break;
                                }
                            }
                        }
                    }
                }
                return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, adm.Id), adm);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<Document> DeletarProfissional(Profissional prof, Administrador adm)
        {
            try
            {
                foreach (PessoaJuridica pj in adm.PessoaJuridica)
                {
                    foreach (Categoria c in pj.Categoria)
                    {
                        foreach (Servico s in c.Servico)
                        {
                            foreach (Profissional p in s.Profissional)
                            {
                                if (p.Id == prof.Id)
                                {
                                    ICollection<Profissional> profs = new List<Profissional>(s.Profissional);

                                    var item = profs.SingleOrDefault(x => x.Id == prof.Id);
                                    if (item != null)
                                    {
                                        profs.Remove(item);
                                        s.Profissional = profs;
                                    }
                                }
                            }
                        }
                    }
                }
                return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, adm.Id), adm);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        // -------------------------------------------- AGENDAMENTO -------------------------------------
        public static async Task<IEnumerable<Agendamento>> ListarAgendamentos(string idAdm, string idProf)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, idAdm));
                Administrador adm = (Administrador)(dynamic)document;

                List<Agendamento> results = new List<Agendamento>();
                foreach (PessoaJuridica p in adm.PessoaJuridica)
                {
                    foreach (Categoria c in p.Categoria)
                    {
                        foreach (Servico s in c.Servico)
                        {
                            foreach (Profissional pro in s.Profissional)
                            {
                                foreach (Agendamento a in pro.Agendamento)
                                {
                                    results.Add(a);
                                }
                            }
                        }
                    }
                }

                return results.Where(x => x.IdProfissional == idProf).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<Agendamento> PegarAgendamentoPeloHorario(DateTime horario)
        {
            try
            {
                Agendamento agend = new Agendamento();

                agend = client.CreateDocumentQuery<Agendamento>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                        .Where(x => x.Horario == horario)
                        .AsEnumerable()
                        .FirstOrDefault();

                return agend;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<Agendamento> PegarAgendamentoPeloHorario2(Administrador adm, DateTime horario)
        {
            try
            {
                List<Agendamento> results = new List<Agendamento>();
                foreach (PessoaJuridica pj in adm.PessoaJuridica)
                {
                    foreach (Categoria c in pj.Categoria)
                    {
                        foreach (Servico s in c.Servico)
                        {
                            foreach (Profissional p in s.Profissional)
                            {
                                foreach (Agendamento a in p.Agendamento)
                                {
                                    results.Add(a);
                                }
                            }
                        }
                    }
                }
                Agendamento agendamento = results.Where(x => x.Horario == horario).ToList()[0];
                return agendamento;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<Agendamento> PegarAgendamento(string idAdm, string id)
        {
            try
            {
                Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);
                List<Agendamento> results = new List<Agendamento>();
                foreach (PessoaJuridica pj in adm.PessoaJuridica)
                {
                    foreach (Categoria c in pj.Categoria)
                    {
                        foreach (Servico s in c.Servico)
                        {
                            foreach (Profissional p in s.Profissional)
                            {
                                foreach (Agendamento a in p.Agendamento)
                                {
                                    results.Add(a);
                                }
                            }
                        }
                    }
                }
                Agendamento agendamento = results.Where(x => x.Id == id).ToList()[0];
                return agendamento;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //Atualiza o adm a partir do Cadastro de um servico
        public static async Task<Document> CadastrarAgendamento(Administrador adm, Agendamento agend)
        {
            try
            {
                // recebe uma lista de pjs
                foreach (PessoaJuridica p in adm.PessoaJuridica)
                {
                    foreach (Categoria c in p.Categoria)
                    {
                        foreach (Servico s in c.Servico)
                        {
                            foreach (Profissional pro in s.Profissional)
                            {
                                // verificar lista de categorias
                                if (pro.Agendamento == null)
                                {
                                    pro.Agendamento = new List<Agendamento>();
                                }
                                if (pro.Id == agend.IdProfissional)
                                {
                                    pro.Agendamento.Add(agend);
                                }
                            }
                        }
                    }
                }
                return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, adm.Id), adm);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<Document> AtualizarAgendamento(Administrador adm, Agendamento agendamento)
        {
            try
            {
                foreach (PessoaJuridica pj in adm.PessoaJuridica)
                {
                    foreach (Categoria c in pj.Categoria)
                    {
                        foreach (Servico s in c.Servico)
                        {
                            foreach (Profissional p in s.Profissional)
                            {
                                foreach (Agendamento a in p.Agendamento)
                                {
                                    if (a.Id == agendamento.Id)
                                    {
                                        a.Id = agendamento.Id;
                                        a.Dia = agendamento.Dia;
                                        a.Horario = agendamento.Horario;
                                        a.Cliente = agendamento.Cliente;
                                        a.IdProfissional = agendamento.IdProfissional;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, adm.Id), adm);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<Document> DeletarAgendamento(Agendamento agend, Administrador adm)
        {
            try
            {
                foreach (PessoaJuridica pj in adm.PessoaJuridica)
                {
                    foreach (Categoria c in pj.Categoria)
                    {
                        foreach (Servico s in c.Servico)
                        {
                            foreach (Profissional p in s.Profissional)
                            {
                                foreach (Agendamento a in p.Agendamento)
                                {
                                    if (a.Id == agend.Id)
                                    {
                                        ICollection<Agendamento> agends = new List<Agendamento>(p.Agendamento);

                                        var item = agends.SingleOrDefault(x => x.Id == agend.Id);
                                        if (item != null)
                                        {
                                            agends.Remove(item);
                                            p.Agendamento = agends;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, adm.Id), adm);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        // -------------------------------------------- CLIENTE -------------------------------------
        public static async Task<Document> CadastrarCliente(T t)
        {
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), t);
        }

        public static async Task<Clientes> PegarCliente(string cpf)
        {

            Clientes cliente = new Clientes();

            cliente = client.CreateDocumentQuery<Clientes>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                    new FeedOptions
                    {
                        MaxItemCount = -1,
                        EnableCrossPartitionQuery = true
                    })
                    .Where(x => x.CpfCliente == cpf)
                    .AsEnumerable()
                    .FirstOrDefault();

            return cliente;
        }

        public static async Task<IEnumerable<Clientes>> ListarClientes()
        {

            IDocumentQuery<Clientes> query = client.CreateDocumentQuery<Clientes>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                    new FeedOptions { MaxItemCount = -1 })
                    .AsDocumentQuery();

            List<Clientes> results = new List<Clientes>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<Clientes>());
            }

            return results;
        }
        // -------------------------------------------- TOKEN -----------------------------------------
        public static async Task<Administrador> GetEmailAsync(string email, string senha)
        {
            try
            {
                IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .AsDocumentQuery();

                List<T> results = new List<T>();
                while (query.HasMoreResults)
                {
                    results.AddRange(await query.ExecuteNextAsync<T>());
                }

                return (Administrador)(dynamic)results;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<Administrador> GetSenhaAsync(string senha)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, senha));
                return (Administrador)(dynamic)document;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
