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
using WPF.Hospital.DTO;
using WPF.Hospital.Service.Interface;
using WPF.Hospital.ViewModel;

namespace WPF.Hospital
{
    public partial class UpdatePatient : Window
    {
        private readonly IPatientService _patientService;
        private readonly PatientViewModel _viewModel;

        public UpdatePatient(IPatientService patientService, PatientViewModel patient)
        {
            InitializeComponent();

            _patientService = patientService;

            _viewModel = new PatientViewModel
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Age = patient.Age,
                Birthdate = patient.Birthdate
            };

            DataContext = _viewModel;
        }

        private void btnUpdatePatient_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_viewModel.FirstName) ||
    string.IsNullOrWhiteSpace(_viewModel.LastName))
            {
                MessageBox.Show("All fields are required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            var existing = _patientService.Get(_viewModel.Id);

            if (existing == null)
            {
                MessageBox.Show("Patient no longer exists in database.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var dto = new Patient
            {
                Id = _viewModel.Id,
                FirstName = _viewModel.FirstName,
                LastName = _viewModel.LastName,
                Age = _viewModel.Age,
                BirthDate = _viewModel.Birthdate
            };

            var result = _patientService.Update(dto);

            if (result.Ok)
            {
                MessageBox.Show(result.Message,
                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show(result.Message,
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
