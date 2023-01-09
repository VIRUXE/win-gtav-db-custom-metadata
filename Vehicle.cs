using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gtavvehicles
{
    internal class Vehicle
    {
        // The database doesn't care if the values are a string or an int so we'll just use strings
        
        public string model { get; set; }
        public string key_type { get; set; }
        public string capacity_fuel { get; set; }
        public string capacity_oil { get; set; }
        public string capacity_radiator { get; set; }
        public string capacity_trunk { get; set; }
        public string capacity_glovebox { get; set; }
        public string fuel_type { get; set; }
        public string factory_price { get; set; }

        public Vehicle(string[] data)
        {
            this.model = data[0];
            this.key_type = data[1];
            this.capacity_fuel = data[2];
            this.capacity_oil = data[3];
            this.capacity_radiator = data[4];
            this.capacity_trunk = data[5];
            this.capacity_glovebox = data[6];
            this.fuel_type = data[7];
            this.factory_price = data[9];
        }
    }
}
