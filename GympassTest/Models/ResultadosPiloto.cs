using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GympassTest.Models
{
    public class ResultadosPiloto
    {
        public int Posicao { get; set; }
        public TimeSpan HoraChegada { get; set; }
        public String CodigoPiloto { get; set; }
        public String Piloto { get; set; }
        public int Voltas { get; set; }
        public TimeSpan TempoTotal { get; set; }
        public TimeSpan MelhorVolta { get; set; }
        public float VelocidadeMedia { get; set; }
        public TimeSpan TempoDiferenca { get; set; }
    }
}