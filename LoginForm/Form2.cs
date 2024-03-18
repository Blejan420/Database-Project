using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;
using System.Linq;
using System.Data;
using System.ComponentModel;

namespace LoginForm
{
    public partial class Form2 : Form
    {
        private PictureBox gifPictureBox; // Declare a PictureBox for the GIF
        private string connectionString = "Data Source=ANDREI\\SQLEXPRESS; Initial Catalog=cabinet_veterinar; Integrated Security=True";

        public Form2()
        {
            InitializeComponent();
            LoadGifBackground();  // Call the method to load the GIF background
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'cabinet_veterinarDataSet1.proprietari' table. You can move, or remove it, as needed.
            this.proprietariTableAdapter.Fill(this.cabinet_veterinarDataSet.proprietari);
            // TODO: This line of code loads data into the 'cabinet_veterinarDataSet1.animale' table. You can move, or remove it, as needed.
            this.animaleTableAdapter.Fill(this.cabinet_veterinarDataSet.animale);

            proprietariDataGridView.Columns[0].Visible = false;
            animaleDataGridView.Columns[0].Visible = false;
            animaleDataGridView.Columns[5].Visible = false;
            label1.Parent = gifPictureBox;
            label1.BackColor = Color.Transparent;
            label2.Parent = gifPictureBox;
            label2.BackColor = Color.Transparent;
            label3.Parent = gifPictureBox;
            label3.BackColor = Color.Transparent;
            label4.Parent = gifPictureBox;
            label4.BackColor = Color.Transparent;
        }

        private void animaleBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.animaleBindingSource.EndEdit();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string updateQuery = "UPDATE animale SET nume = @Nume, specie = @Specie, rasa = @Rasa, data_nasterii = @DataNasterii, proprietar_id = @ProprietarId WHERE animal_id = @AnimalId";

                    foreach (DataRowView rowView in animaleBindingSource.List)
                    {
                        DataRow row = rowView.Row;

                        using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@Nume", row["nume"]);
                            cmd.Parameters.AddWithValue("@Specie", row["specie"]);
                            cmd.Parameters.AddWithValue("@Rasa", row["rasa"]);
                            cmd.Parameters.AddWithValue("@DataNasterii", row["data_nasterii"]);
                            cmd.Parameters.AddWithValue("@ProprietarId", row["proprietar_id"]);
                            cmd.Parameters.AddWithValue("@AnimalId", row["animal_id"]);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Salvare reusita!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la salvare: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadGifBackground()
        {
            string gifUrl = "https://i.pinimg.com/originals/08/fd/e1/08fde17968a7c30f4aed9abb7763f383.gif";

            try
            {
                gifPictureBox = new PictureBox();
                gifPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                gifPictureBox.Dock = DockStyle.Fill;

                WebClient webClient = new WebClient();
                Stream stream = webClient.OpenRead(gifUrl);
                Image gifImage = Image.FromStream(stream);

                gifPictureBox.Image = gifImage;

                this.Controls.Add(gifPictureBox);

                this.ClientSize = new Size(gifImage.Width, gifImage.Height);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading GIF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }
        private void animaleBindingNavigator_RefreshItems(object sender, EventArgs e)
        {

        }
        private void btnExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.proprietariBindingSource.EndEdit();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string updateQuery = "UPDATE proprietari SET nume = @Nume, prenume = @Prenume, adresa = @Adresa, telefon = @Telefon WHERE proprietar_id = @ProprietarId";

                    foreach (DataRowView rowView in proprietariBindingSource.List)
                    {
                        DataRow row = rowView.Row;

                        using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@Nume", row["nume"]);
                            cmd.Parameters.AddWithValue("@Prenume", row["prenume"]);
                            cmd.Parameters.AddWithValue("@Adresa", row["adresa"]);
                            cmd.Parameters.AddWithValue("@Telefon", row["telefon"]);
                            cmd.Parameters.AddWithValue("@ProprietarId", row["proprietar_id"]);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Salvare reusita!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la salvare: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                int maxAnimalId = GetMaxAnimalId(connection);

                string insertQuery = "INSERT INTO animale (animal_id, nume, specie, rasa, data_nasterii, proprietar_id) " +
                                     "VALUES (@AnimalId, @Nume, @Specie, @Rasa, @DataNasterii, @Id_Proprietar)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@AnimalId", maxAnimalId + 1);
                    cmd.Parameters.AddWithValue("@Nume", numeTextBox.Text);
                    cmd.Parameters.AddWithValue("@Specie", specieTextBox.Text);
                    cmd.Parameters.AddWithValue("@Rasa", rasaTextBox.Text);
                    cmd.Parameters.AddWithValue("@DataNasterii", data_nasteriiDateTimePicker.Value);
                    cmd.Parameters.AddWithValue("@Id_Proprietar", Convert.ToInt32(proprietar_idTextBox.Text));

                    cmd.ExecuteNonQuery();
                }
            }

            animaleTableAdapter.Fill(cabinet_veterinarDataSet.animale);
        }
        private int GetMaxAnimalId(SqlConnection connection)
        {
            string query = "SELECT MAX(animal_id) FROM animale";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                object result = cmd.ExecuteScalar();

                if (result != DBNull.Value && result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    return 0;
                }
            }
        }

        // INTEROGARI
        private void ExecuteQuery(string query, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    string resultMessage = "Cod SQL: SELECT m.nume AS medic_name, m.prenume, v.vizita_id, v.animal_id, v.data_vizitei, v.diagnostic FROM medici m LEFT JOIN vizite v ON m.medic_id = v.medic_id;\n\n";

                    while (reader.Read())
                    {
                        resultMessage += $"Nume medic: {reader["medic_name"]} {reader["prenume"]}, " +
                                          $"ID vizita: {(int)reader["vizita_id"]}, ID animal: {(int)reader["animal_id"]}, " +
                                          $"Data vizitei: {((DateTime)reader["data_vizitei"]).ToString("yyyy-MM-dd")}, Diagnostic: {reader["diagnostic"]}\n";
                    }

                    MessageBox.Show(resultMessage);
                }
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query1 = "SELECT m.medic_id, m.nume AS medic_name, m.prenume, " +
                    "v.vizita_id, v.animal_id, v.data_vizitei, v.diagnostic FROM medici m " +
                    "LEFT JOIN vizite v ON m.medic_id = v.medic_id;";
                ExecuteQuery(query1, connection);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query2 = "SELECT a.nume AS animal_name, p.nume AS owner_last_name, " +
                    "p.prenume AS owner_first_name " +
                    "FROM animale a " +
                    "JOIN proprietari p ON a.proprietar_id = p.proprietar_id";
                ExecuteQuery1(query2, connection);
            }
        }

        private void ExecuteQuery1(string query, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    string resultMessage = "Cod SQL:  SELECT a.nume AS animal_name, p.nume AS owner_last_name, p.prenume AS owner_first_name FROM animale a JOIN proprietari p ON a.proprietar_id = p.proprietar_id\n\n";

                    while (reader.Read())
                    {
                        resultMessage += $"Animal: {reader["animal_name"].ToString()}, Proprietar: {reader["owner_last_name"].ToString()} {reader["owner_first_name"].ToString()}\n";
                    }
                    MessageBox.Show(resultMessage);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query3 = "SELECT p.nume AS owner_last_name, p.prenume AS owner_first_name, " +
                        "a.nume AS animal_name, a.specie, a.rasa, a.data_nasterii " +
                        "FROM animale a " +
                        "JOIN proprietari p ON a.proprietar_id = p.proprietar_id " +
                        "WHERE a.data_nasterii > '2019-12-31'"; ;
                ExecuteQuery2(query3, connection);
            }
        }

        private void ExecuteQuery2(string query, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    string resultMessage = "Cod SQL: SELECT p.nume AS owner_last_name, p.prenume AS owner_first_name, a.nume AS animal_name, a.specie, a.rasa, a.data_nasterii FROM animale a JOIN proprietari p ON a.proprietar_id = p.proprietar_id WHERE a.data_nasterii > '2019-12-31'; \n\n";

                    while (reader.Read())
                    {
                        DateTime dataNasterii = (DateTime)reader["data_nasterii"];

                        resultMessage += $"Proprietar: {reader["owner_last_name"].ToString()} {reader["owner_first_name"].ToString()}, " +
                                          $"Animal: {reader["animal_name"].ToString()}, Specie: {reader["specie"].ToString()}, Rasa: {reader["rasa"].ToString()}, " +
                                          $"Data Nasterii: {dataNasterii.ToString("yyyy-MM-dd")}\n";
                    }

                    MessageBox.Show(resultMessage);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string animalNameToSearch = InputBox("Introduceti numele unui animal", "Cautare animal");

            if (!string.IsNullOrWhiteSpace(animalNameToSearch))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query4 = "SELECT p.nume, p.prenume, p.adresa, p.telefon, " +
                                    "(SELECT COUNT(*) FROM animale WHERE proprietar_id = p.proprietar_id) AS num_of_animals_owned " +
                                    "FROM animale a " +
                                    "JOIN proprietari p ON a.proprietar_id = p.proprietar_id " +
                                    "WHERE a.nume = @AnimalName";

                    ExecuteQuery3(query4, connection, animalNameToSearch);
                }
            }
        }

        private void ExecuteQuery3(string query, SqlConnection connection, string animalName)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@AnimalName", animalName);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    string resultMessage = "Cod SQL: SELECT p.nume, p.prenume, p.adresa, p.telefon, " +
                                           "(SELECT COUNT(*) FROM animale WHERE proprietar_id = p.proprietar_id) AS num_of_animals_owned " +
                                           "FROM animale a JOIN proprietari p ON a.proprietar_id = p.proprietar_id WHERE a.nume = @AnimalName \n\n";

                    while (reader.Read())
                    {
                        resultMessage += $"Nume proprietar: {reader["nume"].ToString()}, Prenume proprietar: {reader["prenume"].ToString()}, " +
                                         $"Adresa: {reader["adresa"].ToString()}, Telefon: {reader["telefon"].ToString()}, " +
                                         $"Numar de animale detinute: {(int)reader["num_of_animals_owned"]}\n";
                    }

                    MessageBox.Show(resultMessage);
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query5 =
                    "SELECT p.proprietar_id, p.nume, p.prenume, " +
                    "(SELECT COUNT(a.animal_id) FROM animale a WHERE a.proprietar_id = p.proprietar_id) AS num_of_animals " +
                    "FROM proprietari p";

                ExecuteQuery4(query5, connection);
            }
        }

        private void ExecuteQuery4(string query, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    string resultMessage = "Cod SQL: SELECT p.proprietar_id, p.nume, p.prenume, " +
                                           "(SELECT COUNT(a.animal_id) FROM animale a WHERE a.proprietar_id = p.proprietar_id) AS num_of_animals " +
                                           "FROM proprietari p \n\n";

                    while (reader.Read())
                    {
                        resultMessage += $"Nume proprietar: {reader["nume"].ToString()} {reader["prenume"].ToString()}, " +
                                         $"Nr. de animale: {(int)reader["num_of_animals"]}\n";
                    }

                    MessageBox.Show(resultMessage);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query6 = "SELECT TOP 1 p.* FROM proprietari p JOIN animale a" +
                    " ON p.proprietar_id = a.proprietar_id ORDER BY a.data_nasterii ASC;";
                ExecuteQuery5(query6, connection);
            }
        }

        private void ExecuteQuery5(string query, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    string resultMessage = "Cod SQL: SELECT TOP 1 p.* FROM proprietari p JOIN animale a ON p.proprietar_id = a.proprietar_id ORDER BY a.data_nasterii ASC;\n\n";

                    while (reader.Read())
                    {
                        resultMessage += $"Nume: {reader["nume"].ToString()}, " +
                                         $"Prenume: {reader["prenume"].ToString()}, Adresa: {reader["adresa"].ToString()}, " +
                                         $"Telefon: {reader["telefon"].ToString()}";
                    }

                    MessageBox.Show(resultMessage);
                }
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query6 = @" SELECT a.animal_id, a.nume AS animal_name, a.specie, a.rasa, a.data_nasterii, v.vaccinare_id, v.data_analizei, 
                v.tip_analiza, v.rezultat FROM animale a LEFT JOIN vaccinari v ON a.animal_id = v.animal_id
                WHERE v.vaccinare_id IN (SELECT TOP 1 vaccinare_id FROM vaccinari WHERE animal_id = a.animal_id
                ORDER BY data_analizei DESC, vaccinare_id DESC); ";
                ExecuteQuery6(query6, connection);
            }
        }

        private void ExecuteQuery6(string query, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    string resultMessage = "Cod SQL: SELECT a.animal_id, a.nume AS animal_name, a.specie, a.rasa, a.data_nasterii, v.vaccinare_id, v.data_analizei, v.tip_analiza, v.rezultat FROM animale a LEFT JOIN vaccinari v ON a.animal_id = v.animal_id WHERE v.vaccinare_id IN (SELECT TOP 1 vaccinare_id FROM vaccinari WHERE animal_id = a.animal_id ORDER BY data_analizei DESC, vaccinare_id DESC); \n\n";

                    while (reader.Read())
                    {
                        int animalId = (int)reader["animal_id"];
                        string animalName = reader["animal_name"].ToString();
                        string species = reader["specie"].ToString();
                        string breed = reader["rasa"].ToString();
                        DateTime birthDate = (DateTime)reader["data_nasterii"];
                        int vaccinationId = reader["vaccinare_id"] != DBNull.Value ? (int)reader["vaccinare_id"] : -1;
                        DateTime vaccinationDate = reader["data_analizei"] != DBNull.Value ? (DateTime)reader["data_analizei"] : DateTime.MinValue;
                        string vaccinationType = reader["tip_analiza"].ToString();
                        string vaccinationResult = reader["rezultat"].ToString();
                        string formattedBirthDate = birthDate.ToShortDateString();;
                        string formattedVaccinationDate = vaccinationDate != DateTime.MinValue ? vaccinationDate.ToShortDateString() : "N/A";
                        resultMessage += $"ID: {animalId}, Nume: {animalName}, Data nasterii: {formattedBirthDate}" +
                        $":\nVaccin ID: {vaccinationId}, Data vaccinarii: {formattedVaccinationDate}, Tipul vaccinului: {vaccinationType}, Rezultat: {vaccinationResult}\n";
                    }

                    MessageBox.Show(resultMessage);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string speciesInput = InputBox("Introduceti specia:", "Alegere Specie");

            if (!string.IsNullOrEmpty(speciesInput))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT DISTINCT p.*
                             FROM animale a
                             JOIN proprietari p ON a.proprietar_id = p.proprietar_id
                             WHERE a.specie = @inputSpecies;";

                    ExecuteQuery7(query, connection, speciesInput);
                }
            }
        }

        private void ExecuteQuery7(string query, SqlConnection connection, string speciesInput)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@inputSpecies", speciesInput);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    string resultMessage = "Cod SQL: SELECT DISTINCT p.* FROM animale a JOIN proprietari p ON a.proprietar_id = p.proprietar_id WHERE a.specie = @inputSpecies;\n\n";

                    while (reader.Read())
                    {
                        resultMessage += $"Nume: {reader["nume"].ToString()}, " +
                                         $"Prenume: {reader["prenume"].ToString()}, Adresa: {reader["adresa"].ToString()}, " +
                                         $"Telefon: {reader["telefon"].ToString()}\n";
                    }

                    MessageBox.Show(resultMessage);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string thresholdInput = InputBox("Introduceti un an:", "Alegere anul de prag");

            if (!string.IsNullOrEmpty(thresholdInput))
            {
                if (int.TryParse(thresholdInput, out int birthYearThreshold))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query9 = @"SELECT 
                                  (SELECT SUM(DATEDIFF(YEAR, a1.data_nasterii, GETDATE())) 
                                   FROM animale a1 
                                   WHERE YEAR(a1.data_nasterii) > @BirthYearThreshold) AS total_age 
                                   FROM animale a 
                                   WHERE YEAR(a.data_nasterii) > @BirthYearThreshold;";

                        ExecuteQuery8(query9, connection, birthYearThreshold);
                    }
                }
                else
                {
                    MessageBox.Show("An invalid!");
                }
            }
        }

        private void ExecuteQuery8(string query, SqlConnection connection, int birthYearThreshold)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BirthYearThreshold", birthYearThreshold);

                string resultMessage = "Cod SQL: SELECT (SELECT SUM(DATEDIFF(YEAR, a1.data_nasterii, GETDATE())) FROM animale a1 WHERE YEAR(a1.data_nasterii) > @BirthYearThreshold) AS total_age FROM animale a WHERE YEAR(a.data_nasterii) > @BirthYearThreshold; \n\n";

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        object totalAgeObj = reader[0];

                        if (totalAgeObj != DBNull.Value && Convert.ToInt32(totalAgeObj) > 0)
                        {
                            int totalAge = Convert.ToInt32(totalAgeObj);
                            resultMessage += $"Suma varstelor animalelor nascute dupa {birthYearThreshold}: {totalAgeObj} ani";
                            MessageBox.Show(resultMessage);
                        }
                        else
                        {
                            MessageBox.Show("Nu exista animale nascute dupa anul specificat.");
                        }
                    }
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query10 = @"SELECT a.* 
                           FROM animale a 
                           JOIN proprietari p ON a.proprietar_id = p.proprietar_id
                           WHERE p.nume LIKE '%escu';";
                ExecuteQuery9(query10, connection);
            }
        }

        private void ExecuteQuery9(string query, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    string resultMessage = "Cod SQL: SELECT a.* FROM animale a JOIN proprietari p ON a.proprietar_id = p.proprietar_id WHERE p.nume LIKE '%escu';\n\n";
                    while (reader.Read())
                    {
                        resultMessage += $"ID: {(int)reader["animal_id"]}, Nume: {reader["nume"].ToString()}, Specie: {reader["specie"].ToString()}, Rasa: {reader["rasa"].ToString()}, Data nasterii: {((DateTime)reader["data_nasterii"]).ToString("yyyy-MM-dd")}\n";
                    }

                    MessageBox.Show(resultMessage);
                }
            }
        }

        private string InputBox(string prompt, string title)
        {
            Form promptForm = new Form()
            {
                Width = 400,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = title,
                StartPosition = FormStartPosition.CenterScreen
            };

            System.Windows.Forms.Label textLabel = new System.Windows.Forms.Label() { Left = 20, Top = 20, Text = prompt, Height = 50, Width = 350 };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 300 };
            Button confirmation = new Button() { Text = "OK", Left = 250, Width = 100, Top = 70, DialogResult = DialogResult.OK };

            confirmation.Click += (sender, e) => { promptForm.Close(); };

            promptForm.Controls.Add(textBox);
            promptForm.Controls.Add(confirmation);
            promptForm.Controls.Add(textLabel);

            return promptForm.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void numeLabel1_Click(object sender, EventArgs e)
        {

        }

        private void numeTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void prenumeLabel_Click(object sender, EventArgs e)
        {

        }

        private void prenumeTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void adresaLabel_Click(object sender, EventArgs e)
        {

        }

        private void adresaTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void telefonLabel_Click(object sender, EventArgs e)
        {

        }

        private void telefonTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void proprietariDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void animaleDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void numeLabel_Click(object sender, EventArgs e)
        {

        }

        private void numeTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void specieLabel_Click(object sender, EventArgs e)
        {

        }

        private void specieTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void rasaLabel_Click(object sender, EventArgs e)
        {

        }

        private void rasaTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void data_nasteriiLabel_Click(object sender, EventArgs e)
        {

        }

        private void data_nasteriiDateTimePicker_ValueChanged(object sender, EventArgs e)
        {

        }

        private void proprietar_idLabel_Click(object sender, EventArgs e)
        {

        }

        private void proprietar_idTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                int maxProprietarId = GetMaxProprietarId(connection);

                string insertQuery = "INSERT INTO proprietari (proprietar_id, nume, prenume, adresa, telefon) " +
                                     "VALUES (@ProprietarId, @Nume, @Prenume, @Adresa, @Telefon)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@ProprietarId", maxProprietarId + 1);
                    cmd.Parameters.AddWithValue("@Nume", numeTextBox1.Text);
                    cmd.Parameters.AddWithValue("@Prenume", prenumeTextBox.Text);
                    cmd.Parameters.AddWithValue("@Adresa", adresaTextBox.Text);
                    cmd.Parameters.AddWithValue("@Telefon", telefonTextBox.Text);

                    cmd.ExecuteNonQuery();
                }
            }

            proprietariTableAdapter.Fill(cabinet_veterinarDataSet.proprietari);
        }
        private int GetMaxProprietarId(SqlConnection connection)
        {
            string query = "SELECT MAX(proprietar_id) FROM proprietari";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                object result = cmd.ExecuteScalar();

                if (result != DBNull.Value && result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    return 0;
                }
            }
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Sunteti sigur ca vreti sa stergeti aceste date?", "Confirmare", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DataRowView currentDataRowView = (DataRowView)animaleBindingSource.Current;

                    if (currentDataRowView != null)
                    {
                        DataRow currentDataRow = currentDataRowView.Row;

                        if (currentDataRow != null)
                        {
                            int animalId = Convert.ToInt32(currentDataRow["animal_id"]);

                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();

                                string deleteAnimalQuery = "DELETE FROM animale WHERE animal_id = @AnimalId";

                                using (SqlCommand deleteAnimalCmd = new SqlCommand(deleteAnimalQuery, connection))
                                {
                                    deleteAnimalCmd.Parameters.AddWithValue("@AnimalId", animalId);
                                    deleteAnimalCmd.ExecuteNonQuery();
                                }
                            }

                            animaleTableAdapter.Fill(cabinet_veterinarDataSet.animale);
                        }
                    }
                }
                catch (DBConcurrencyException ex)
                {
                    MessageBox.Show("Concurrency violation: The data has been modified by another user. Please refresh and try again.", "Concurrency Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    cabinet_veterinarDataSet.animale.Clear();
                    animaleTableAdapter.Fill(cabinet_veterinarDataSet.animale);
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Sunteti sigur ca vreti sa stergeti aceste date?", "Confirmare", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                DataRowView currentDataRowView = (DataRowView)proprietariBindingSource.Current;

                if (currentDataRowView != null)
                {
                    DataRow currentDataRow = currentDataRowView.Row;

                    if (currentDataRow != null)
                    {
                        int proprietarId = Convert.ToInt32(currentDataRow["proprietar_id"]);

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            string deleteProprietarQuery = "DELETE FROM proprietari WHERE proprietar_id = @ProprietarId";

                            using (SqlCommand deleteProprietarCmd = new SqlCommand(deleteProprietarQuery, connection))
                            {
                                deleteProprietarCmd.Parameters.AddWithValue("@ProprietarId", proprietarId);
                                deleteProprietarCmd.ExecuteNonQuery();
                            }
                        }

                        proprietariTableAdapter.Fill(cabinet_veterinarDataSet.proprietari);
                    }
                }
            }
        }
    }
}


