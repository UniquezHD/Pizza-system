using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace idk.src.Class
{
    public class Pizza
    {
        private string name;
        private double price;
        private List<string> ingredients;
        private int amount;

        public Pizza(string name, double price, List<string> ingredients, int amount)
        {
            this.name = name;
            this.price = price;
            this.ingredients = ingredients;
            this.amount = amount;
        }

        public Pizza(string name, double price, List<string> ingredients)
        {
            this.name = name;
            this.price = price;
            this.ingredients = ingredients;
        }

        public Pizza() { }

        public void EditPizza(int index)
        {
            JsonArray ingredientsArray = new JsonArray();
            foreach(var ingredient in ingredients)
            {
                ingredientsArray.Add(ingredient);
            }

            JsonObject newPizza = new JsonObject
            {
                ["name"] = name,
                ["price"] = price,
                ["ingredients"] = new JsonArray(ingredientsArray)
            };

            string json;
            using(StreamReader r = new StreamReader(Properties.Settings.Default.JsonPath))
            {
                json = r.ReadToEnd();
                r.Close();
            }

            JsonObject jsonObject = JsonNode.Parse(json)?.AsObject();

            if(jsonObject != null)
            {
                JsonArray pizzas = jsonObject["pizzas"]?.AsArray() ?? new JsonArray();

                pizzas.RemoveAt(index);

                pizzas.Insert(index, newPizza);

                jsonObject["pizzas"] = pizzas;

                File.WriteAllText(Properties.Settings.Default.JsonPath, jsonObject.ToString());
            }
        }

        public void DeletePizza(int index)
        {
            string json;
            using(StreamReader r = new StreamReader(Properties.Settings.Default.JsonPath))
            {
                json = r.ReadToEnd();
                r.Close();
            }

            JsonObject jsonObject = JsonNode.Parse(json)?.AsObject();      

            if(jsonObject != null)
            {
                JsonArray pizzas = jsonObject["pizzas"]?.AsArray() ?? new JsonArray();

                pizzas.RemoveAt(index);

                jsonObject["pizzas"] = pizzas;

                File.WriteAllText(Properties.Settings.Default.JsonPath, jsonObject.ToString());
            }
        }

        public void SavePizza()
        {
            JsonArray ingredientsArray = new JsonArray();
            foreach(var ingredient in ingredients)
            {
                ingredientsArray.Add(ingredient);
            }

            JsonObject newPizza = new JsonObject
            {
                ["name"] = name,
                ["price"] = price,
                ["ingredients"] = new JsonArray(ingredientsArray) 
            };

            string json;
            using(StreamReader r = new StreamReader(Properties.Settings.Default.JsonPath))
            {
                json = r.ReadToEnd();
                r.Close();
            }

            JsonObject jsonObject = JsonNode.Parse(json)?.AsObject();

            if(jsonObject != null)
            {
                JsonArray pizzas = jsonObject["pizzas"]?.AsArray() ?? new JsonArray();

                pizzas.Add(newPizza);

                jsonObject["pizzas"] = pizzas;

                File.WriteAllText(Properties.Settings.Default.JsonPath, jsonObject.ToString());
            }
        }

        public string GetName()
        {
            return name;
        }

        public int GetAmount()
        {
            return amount;
        }

        public double GetPrice()
        {
            return price;
        }

        public List<string> GetIngredients()
        {
            return ingredients;
        }

        public int GetIngredientsLength()
        {
            return ingredients.Count;
        }
    }
}
