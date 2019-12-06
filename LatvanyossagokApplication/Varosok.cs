using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatvanyossagokApplication
{
    class Varosok
    {
        private int varosId;
        private string varosNev;
        private int varosLakossag;

        public Varosok(int varosId, string varosNev, int varosLakossag)
        {
            this.varosId = varosId;
            this.varosNev = varosNev;
            this.varosLakossag = varosLakossag;
        }
        public int Id { get => varosId; set => varosId = value; }
        public string Nev { get => varosNev; set => varosNev = value; }
        public int Lakossag { get => varosLakossag; set => varosLakossag = value; }
        public override string ToString()
        {
            return varosNev+"- Lakosság: "+ varosLakossag+" fő";
        }
    }
}
