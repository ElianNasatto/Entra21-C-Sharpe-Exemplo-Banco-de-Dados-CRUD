using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exemplo00
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Carro carro = new Carro();

            carro.Modelo = txtModelo.Text;
            carro.Ano = Convert.ToInt32(nudAno.Value);
            carro.Preco = Convert.ToDecimal(mtbPreco.Text);
            carro.Cor = cbCor.SelectedItem.ToString();
        }
    }
}
