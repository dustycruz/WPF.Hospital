using System;
using System.Linq;
using System.Windows;
using WPF.Hospital.DTO;
using WPF.Hospital.Service.Interface;
using WPF.Hospital.ViewModel;

namespace WPF.Hospital
{
    public partial class PatientHistory : Window
    {
        private readonly IHistoryService _historyService;
        private readonly IDoctorService _doctorService;
        private readonly Patient _patient;

        public PatientHistory(IHistoryService historyService, IDoctorService doctorService, Patient patient)
        {
            InitializeComponent();
            _historyService = historyService;
            _doctorService = doctorService;
            _patient = patient;

            tbPatientName.Text = $"Medical History - {patient.FirstName} {patient.LastName}";
            RefreshHistory();
        }

        private void RefreshHistory()
        {
            dgHistory.ItemsSource = _historyService.GetByPatient(_patient.Id)
                .Select(h => new HistoryViewModel
                {
                    Id = h.Id,
                    PatientName = $"{h.Patient.FirstName} {h.Patient.LastName}",
                    DoctorName = $"{h.Doctor.FirstName} {h.Doctor.LastName}",
                    Procedure = h.Procedure
                })
                .ToList();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgHistory.SelectedItem as HistoryViewModel;

            if (selected == null)
            {
                MessageBox.Show("Please select a medical history record first.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var historyDto = _historyService.Get(selected.Id);

            if (historyDto == null)
            {
                MessageBox.Show("History record not found.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var updateWindow = new UpdateHistory(_historyService, _doctorService, historyDto)
            {
                Owner = this
            };

            var result = updateWindow.ShowDialog();

            if (result == true)
            {
                RefreshHistory();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgHistory.SelectedItem as HistoryViewModel;

            if (selected == null)
            {
                MessageBox.Show("Please select a medical history record first.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete this medical history record?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            var result = _historyService.Delete(selected.Id);

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

            RefreshHistory();
        }
    }
}