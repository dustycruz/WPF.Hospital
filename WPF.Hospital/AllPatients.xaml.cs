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
    /// Interaction logic for AllPatients.xaml
    /// </summary>
    public partial class AllPatients : Window
    {
        private readonly IPatientService _patientService;
        public AllPatients(IPatientService patientService)
        {
            InitializeComponent();
            _patientService = patientService;
            DataContext = new
            {
                Patients = _patientService.GetAll()
                .Select(p => new PatientViewModel()
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Age = p.Age,
                    Birthdate = p.BirthDate,
                })
            };

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgPatients.SelectedItem as PatientViewModel;

            if (selected == null)
            {
                MessageBox.Show("Please select a patient first.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updateWindow = new UpdatePatient(_patientService, selected);

            var result = updateWindow.ShowDialog();

            if (result == true)
            {
                // Refresh DataGrid after successful update
                RefreshPatients();
            }
        }
    }

}