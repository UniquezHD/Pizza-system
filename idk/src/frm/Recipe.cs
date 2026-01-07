using idk.src.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace idk.src.frm
{
    public partial class Recipe : Form
    {
        List<Pizza> pizzas;
        Ordre ordre;
        double total;

        public Recipe()
        {
            InitializeComponent();
        }

        public Recipe(Ordre ordre, List<Pizza> pizzas, double total)
        {
            InitializeComponent();

            this.ordre = ordre;
            this.pizzas = pizzas;
            this.total = total;

            InitTextBox(this.ordre.GetName(), this.ordre.GetAdress(), this.ordre.GetPhoneNumber(), this.ordre.IsDelivery(), this.pizzas);
        }

        public void InitTextBox(string name, string address, int phoneNumber, bool isDelivery, List<Pizza> pizzas)
        {
            double discountAmount = 0;
            double total = 0;

            var sb = new StringBuilder();

            sb.AppendLine($"{name}");
            sb.AppendLine($"{address}");
            sb.AppendLine($"Phone: {phoneNumber}");
            sb.AppendLine($"Delivery: {(isDelivery ? "Yes" : "No")}");
            sb.AppendLine(new string('-', 30));

            foreach (var pizza in pizzas)
            {
                double pizzaTotal = pizza.GetAmount() * pizza.GetPrice();
                total += pizzaTotal;

                sb.AppendLine($"{pizza.GetAmount()}x {pizza.GetName()} - {pizzaTotal} Kr");
                sb.AppendLine($"Ingredients: {string.Join(", ", pizza.GetIngredients())}");
                sb.AppendLine();
            }

            if (total >= 200)
            {
                discountAmount = total * 0.10;
                total -= discountAmount;
            }

            sb.AppendLine(new string('-', 30));
            sb.AppendLine($"Discount: {discountAmount} Kr");
            sb.AppendLine($"Total: {total} Kr");

            textBox1.Text = sb.ToString();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }

            base.WndProc(ref m);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string savePath;
            DateTime timeStamp = DateTime.Now;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            
            dialog.ShowDialog();

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                savePath = dialog.SelectedPath;
                File.WriteAllText(savePath + $"recipe-{timeStamp}.txt", textBox1.Text);
                
                this.Close();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
