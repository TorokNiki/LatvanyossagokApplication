using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatvanyossagokApplication
{
    class Latvanyossag
    {
        private int latvanyossagId;
        private string latvanyossagNev;
        private string latvanyossagLeiras;
        private int latvanyossagAr;
        private int varosId;

        public Latvanyossag(int latvanyossagId, string latvanyossagNev, string latvanyossagLeiras, int latvanyossagAr, int varosId)
        {
            this.latvanyossagId = latvanyossagId;
            this.latvanyossagNev = latvanyossagNev;
            this.latvanyossagLeiras = latvanyossagLeiras;
            this.latvanyossagAr = latvanyossagAr;
            this.varosId = varosId;
        }

        public int LatvanyossagId { get => latvanyossagId; set => latvanyossagId = value; }
        public string LatvanyossagNev { get => latvanyossagNev; set => latvanyossagNev = value; }
        public string LatvanyossagLeiras { get => latvanyossagLeiras; set => latvanyossagLeiras = value; }
        public int LatvanyossagAr { get => latvanyossagAr; set => latvanyossagAr = value; }
        public int VarosId { get => varosId; set => varosId = value; }

        public override string ToString()
        {
            if (latvanyossagAr == 0)
            {
                return latvanyossagNev + " - Ingyenes ";
            }
            else
            {
                return latvanyossagNev + " - " + latvanyossagAr + " Ft";
            }
        }
    }
}
