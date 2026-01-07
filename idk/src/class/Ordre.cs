using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idk.src
{
    public class Ordre
    {

        private string name;
        private string adress;
        private int phoneNumber;
        private bool isDelivery;

        public Ordre(string name, string adress, int phoneNumber, bool isDelivery) { 
            this.name = name;
            this.adress = adress;   
            this.phoneNumber = phoneNumber;
            this.isDelivery = isDelivery;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetPhoneNumber(int phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }

        public void SetAdress(string adress)
        {
            this.adress = adress;
        }

        public void SetAdress(bool isDelivery)
        {
            this.isDelivery = isDelivery;
        }

        public bool IsDelivery()
        {
            return isDelivery;
        }

        public int GetPhoneNumber()
        {
            return phoneNumber;
        }

        public string GetName()
        {
            return name;
        }

        public string GetAdress()
        {
            return adress;
        }
    }
}
