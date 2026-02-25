using idk.src;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using idk.src.Class;
using System.Linq;
using System.Collections.Generic;
using idk.src.frm;

namespace idk
{
    public partial class Main : Form
    {
        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        Ordre ordre;

        bool isDelivery = false;

        List<string> toppings = new List<string>();

        List<Pizza> pizzaList = new List<Pizza>();

        JsonElement pizzas;

        double totalPrice = 0;

        Util util = new Util();

        enum FLAG
        {
            ADD,
            SUBTRACT
        }


        public Main()
        {
            InitializeComponent();

            SendMessage(textBox1.Handle, EM_SETCUEBANNER, 0, "Name");
            SendMessage(textBox2.Handle, EM_SETCUEBANNER, 0, "Adress");
            SendMessage(textBox3.Handle, EM_SETCUEBANNER, 0, "Phone nr");
            SendMessage(textBox4.Handle, EM_SETCUEBANNER, 0, "Amount");

            this.TopMost = true;

            try
            {
                if (Properties.Settings.Default.JsonPath != null)
                {
        using (StreamReader r = new StreamReader(Properties.Settings.Default.JsonPath))
        {
            string json = r.ReadToEnd();

            var jsonObject = JsonDocument.Parse(json);

            pizzas = jsonObject.RootElement.GetProperty("pizzas");

            if (!util.IsJsonValid(pizzas))
            {
                return;
            }

            for (int i = 0; i < pizzas.GetArrayLength(); i++)
            {
                comboBox1.Items.Add($"{pizzas[i].GetProperty("name")} {pizzas[i].GetProperty("price")}Kr");
            }
        }
    }
} catch {
    MessageBox.Show("Link Json data in settings");
}
            
        }

        protected override void WndProc(ref Message m)
        {
            switch(m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }

            base.WndProc(ref m);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(checkedListBox2.Items.Count == 0)
            {
                return;
            }

            int amount;

            if(!Int32.TryParse(textBox3.Text, out amount))
            {
                return;
            }

            if (!isDelivery)
            {
                if(textBox1.Text != "" && comboBox1.SelectedIndex != -1)
                {
                    ordre = new Ordre(textBox1.Text, textBox2.Text, amount, isDelivery);
                }
            }
            else
            {
                if(textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && comboBox1.SelectedIndex != -1)
                {
                    ordre = new Ordre(textBox1.Text, textBox2.Text, Int32.Parse(textBox3.Text), isDelivery);

                    Console.WriteLine($"{ordre.GetName()}, {ordre.GetAdress()}, {ordre.GetPhoneNumber()}, {pizzas[comboBox1.SelectedIndex].GetProperty("name")}");
                    Recipe recipe = new Recipe(ordre, pizzaList, totalPrice);
                    recipe.ShowDialog();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == -1)
            {
                return;
            }

            foreach(Control checkBox in Controls)
            {
                if(checkBox is CheckBox && checkBox.Name != "checkBox1")
                {
                    ((CheckBox)checkBox).Checked = false;
                }
            }

            var ingredients = pizzas[comboBox1.SelectedIndex].GetProperty("ingredients");

            for(int i = 0; i < ingredients.GetArrayLength(); i++)
            {
                for(int j = 0; j < ingredients[i].GetArrayLength(); j++)
                {
                    switch(ingredients[i][j].ToString())
                    {
                        case "Tomat":
                            checkBoxTomat.CheckState = CheckState.Checked;
                            break;
                        case "Ost":
                            checkBoxOst.CheckState = CheckState.Checked;
                            break;
                        case "Skinke":
                            checkBoxSkinke.CheckState = CheckState.Checked;
                            break;
                        case "Champingions":
                            checkBoxChampingions.CheckState = CheckState.Checked;
                            break;
                        case "Oksekød":
                            checkBoxOksekod.CheckState = CheckState.Checked;
                            break;
                        case "Chili":
                            checkBoxChili.CheckState = CheckState.Checked;
                            break;
                        case "Salat":
                            checkBoxSalat.CheckState = CheckState.Checked;
                            break;
                        case "Rejer":
                            checkBoxRejer.CheckState = CheckState.Checked;
                            break;
                        case "Tun":
                            checkBoxTun.CheckState = CheckState.Checked;
                            break;
                        case "Løg":
                            checkBoxLog.CheckState = CheckState.Checked;
                            break;
                        case "Salami":
                            checkBoxSalami.CheckState = CheckState.Checked;
                            break;
                        default: break;
                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                isDelivery = true;
            }
        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            string ingredients = "";
            int amount;

            if(!Int32.TryParse(textBox4.Text, out amount))
            {
                return;
            }

            if (textBox4.Text == "" || comboBox1.SelectedIndex == -1 || amount == 0)
            {
                return;
            }

            foreach(Control checkBox in Controls)
            {
                if(checkBox is CheckBox && ((CheckBox)checkBox).Checked == true && checkBox.Name != "checkBox1")
                {
                    toppings.Add(((CheckBox)checkBox).Text);
                    ingredients += $"{((CheckBox)checkBox).Text}, ";
                }
            }

            Pizza pizza = new Pizza(pizzas[comboBox1.SelectedIndex].GetProperty("name").ToString(), Double.Parse(pizzas[comboBox1.SelectedIndex].GetProperty("price").ToString()), toppings, amount);
            
            checkedListBox2.Items.Add($"{amount} {pizza.GetName()} {pizza.GetPrice()}Kr, {ingredients.Remove(ingredients.Length - 2)}");
            
            CalcTotalPrice(pizza.GetPrice() * amount, FLAG.ADD);

            pizzaList.Add(pizza);
        }

        private double CalcTotalPrice(double amount, FLAG flag)
        {
            if(flag == FLAG.ADD)
            {
                totalPrice += amount;          

                label2.Text = $"Total: {totalPrice}Kr";
                return totalPrice;

            }
            else if(flag == FLAG.SUBTRACT)
            {

                totalPrice -= amount;
                label2.Text = $"Total: {totalPrice}Kr";
                return totalPrice;
            }

            return 0000;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CalcTotalPrice(pizzaList[checkedListBox2.SelectedIndex].GetPrice(), FLAG.SUBTRACT);
            
            checkedListBox2.Items.RemoveAt(checkedListBox2.SelectedIndex);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Password password = new Password();
            password.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
