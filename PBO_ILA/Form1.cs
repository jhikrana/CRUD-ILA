using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace PBO_ILA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class databaseHelper
        {
            string connString = "Host=localhost;Username=postgres;Password=1;Database=Univ";
            NpgsqlConnection conn;

            public void connect()
            {
                if (conn == null)
                {
                    conn = new NpgsqlConnection(connString);
                }
                conn.Open();
            }

            public DataTable getData(string sql)
            {
                DataTable table = new DataTable();
                connect();
                try
                {
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, conn);
                    adapter.Fill(table);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    conn.Close();
                }
                return table;
            }

            public void exc(String sql)
            {
                connect();
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch
                {

                }
                finally
                {
                    conn.Close();
                }
            }
        }

        databaseHelper db = new databaseHelper();


        private void Form1_Load(object sender, EventArgs e)
        {
            DataDosen();
        }

        private void DataDosen()
        {
            string sql = "select * from dosen";
            dataGridView1.DataSource = db.getData(sql);
            dataGridView1.Columns["id_dosen"].HeaderText = "ID Dosen";
            dataGridView1.Columns["nama"].HeaderText = "Nama";
            dataGridView1.Columns["alamat"].HeaderText = "Alamat";
            dataGridView1.Columns["no_telepon"].HeaderText = "No Telepon";
            dataGridView1.Columns["alamat"].HeaderText = "Alamat";
            dataGridView1.Columns["email"].HeaderText = "Email";
            dataGridView1.Columns["edit"].DisplayIndex = 6;
            dataGridView1.Columns["delete"].DisplayIndex = 6;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = $"INSERT INTO dosen(id_dosen,nama,alamat,no_telepon,email) VALUES ('{textBox1.Text}','{textBox2.Text}','{textBox3.Text}','{textBox4.Text}','{textBox5.Text}')";
            DialogResult dialogResult = MessageBox.Show("APAKAH ANDA YAKIN?", "INSERT DATA DOSEN", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                MessageBox.Show("Berhasil!");
                db.exc(sql);
                DataDosen();
                button3.PerformClick();
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("Gagal!");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Edit")
            {
                button1.Enabled = false;
                textBox1.Enabled = false;
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["id_dosen"].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["nama"].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["alamat"].Value.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["no_telepon"].Value.ToString();
                textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells["email"].Value.ToString();
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {
                var id = dataGridView1.Rows[e.RowIndex].Cells["id_dosen"].Value.ToString();
                string sql = $"delete from dosen where id_dosen = '{id}'";

                DialogResult dialogResult = MessageBox.Show("APAKAH ANDA YAKIN?", "DELETE DATA DOSEN", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    MessageBox.Show("Berhasil!");
                    db.exc(sql);
                    DataDosen();
                    button3.PerformClick();
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("Gagal!");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            button1.Enabled = true;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = $"update dosen set nama = '{textBox2.Text}', alamat = '{textBox3.Text}', no_telepon = '{textBox4.Text}', email='{textBox5.Text}' where id_dosen = '{textBox1.Text}'";

            DialogResult dialogResult = MessageBox.Show("APAKAH ANDA YAKIN?", "UPDATE DATA DOSEN", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                MessageBox.Show("Berhasil!");
                db.exc(sql);
                DataDosen();
                button3.PerformClick();
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("Gagal!");
            }
        }
    }
}
