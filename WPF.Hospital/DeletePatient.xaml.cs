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
            // VALIDATION: Check if ID is provided
            if (string.IsNullOrWhiteSpace(tbPatientId.Text))
            {
                MessageBox.Show("Please enter a patient ID.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // VALIDATION: Check if ID is a valid integer
            if (!int.TryParse(tbPatientId.Text, out int patientId))
            {
                MessageBox.Show("Patient ID must be a valid number.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // CONFIRMATION: Ask user to confirm deletion
            var confirm = MessageBox.Show(
                $"Are you sure you want to delete patient with ID {patientId}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            // CALL SERVICE and check result
            var result = _patientService.Delete(patientId);

            if (!result.Ok)
            {
                MessageBox.Show(result.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // SUCCESS
            MessageBox.Show(result.Message,
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            tbPatientId.Clear();


        }
    }
}
