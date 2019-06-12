using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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

        private void LimpaCampos()
        {
            txtModelo.Clear();
            cbCor.SelectedIndex = -1;
            mtbPreco.Clear();
            nudAno.Value = 1885;

        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Carro carro = new Carro();

            carro.Modelo = txtModelo.Text;
            if (carro.Modelo == "")
            {
                MessageBox.Show("Digite o modelo!");
                return;
            }


            carro.Ano = Convert.ToInt32(nudAno.Value);
            if (carro.Ano == 0)
            {
                MessageBox.Show("Digite o ano");
                return;
            }


            string preco = mtbPreco.Text;
            if (preco == "R$      .")
            {
                MessageBox.Show("Digite o preco");
                return;
            }
            else
            {
                preco = preco.Replace('R', ' ');
                preco = preco.Replace('$', ' ');

            }
            carro.Preco = Convert.ToDecimal(preco);


            try
            {
                carro.Cor = cbCor.SelectedItem.ToString();

            }
            catch (Exception)
            {
                MessageBox.Show("Selecione a cor");
                return;
            }



            SqlConnection conexao = new SqlConnection();
            conexao.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=T:\Documentos\GitHub\C-Sharpe---Entra21---Exemplo---Banco-de-Dados\ExemploBD\MeusCarros.mdf;Integrated Security=True;Connect Timeout=30";
            try
            {
                conexao.Open();

            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao conectar no banco de dados!");
                return;
            }

            SqlCommand comando = new SqlCommand();
            comando.Connection = conexao;
            try
            {

                comando.CommandText = @"INSERT INTO carro (modelo, ano, preco, cor) VALUES (@MODELO,@ANO,@PRECO,@COR)";
                comando.Parameters.AddWithValue("@MODELO", carro.Modelo);
                comando.Parameters.AddWithValue("@ANO", carro.Ano);
                comando.Parameters.AddWithValue("@PRECO", carro.Preco);
                comando.Parameters.AddWithValue("@COR", carro.Cor);
                comando.ExecuteNonQuery();
                MessageBox.Show("Adicionado com sucesso");


            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao adicionar");
                MessageBox.Show(erro.ToString());
                conexao.Close();
                return;
            }

            LimpaCampos();
            conexao.Close();
            AtualizarTabela();

        }

        private void cbCor_DropDown(object sender, EventArgs e)
        {
        }

        private void cbCor_Enter(object sender, EventArgs e)
        {
            if (cbCor.DroppedDown == false)
            {
                cbCor.DroppedDown = true;
            }
        }

        private void nudAno_Enter(object sender, EventArgs e)
        {

        }

        private void mtbPreco_Enter(object sender, EventArgs e)
        {
            mtbPreco.SelectAll();
        }

        private void AtualizarTabela()
        {
            dataGridView1.Rows.Clear();
            SqlConnection conexao = new SqlConnection();
            conexao.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=T:\Documentos\GitHub\C-Sharpe---Entra21---Exemplo---Banco-de-Dados\ExemploBD\MeusCarros.mdf;Integrated Security=True;Connect Timeout=30";
            conexao.Open();

            SqlCommand comando = new SqlCommand();
            comando.Connection = conexao;

            comando.CommandText = "SELECT id, modelo, cor, preco, ano from carro";
            DataTable tabela = new DataTable();
            tabela.Load(comando.ExecuteReader());

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                DataRow linha = tabela.Rows[i];
                Carro carro = new Carro();
                carro.Id = Convert.ToInt32(linha["id"]);
                carro.Modelo = linha["modelo"].ToString();
                carro.Cor = linha["cor"].ToString();
                carro.Preco = Convert.ToDecimal(linha["preco"]);
                carro.Ano = Convert.ToInt32(linha["ano"]);
                dataGridView1.Rows.Add(new string[] { carro.Id.ToString(), carro.Modelo, carro.Cor, carro.Preco.ToString(), carro.Ano.ToString() });
            }

            conexao.Close();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            AtualizarTabela();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Cadastre primeiro um carro");
                return;
            }

            DialogResult result = MessageBox.Show("Deseja apagar?", "Aviso ", MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
            if (result == DialogResult.Yes)
            {
                SqlConnection conexao = new SqlConnection();
                conexao.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=T:\Documentos\GitHub\C-Sharpe---Entra21---Exemplo---Banco-de-Dados\ExemploBD\MeusCarros.mdf;Integrated Security=True;Connect Timeout=30";
                conexao.Open();

                SqlCommand comando = new SqlCommand();
                comando.Connection = conexao;

                comando.CommandText = @"DELETE FROM carro WHERE id = @ID";
                int id = 0;
                try
                {

                    id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                }
                catch (Exception)
                {
                    MessageBox.Show("Você deve cadastrar um carro primeiro");
                    return;
                }
                comando.Parameters.AddWithValue("@ID", id);
                try
                {
                    comando.ExecuteNonQuery();
                    MessageBox.Show("Excluido com sucesso");
                    conexao.Close();
                    AtualizarTabela();
                }
                catch (Exception)
                {
                    MessageBox.Show("Erro ao apagar o registro");
                    return;

                }
            }
        }

        int idAtualizar = 0;

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnSalvar.Visible = false;
            btnAtualizar.Visible = true;

            SqlConnection conexao = new SqlConnection();
            conexao.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=T:\Documentos\GitHub\C-Sharpe---Entra21---Exemplo---Banco-de-Dados\ExemploBD\MeusCarros.mdf;Integrated Security=True;Connect Timeout=30";
            conexao.Open();

            SqlCommand comando = new SqlCommand();
            comando.Connection = conexao;

            comando.CommandText = "SELECT id, modelo, cor, preco, ano from carro WHERE id = @ID";
            comando.Parameters.AddWithValue("@ID", dataGridView1.CurrentRow.Cells[0].Value);

            DataTable tabela = new DataTable();
            tabela.Load(comando.ExecuteReader());

            conexao.Close();


            DataRow linha = tabela.Rows[0];
            Carro carro = new Carro();

            idAtualizar = Convert.ToInt32(linha["Id"]);
            carro.Modelo = linha["modelo"].ToString();
            carro.Cor = linha["cor"].ToString();
            carro.Preco = Convert.ToDecimal(linha["preco"]);
            carro.Ano = Convert.ToInt32(linha["ano"]);



            txtModelo.Text = carro.Modelo;
            cbCor.Text = carro.Cor;
            mtbPreco.Text = carro.Preco.ToString();
            nudAno.Text = carro.Ano.ToString();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Deseja alterar?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            if (result == DialogResult.Cancel)
            {
                btnAtualizar.Visible = false;
                btnSalvar.Visible = true;
                LimpaCampos();
                return;
            }
            else
            {


                SqlConnection conexao = new SqlConnection();
                conexao.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=T:\Documentos\GitHub\C-Sharpe---Entra21---Exemplo---Banco-de-Dados\ExemploBD\MeusCarros.mdf;Integrated Security=True;Connect Timeout=30";

                SqlCommand comando = new SqlCommand();
                comando.Connection = conexao;

                conexao.Open();

                comando.CommandText = "UPDATE carro SET modelo = @MODELO, cor = @COR, preco = @PRECO, ano = @ANO WHERE id = @ID";

                Carro carro = new Carro();
                carro.Id = idAtualizar;
                carro.Modelo = txtModelo.Text;
                carro.Cor = cbCor.SelectedItem.ToString();
                string preco = mtbPreco.Text;
                preco = preco.Replace('R', ' ');
                preco = preco.Replace('$', ' ');
                carro.Preco = Convert.ToDecimal(preco);
                carro.Ano = Convert.ToInt32(nudAno.Value);

                comando.Parameters.AddWithValue("@ID", carro.Id);
                comando.Parameters.AddWithValue("@MODELO", carro.Modelo);
                comando.Parameters.AddWithValue("@COR", carro.Cor);
                comando.Parameters.AddWithValue("@PRECO", carro.Preco);
                comando.Parameters.AddWithValue("@ANO", carro.Ano);

                try
                {
                    comando.ExecuteNonQuery();
                    conexao.Close();
                    btnAtualizar.Visible = false;
                    btnSalvar.Visible = true;
                    LimpaCampos();
                    AtualizarTabela();
                    MessageBox.Show("Atualizado o registro " + idAtualizar + " com sucesso");
                }
                catch (Exception erro)
                {
                    MessageBox.Show("Não foi possivel alterar");
                    MessageBox.Show(erro.ToString());
                    btnAtualizar.Visible = false;
                    btnSalvar.Visible = true;
                    LimpaCampos();
                    return;
                }
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AtualizarTabela();
        }
    }


}
