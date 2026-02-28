using System;
using System.Windows;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital
{
    public partial class DeleteDoctor : Window
    {
        private readonly IDoctorService _doctorService;

        public DeleteDoctor(IDoctorService doctorService)
        {
            InitializeComponent();
            _doctorService = doctorService;
        }

        private void btnDeleteDoctor_Click(object sender, RoutedEventArgs e)
        {
            // Get the Doctor ID from TextBox
            string idText = tbPatientId.Text;

            // VALIDATION: Check if ID is provided
            if (string.IsNullOrWhiteSpace(idText))
            {
                MessageBox.Show("Doctor ID is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // VALIDATION: Check if ID is a valid integer
            if (!int.TryParse(idText, out int doctorId))
            {
                MessageBox.Show("Doctor ID must be a valid number.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // CONFIRMATION
            var confirm = MessageBox.Show(
                $"Are you sure you want to delete doctor with ID {doctorId}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            // CALL SERVICE
            var result = _doctorService.Delete(doctorId);

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