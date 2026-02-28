using System;
using System.Linq;
using System.Windows;
using WPF.Hospital.DTO;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital
{
    public partial class UpdateHistory : Window
    {
        private readonly IHistoryService _historyService;
        private readonly IDoctorService _doctorService;
        private readonly History _history;

        public UpdateHistory(IHistoryService historyService, IDoctorService doctorService, History history)
        {
            InitializeComponent();
            _historyService = historyService;
            _doctorService = doctorService;
            _history = history;

            DataContext = history;
            LoadDoctors();
            cbxDoctor.SelectedValue = history.Doctor.Id;
        }

        private void LoadDoctors()
        {
            var doctors = _doctorService.GetAll().ToList();
            cbxDoctor.ItemsSource = doctors;
        }

        private void btnUpdateHistory_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProcedure.Text))
            {
                MessageBox.Show("Procedure is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var selectedDoctor = cbxDoctor.SelectedItem as Doctor;
            if (selectedDoctor == null)
            {
                MessageBox.Show("A doctor must be selected.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var historyDto = new History
            {
                Id = _history.Id,
                Patient = _history.Patient,
                Doctor = selectedDoctor,
                Procedure = txtProcedure.Text
            };

            var result = _historyService.Update(historyDto);

            if (!result.Ok)
            {
                MessageBox.Show(result.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show(result.Message,
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            this.DialogResult = true;
            this.Close();
        }
    }
}