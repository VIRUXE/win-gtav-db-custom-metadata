﻿using MySqlConnector;

namespace gtavvehicles
{
    public partial class frmMain : Form
    {
        public static MySqlConnection db = new();
        private static List<Vehicle> vehicles = new();
        private int currentVehicle = 0;
        private static Dictionary<string, string> vehicleImages = new();

        public frmMain()
        {
            InitializeComponent();

            ConnectionCredentials cc = new();

            // If the dialog doesn't come back as OK then close the application
            if (cc.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("You must enter a valid username and password to continue", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            else
            {
                // Execute a query to get all the vehicles that need to be edited
                MySqlCommand cmd = new("SELECT * FROM vehicles_metadata_custom WHERE fuel_type IS NULL;", db);
                MySqlDataReader reader = cmd.ExecuteReader();
                
                // Add each vehicle to the list
                while (reader.Read())
                {
                    // Put all the reader values inside an array
                    string[] data = new string[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        data[i] = reader.IsDBNull(i) ? "NULL" : reader.GetValue(i).ToString();
                    }
                    
                    vehicles.Add(new Vehicle(data));
                }

                // Close the reader
                reader.Close();

                this.Text = $"GTAV Vehicles - {vehicles.Count} vehicles";

                // Now we get the images from the "vehicles_images"
                // We'll use the model name as the key with "vehicleImages"
                cmd = new("SELECT model, url FROM vehicles_images", db);
                reader = cmd.ExecuteReader();

                // Add each vehicle to the list
                while (reader.Read())
                {
                    string model = reader.GetString(0);
                    string url = reader.GetString(1);

                    if (!vehicleImages.ContainsKey(model)) vehicleImages.Add(model, url);
                }

                // Close the reader
                reader.Close();
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Load the first vehicle in the list
            PopulateForm();
        }

        // Application Close
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Close the database connection
            db.Close();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            // If the current vehicle is the first one then don't do anything
            if (currentVehicle == 0)
                return;
            
            currentVehicle--;
            PopulateForm();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentVehicle == vehicles.Count - 1)
                return;

            currentVehicle++;
            PopulateForm();
        }

        private void PopulateForm()
        {
            Vehicle v = vehicles[currentVehicle];
            string model = v.properties[Vehicle.Property.model];

            this.Text = $"GTAV Vehicles - {currentVehicle + 1} / {vehicles.Count} - {model}";
            cmbKeyType.Text = v.properties[Vehicle.Property.key_type];
            txtFuelCapacity.Text = v.properties[Vehicle.Property.capacity_fuel];
            txtOilCapacity.Text = v.properties[Vehicle.Property.capacity_oil];
            txtRadiatorCapacity.Text = v.properties[Vehicle.Property.capacity_radiator];
            txtTrunkCapacity.Text = v.properties[Vehicle.Property.capacity_trunk];
            txtGloveboxCapacity.Text = v.properties[Vehicle.Property.capacity_glovebox];
            cmbFuelType.Text = v.properties[Vehicle.Property.fuel_type];
            txtFactoryPrice.Text = v.properties[Vehicle.Property.factory_price];
            
            // Try to load up the image we go from the database
            try
            {
                if (vehicleImages.ContainsKey(model))
                {
                    // We cancel any loading the could have been happening before
                    pctVehicle.CancelAsync();

                    pctVehicle.LoadAsync($"https://gtabase.com{vehicleImages[model]}");
                }
                else
                {
                    // Show that the image is loading
                    pctVehicle.Text = this.Text = $"{this.Text} (Sem imagem.)";
                    pctVehicle.Image = null;
                }
            }
            catch (Exception)
            {
                pctVehicle.Image = null;
            }

            // If the current vehicle was saved to the database, the button would be disabled, so enable it
            if (btnSave.Enabled == false) btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Vehicle v = vehicles[currentVehicle];
            // Don't do anything if the form values are the same as the database values
            if (cmbKeyType.Text == v.properties[Vehicle.Property.key_type] &&
                txtFuelCapacity.Text == v.properties[Vehicle.Property.capacity_fuel] &&
                txtOilCapacity.Text == v.properties[Vehicle.Property.capacity_oil] &&
                txtRadiatorCapacity.Text == v.properties[Vehicle.Property.capacity_radiator] &&
                txtTrunkCapacity.Text == v.properties[Vehicle.Property.capacity_trunk] &&
                txtGloveboxCapacity.Text == v.properties[Vehicle.Property.capacity_glovebox] &&
                cmbFuelType.Text == v.properties[Vehicle.Property.fuel_type] &&
                txtFactoryPrice.Text == v.properties[Vehicle.Property.factory_price])
                return;

            // Build the query string with the text from the form
            // We need to make sure that if any value is NULL we don't put apostrophes around it
            string query = $"UPDATE vehicles_metadata_custom SET " +
                           $"key_type = {(cmbKeyType.Text == "NULL" ? "NULL" : $"'{cmbKeyType.Text}'")}, " +
                           $"capacity_fuel = {(txtFuelCapacity.Text == "NULL" ? "NULL" : $"'{txtFuelCapacity.Text}'")}, " +
                           $"capacity_oil = {(txtOilCapacity.Text == "NULL" ? "NULL" : $"'{txtOilCapacity.Text}'")}, " +
                           $"capacity_radiator = {(txtRadiatorCapacity.Text == "NULL" ? "NULL" : $"'{txtRadiatorCapacity.Text}'")}, " +
                           $"capacity_trunk = {(txtTrunkCapacity.Text == "NULL" ? "NULL" : $"'{txtTrunkCapacity.Text}'")}, " +
                           $"capacity_glovebox = {(txtGloveboxCapacity.Text == "NULL" ? "NULL" : $"'{txtGloveboxCapacity.Text}'")}, " +
                           $"fuel_type = {(cmbFuelType.Text == "NULL" ? "NULL" : $"'{cmbFuelType.Text}'")}, " +
                           $"factory_price = {(txtFactoryPrice.Text == "NULL" ? "NULL" : $"'{txtFactoryPrice.Text}'")} " +
                           $"WHERE model = '{v.properties[Vehicle.Property.model]}';";
            MySqlCommand cmd = new(query, db);

            // Execute the query and show a message box with the result
            try
            {
                if (cmd.ExecuteNonQuery() > 0)
                {
                    // Update the vehicle object with the new values
                    v.properties[Vehicle.Property.key_type] = cmbKeyType.Text;
                    v.properties[Vehicle.Property.capacity_fuel] = txtFuelCapacity.Text;
                    v.properties[Vehicle.Property.capacity_oil] = txtOilCapacity.Text;
                    v.properties[Vehicle.Property.capacity_radiator] = txtRadiatorCapacity.Text;
                    v.properties[Vehicle.Property.capacity_trunk] = txtTrunkCapacity.Text;
                    v.properties[Vehicle.Property.capacity_glovebox] = txtGloveboxCapacity.Text;
                    v.properties[Vehicle.Property.fuel_type] = cmbFuelType.Text;
                    v.properties[Vehicle.Property.factory_price] = txtFactoryPrice.Text;

                    btnSave.Enabled = false;
                    // Focus on the next button
                    btnNext.Focus();
                }
                else
                    MessageBox.Show("Não foi possível guardar os dados!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao guardar os dados:\n\n{ex.Message}\n\nConsulta: {query}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            PopulateForm();
        }
    }
}
