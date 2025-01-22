using AdoNetCorePractica.Model;
using AdoNetCorePractica.Models;
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

        public async Task LoadEmpleados(string namehospital)
        {
            List<string> empleados = await this.repo.GetEmpleadosHospitalAsync(namehospital);
            EmpHospInfo model = await this.repo.GetEmpleadosHospital(namehospital);

            this.lblEmpHospital.Items.Clear();

            foreach (string empleado in empleados)
            {
                this.lblEmpHospital.Items.Add(empleado);
            }
            this.txtSuma.Text = model.SumaSalarial.ToString();
            this.txtMedia.Text = model.MediaSalarial.ToString();
            this.txtPersonas.Text = model.Personas.ToString();
        }



        private async void cmbHospitales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbHospitales.SelectedIndex != -1)
            {
                string nombreHospital = this.cmbHospitales.SelectedItem.ToString();

                await this.LoadEmpleados(nombreHospital);
            }
        }

    }
}
