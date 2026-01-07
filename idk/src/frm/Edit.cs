using idk.src.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace idk.src.frm
{
    public partial class Edit : Form
    {
        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        List<string> newToppings = new List<string>();

        List<string> editToppings = new List<string>();

        JsonElement pizzas;

        Util util = new Util();
        public Edit()
        {
            InitializeComponent();

            SendMessage(textBox1.Handle, EM_SETCUEBANNER, 0, "Price");
            SendMessage(textBox2.Handle, EM_SETCUEBANNER, 0, "Price");
            SendMessage(textBox3.Handle, EM_SETCUEBANNER, 0, "Name");
            SendMessage(textBox4.Handle, EM_SETCUEBANNER, 0, "Name");

            if(Properties.Settings.Default.JsonPath != null)
            {
                RefreshData();
            }
        }

        private void RefreshData()
        {
            comboBox1.Items.Clear();
            using(StreamReader r = new StreamReader(Properties.Settings.Default.JsonPath))
            {
                string json = r.ReadToEnd();
                var jsonObject = JsonDocument.Parse(json);
                pizzas = jsonObject.RootElement.GetProperty("pizzas");

                if(!util.IsJsonValid(pizzas))
                {
                    return;
                }

                for(int i = 0; i < pizzas.GetArrayLength(); i++)
                {
                    comboBox1.Items.Add($"{pizzas[i].GetProperty("name")} {pizzas[i].GetProperty("price")}Kr");
                }
            }
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            newToppings.Clear();

            string[] bannedCheckBoxes = {
                "checkBoxTomat",
                "checkBoxOst",
                "checkBoxSkinke",
                "checkBoxChampingions",
                "checkBoxOksekod",
                "checkBoxChili",
                "checkBoxSalat",
                "checkBoxRejer",
                "checkBoxTun",
                "checkBoxLog",
                "checkBoxBacon",
                "checkBoxSalami"
            };

            foreach(Control checkBox in Controls)
            {
                if(checkBox is CheckBox && ((CheckBox)checkBox).Checked == true && !bannedCheckBoxes.Contains(checkBox.Name))
                {
                    newToppings.Add(((CheckBox)checkBox).Text); 
                }
            }

            try
            {
                Pizza newPizza = new Pizza(textBox3.Text, Double.Parse(textBox2.Text), newToppings);
                newPizza.SavePizza();
            
                label2.Text = $"Added: {newPizza.GetName()}";

                RefreshData();
            }
            catch(Exception ex)
            {
                label2.Text = $"Failed: {ex.Message}";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            editToppings.Clear();

            if(comboBox1.SelectedIndex == -1)
            {
                return;
            }

            string[] bannedCheckBoxes = {
                "checkBox1",
                "checkBox2",
                "checkBox3",
                "checkBox4",
                "checkBox5",
                "checkBox6",
                "checkBox7",
                "checkBox8",
                "checkBox9",
                "checkBox10",
                "checkBox11",
                "checkBox12"
            };

            foreach(Control checkBox in Controls)
            {
                if(checkBox is CheckBox && ((CheckBox)checkBox).Checked == true && !bannedCheckBoxes.Contains(checkBox.Name))
                {
                    Console.WriteLine("Editdssd " + ((CheckBox)checkBox).Text);
                    editToppings.Add(((CheckBox)checkBox).Text);
                }
            }

            try
            {
                Pizza editPizza = new Pizza(textBox4.Text, Double.Parse(textBox1.Text), editToppings);

                editPizza.EditPizza(comboBox1.SelectedIndex);

                label2.Text = $"Edited: {editPizza.GetName()}";

                RefreshData();
            }
            catch (Exception ex)
            {
                label2.Text = $"Failed: {ex.Message}";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == -1)
            {
                return;
            }

            try
            {
                Pizza deletePizza = new Pizza();
                
                label2.Text = $"Deleted: {pizzas[comboBox1.SelectedIndex].GetProperty("name")}";

                deletePizza.DeletePizza(comboBox1.SelectedIndex);
            
                RefreshData();
            
                comboBox1.SelectedIndex = -1;
                comboBox1.Text = "";
            }
            catch(Exception ex)
            {
                label2.Text = $"Failed to delete: {pizzas[comboBox1.SelectedIndex].GetProperty("name")}";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] bannedCheckBoxes = {
                "checkBox1",
                "checkBox2",
                "checkBox3",
                "checkBox4",
                "checkBox5",
                "checkBox6",
                "checkBox7",
                "checkBox8",
                "checkBox9",
                "checkBox10",
                "checkBox11",
                "checkBox12"
            };

            if (comboBox1.SelectedIndex == -1)
            {
                return;
            }

            textBox4.Text = pizzas[comboBox1.SelectedIndex].GetProperty("name").ToString();
            textBox1.Text = pizzas[comboBox1.SelectedIndex].GetProperty("price").ToString();

            foreach (Control checkBox in Controls)
            {
                if(checkBox is CheckBox && ((CheckBox)checkBox).Checked == true && !bannedCheckBoxes.Contains(checkBox.Name))
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json Files|*.json;";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                Properties.Settings.Default.JsonPath = filePath;
            }
        }
    }
}
