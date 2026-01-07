using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace idk.src.frm
{
    public partial class Password : Form
    {
        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        public Password()
        {
            InitializeComponent();

            this.TopMost = true;

            SendMessage(textBox1.Handle, EM_SETCUEBANNER, 0, "Password");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckLogin();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                CheckLogin();
            }
        }

        private void CheckLogin()
        {
            if (textBox1.Text == Properties.Settings.Default.Password)
            {
                Edit edit = new Edit();
                edit.Show();
                this.Close();
            }
            else
            {
                this.Close();
            }
        }
    }
}
