using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF.Hospital.Service.Interface;
using WPF.Hospital.ViewModel;

namespace WPF.Hospital
{
    /// <summary>
    /// Interaction logic for DeletePatient.xaml
    /// </summary>
    public partial class DeletePatient : Window
    {
        private readonly IPatientService _patientService;
        public DeletePatient(IPatientService patientService)
        {
            InitializeComponent();
            _patientService = patientService;
            DataContext = new
                {
                    Patients = _patientService.GetAll()
                .Select(p => new PatientViewModel()
                {
                    Id = p.Id,
                })
                };

        }

        private void btnDeletePatient_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbPatientId.Text))
            {
                MessageBox.Show("Please enter a patient id");
                return;
            }
            if (DataContext == null)
            {
                MessageBox.Show("Id not found");
               
            }
            else            {
                _patientService.Delete(Convert.ToInt32(tbPatientId.Text));
                MessageBox.Show("Patient Deleted Succesfully!");
            }



        }
    }
}
