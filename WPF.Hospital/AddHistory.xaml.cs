using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WPF.Hospital.DTO;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital
{
    public partial class AddHistory : Window
    {
        private readonly IHistoryService _historyService;
        private readonly IDoctorService _doctorService;
        private readonly Patient _patient;
        private Doctor _selectedDoctor;

        public AddHistory(IHistoryService historyService, IDoctorService doctorService, Patient patient)
        {
            InitializeComponent();

            _historyService = historyService;
            _doctorService = doctorService;
            _patient = patient;

            DataContext = new
            {
                PatientName = $"{patient.FirstName} {patient.LastName}"
            };

            LoadDoctors();
        }

        private void LoadDoctors()
        {
            try
            {
                if (_doctorService == null)
                {
                    MessageBox.Show("Doctor service is not initialized.", "Error");
                    return;
                }

                var doctors = _doctorService.GetAll().ToList();

                if (doctors == null || doctors.Count == 0)
                {
                    MessageBox.Show("No doctors available in the database.", "Warning");
                    return;
                }

                cbxDoctor.ItemsSource = doctors;
                cbxDoctor.DisplayMemberPath = "FirstName";  // ✅ Set display path
                cbxDoctor.SelectedValuePath = "Id";          // ✅ Set value path
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading doctors: {ex.Message}", "Error");
            }
        }

        private void btnAddHistory_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProcedure.Text))
            {
                MessageBox.Show("Procedure is required.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _selectedDoctor = cbxDoctor.SelectedItem as Doctor;
            if (_selectedDoctor == null)
            {
                MessageBox.Show("A doctor must be selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = _historyService.Create(new History
            {
                Patient = _patient,
                Doctor = _selectedDoctor,
                Procedure = txtProcedure.Text
            });

            if (!result.Ok)
            {
                MessageBox.Show(result.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show(result.Message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
            Close();
        }
    }
}