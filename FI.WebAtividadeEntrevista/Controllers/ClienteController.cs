using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebAtividadeEntrevista.Helpers;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (this.ModelState.IsValid)
            {
                if (!ValidacaoHelper.IsCpf(model.CPF))
                {
                    this.ModelState.AddModelError("CPF", "Digite um CPF válido");
                }
                else if (bo.VerificarExistencia(model.CPF))
                {
                    this.ModelState.AddModelError("CPF", "CPF já cadastrado");
                }
                else if (model.Beneficiarios != null)
                {
                    foreach (BeneficiarioModel beneficiario in model.Beneficiarios)
                    {
                        if (!ValidacaoHelper.IsCpf(beneficiario.CPF))
                        {
                            this.ModelState.AddModelError("Beneficiario[CPF]", string.Format("CPF do beneficiário inválido: {0} - {1}",
                                beneficiario.CPF,
                                beneficiario.Nome));
                            break;
                        }
                    }

                    if (model.Beneficiarios.GroupBy(b => b.CPF).Any(b => b.Count() > 1))
                        this.ModelState.AddModelError("Beneficiario[CPF]", "CPF do beneficiário em duplicidade");
                }
            }

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                List<Beneficiario> beneficiarios = new List<Beneficiario>();
                if (model.Beneficiarios != null)
                {
                    beneficiarios = model.Beneficiarios.Select(item => new Beneficiario()
                    {
                        Nome = item.Nome,
                        CPF = item.CPF
                    }).ToList();
                }

                model.Id = bo.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    CPF = model.CPF,
                    Telefone = model.Telefone,
                    Beneficiarios = beneficiarios
                });


                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (this.ModelState.IsValid)
            {
                if (!ValidacaoHelper.IsCpf(model.CPF))
                {
                    this.ModelState.AddModelError("CPF", "Digite um CPF válido");
                }
                else if (bo.VerificarExistencia(model.CPF, model.Id))
                {
                    this.ModelState.AddModelError("CPF", "CPF já cadastrado");
                }
                else if (model.Beneficiarios != null)
                {
                    foreach (BeneficiarioModel beneficiario in model.Beneficiarios)
                    {
                        if (!ValidacaoHelper.IsCpf(beneficiario.CPF))
                        {
                            this.ModelState.AddModelError("Beneficiario[CPF]", string.Format("CPF do beneficiário inválido: {0} - {1}",
                                beneficiario.CPF,
                                beneficiario.Nome));
                            break;
                        }
                    }

                    if (model.Beneficiarios.GroupBy(b => b.CPF).Any(b => b.Count() > 1))
                        this.ModelState.AddModelError("Beneficiario[CPF]", "CPF do beneficiário em duplicidade");
                }
            }

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                List<Beneficiario> beneficiarios = new List<Beneficiario>();
                if (model.Beneficiarios != null)
                {
                    beneficiarios = model.Beneficiarios.Select(item => new Beneficiario()
                    {
                        Nome = item.Nome,
                        CPF = item.CPF,
                        IdCliente = model.Id,
                        Id = item.Id
                    }).ToList();
                }

                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    CPF = model.CPF,
                    Telefone = model.Telefone,
                    Beneficiarios = beneficiarios
                });
                               
                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    CPF = cliente.CPF,
                    Telefone = cliente.Telefone,
                    Beneficiarios = cliente.Beneficiarios.Select(item => new BeneficiarioModel()
                    {
                        Id = item.Id,
                        Nome = item.Nome,
                        CPF = item.CPF,
                        IdCliente = item.IdCliente
                    }).ToList()
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}