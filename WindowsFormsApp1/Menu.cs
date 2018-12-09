using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            using (var db = new Model.AppContext())
            {
                db.HotelRooms.Load();
                dataGridView1.DataSource = db.HotelRooms.Local.ToList();
            } 
        }
    }
}
