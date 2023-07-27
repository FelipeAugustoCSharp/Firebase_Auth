using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_em_Firebase
{
    public class Produtos
    {
        public string id;
        public string Nome;
        public string Desc;
        public double Preco;
        public override string ToString()
        {
            return Nome + " - " + Desc;
        }
        public Produtos(string id, string nome, string desc, double preco)
        {
            this.id = id;
            Nome = nome;
            Desc = desc;
            Preco = preco;
        }
    }
}
