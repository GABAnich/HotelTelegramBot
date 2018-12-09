using System;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.Model;

namespace WindowsFormsApp1
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            string secondName = textBox1.Text;
            string firstName = textBox2.Text;
            string middleName = textBox3.Text;
            string login = textBox4.Text;
            string password = textBox5.Text;
            string repeatPassword = textBox6.Text;

            if (password != repeatPassword)
            {
                MessageBox.Show("Паролі не співпадають", "Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //using (Model.AppContext db = new Model.AppContext())
            //{
            //    Operator op = db.Operators
            //        .Where(o => o.Login == login)
            //        .SingleOrDefault();

            //    if (op != null)
            //    {
            //        MessageBox.Show("Логін вже існує", "Помилка",
            //            MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }

            //    try
            //    {
            //        Operator o = new Operator
            //        {
            //            SecondName = secondName,
            //            FirstName = firstName,
            //            MiddleName = middleName,
            //            DateOfRegister = DateTime.Now.ToShortDateString(),
            //            Login = login,
            //            Password = password
            //        };

            //        db.Operators.Add(o);
            //        db.SaveChanges();
            //    }
            //    catch (Exception exception)
            //    {
            //        MessageBox.Show(exception.Message, "Помилка",
            //                MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //}

            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
