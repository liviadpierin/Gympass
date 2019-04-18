using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GympassTest.Models
{
    public class VoltaCorrida
    {
        public TimeSpan Hora { get; set; }
        public String CodigoPiloto { get; set; }
        public String Piloto { get; set; }
        public int Volta { get; set; }
        public TimeSpan TempoVolta { get; set; }
        public float VelocidadeMedia { get; set; }
    }
}