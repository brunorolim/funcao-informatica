using System.Collections.Generic;
using System.Linq;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();

            /**
             * TODO: Incluir controle transacional
             */

            // Incluir cliente
            cliente.Id = cli.Incluir(cliente);

            // Incluir beneficiários
            if (cliente.Beneficiarios != null)
            {
                foreach (DML.Beneficiario beneficiario in cliente.Beneficiarios)
                {
                    beneficiario.IdCliente = cliente.Id;
                    beneficiario.Id = ben.Incluir(beneficiario);
                }
            }

            return cliente.Id;
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();

            /**
             * TODO: Incluir controle transacional
             */

            // Alterar cliente
            cli.Alterar(cliente);

            // Excluir beneficiários existentes que não estão na lista enviada 
            List<DML.Beneficiario> bdBeneficiarios = ben.Listar(cliente.Id);
            if (bdBeneficiarios != null)
            {
                if (cliente.Beneficiarios != null)
                    bdBeneficiarios.RemoveAll(b => cliente.Beneficiarios.Select(bc => bc.Id).ToArray().Contains(b.Id));

                foreach (DML.Beneficiario beneficiario in bdBeneficiarios)
                    ben.Excluir(beneficiario.Id);
            }

            // Incluir/Alterar beneficiários
            foreach (DML.Beneficiario beneficiario in cliente.Beneficiarios)
            {
                if (beneficiario.Id == 0)
                    beneficiario.Id = ben.Incluir(beneficiario);
                else
                    ben.Alterar(beneficiario);
            }
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();

            DML.Cliente cliente = cli.Consultar(id);
            cliente.Beneficiarios = ben.Listar(cliente.Id);

            return cliente;
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm,  quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// Verifica a existência de um CPF
        /// </summary>
        /// <param name="CPF">CPF para validação</param>
        /// <param name="Id">Id do cliente para não considerar o mesmo CPF</param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF, long Id = 0)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.VerificarExistencia(CPF, Id);
        }
    }
}
