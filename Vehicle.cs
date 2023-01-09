using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gtavvehicles
{
    internal class Vehicle
    {
        public enum Property
        {
            model,
            key_type,
            capacity_fuel,
            capacity_oil,
            capacity_radiator,
            capacity_trunk,
            capacity_glovebox,
            fuel_type,
            factory_price
        }

        public Dictionary<Property, string> properties = new();

        public Vehicle(string[] data)
        {
            properties.Add(Property.model, data[0]);
            properties.Add(Property.key_type, data[1]);
            properties.Add(Property.capacity_fuel, data[2]);
            properties.Add(Property.capacity_oil, data[3]);
            properties.Add(Property.capacity_radiator, data[4]);
            properties.Add(Property.capacity_trunk, data[5]);
            properties.Add(Property.capacity_glovebox, data[6]);
            properties.Add(Property.fuel_type, data[7]);
            properties.Add(Property.factory_price, data[9]);
        }
    }
}
