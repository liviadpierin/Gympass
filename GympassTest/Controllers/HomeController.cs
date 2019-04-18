using GympassTest.Models;
using GympassTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace GympassTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Submit(HttpPostedFileBase file)
        {
            CultureInfo cultura = new CultureInfo("pt-BR");
            if (file != null && file.ContentLength > 0)
            {
                List<VoltaCorrida> corrida = new List<VoltaCorrida>();
                DashboardClassificacaoViewModel dashboard = new DashboardClassificacaoViewModel();
                VoltaCorrida volta;

                try
                {
                    BinaryReader reader = new BinaryReader(file.InputStream);
                    byte[] data = reader.ReadBytes(file.ContentLength);

                    string result = System.Text.Encoding.UTF8.GetString(data);
                    string[] linhas = result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 1; i < linhas.Length; i++)
                    {
                        volta = new VoltaCorrida();
                        string linha = linhas[i].Replace("–", "").Trim();
                        string[] colunas = linha.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                        if (new Regex(@"([0-9]{2}:[0-9]{2}:[0-9]{2}.[0-9]{3})", RegexOptions.IgnoreCase).IsMatch(colunas[0]))
                        {
                            volta.Hora = TimeSpan.Parse(colunas[0], cultura);
                        }
                        if (new Regex(@"([0-9]{3})", RegexOptions.IgnoreCase).IsMatch(colunas[1]))
                        {
                            volta.CodigoPiloto = colunas[1];
                        }
                        if (new Regex(@"([A-Za-z])+", RegexOptions.IgnoreCase).IsMatch(colunas[2]))
                        {
                            volta.Piloto = colunas[2];
                        }
                        if (new Regex(@"([0-9])", RegexOptions.IgnoreCase).IsMatch(colunas[3]))
                        {
                            volta.Volta = int.Parse(colunas[3]);
                        }
                        if (new Regex(@"([0-9]{1,2}:[0-9]{2}.[0-9]{3})", RegexOptions.IgnoreCase).IsMatch(colunas[4]))
                        {
                            string tempoVolta = colunas[4];
                            if (tempoVolta.Split(':').Length < 3)
                            {
                                tempoVolta = "00:" + tempoVolta;
                            }
                            volta.TempoVolta = TimeSpan.Parse(tempoVolta, cultura);
                        }
                        if (new Regex(@"([0-9]{2},[0-9]{3})", RegexOptions.IgnoreCase).IsMatch(colunas[5]))
                        {
                            volta.VelocidadeMedia = float.Parse(colunas[5], cultura.NumberFormat);
                        }

                        corrida.Add(volta);
                    }

                    dashboard.Classificacao = corrida
                        .GroupBy(c => c.CodigoPiloto)
                        .Select(y => new ResultadosPiloto()
                        {
                            HoraChegada = y.ToList().Max(i => i.Hora),
                            CodigoPiloto = y.Key,
                            Piloto = y.ToList().Where(i => i.CodigoPiloto == y.Key).Select(i => i.Piloto).First(),
                            Voltas = y.ToList().Max(i => i.Volta),
                            MelhorVolta = y.ToList().Min(i => i.TempoVolta),
                            TempoTotal = new TimeSpan(y.ToList().Sum(i => i.TempoVolta.Ticks)),
                            VelocidadeMedia = (y.ToList().Sum(i => i.VelocidadeMedia) / y.ToList().Max(i => i.Volta))
                        })
                        .OrderBy(x => x.HoraChegada)
                        .ThenByDescending(n => n.Voltas)
                        .ToList();
                    return PartialView("_DashboardClassificacao", dashboard);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }

            }
            return PartialView();
        }
    }
}