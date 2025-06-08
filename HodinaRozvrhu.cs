using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZlabGrade
{
    public class HodinaRozvrhu
    {
        public string Den { get; set; }
        public int Hodina { get; set; }
        public string Predmet { get; set; }
        public string Ucitel { get; set; }
        public string Mistnost { get; set; }

        public HodinaRozvrhu(string den, int hodina, string predmet, string ucitel, string mistnost)
        {
            Den = den;
            Hodina = hodina;
            Predmet = predmet;
            Ucitel = ucitel;
            Mistnost = mistnost;
        }

    }
}
