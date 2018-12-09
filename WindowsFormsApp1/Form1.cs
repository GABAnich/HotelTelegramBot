using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.Model;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            using (AppContext db = new AppContext())
            {
                string login = textBox1.Text;
                string password = textBox2.Text;

                if (password != "123123")
                {
                    MessageBox.Show("Невірний пароль", "Помилка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }

                //Operator op = db.Operators
                //    .Where(o => o.Login == login)
                //    .SingleOrDefault();

                //if (op == null)
                //{
                //    MessageBox.Show("Такого оператора не існує", "Помилка",
                //        MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //else if (op.Password != password)
                //{
                //    MessageBox.Show("Невірний пароль", "Помилка",
                //        MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                Hide();
                Menu menu = new Menu();
                menu.Show();
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Register register = new Register();
            register.Show();            
        }
    }
}
