using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Athenas.Service.Interfaces;
using Microsoft.Azure.Documents;
using Athenas.Domain;
using Athenas.Repository;

namespace Athenas.Service
{
    public class AdministradorService : IAdministradorService
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }


        //Listagem de adm
        public async Task<IEnumerable<Administrador>> ListarAdministradores()
        {
            List<Administrador> administradores = (List<Administrador>)await Repository<Administrador>.ListarAdm();
            
            if (administradores == null)
            {
                throw new ArgumentException("O ID informado está incorreto ou ele não existe.", "adm");
            }
            else
            {
                return administradores;
            }
        }

        //Cadastrar um adm
        public async Task<Administrador> CadastrarAdm(Administrador adm)
        {
            adm.PessoaJuridica = new List<PessoaJuridica>();
            adm.HashearSenha();
            await Repository<Administrador>.CadastrarItem(adm);
            adm = await Repository<Administrador>.PegarAdmPorEmail(adm.Email);

            if (adm == null)
            {
                throw new ArgumentException("Ocorreu um erro no cadastro. Por favor, verifique suas informações.", "adm");
            }
            else
            {
                return adm;
            }
        }

        //Pegar um único adm
        public async Task<Administrador> PegarAdm(string id)
        {
            Administrador adm = await Repository<Administrador>.PegarAdm(id);

            if (adm == null)
            {
                throw new ArgumentException("O ID informado está incorreto ou ele não existe.", "adm");
            }
            else
            {
                return adm;
            }
        }


        //Atualizar um adm
        public async Task<Document> AtualizarAdm(string id, Administrador adm)
        {
            Administrador admin = await Repository<Administrador>.PegarAdm(id);

            if (adm.NomeCompleto == null)
            {
                adm.NomeCompleto = admin.NomeCompleto;
            }

            if (adm.Senha == null)
            {
                adm.Senha = admin.Senha;
            }

            if (adm.Email == null)
            {
                adm.Email = admin.Email;
            }

            if (adm.PessoaJuridica == null)
            {
                adm.PessoaJuridica = admin.PessoaJuridica;
            }

            adm.Id = id;

            return await Repository<Administrador>.AtualizarAdm(id, adm);
        }


        //Deletar um adm
        public async Task DeletarAdm(string id)
        {
            try
            {
                await Repository<Administrador>.DeletarItem(id);
            }
            catch (ArgumentException e)
            {
                throw null;
            }

        }

    }
}
