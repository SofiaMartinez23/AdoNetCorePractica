using AdoNetCorePractica.Repository;

namespace AdoNetCorePractica
{
    public partial class Form1 : Form
    {
        RepositoryHospitales repo;
        public Form1()
        {
            InitializeComponent();
            this.repo = new RepositoryHospitales();
            this.LoadHospitales();
        }

        public async Task LoadHospitales()
        {
            List<string> hospitales = await this.repo.GetHospitalesAsync();
            this.cmbHospitales.Items.Clear();

            foreach (string name in hospitales)
            {
                this.cmbHospitales.Items.Add(name);
            }

        }

        private async void cmbHospitales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbHospitales.SelectedIndex == -1)
            {
                string nombre = this.cmbHospitales.SelectedItem.ToString();
            }
        }
    }
}
