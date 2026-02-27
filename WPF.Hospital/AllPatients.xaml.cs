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
    public partial class AllPatients : Window
    {
        private readonly IPatientService _patientService;
        private readonly IHistoryService _historyService;

        public AllPatients(IPatientService patientService, IHistoryService historyService)
        {
            InitializeComponent();
            _patientService = patientService;

            RefreshPatients();
            _historyService = historyService;
        }

        private void RefreshPatients()
        {
            dgPatients.ItemsSource = _patientService.GetAll()
                .Select(p => new PatientViewModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Age = p.Age,
                    Birthdate = p.BirthDate
                })
                .ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgPatients.SelectedItem as PatientViewModel;

            // RULE: Must select patient
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
                RefreshPatients(); // refresh after successful update
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selected = dgPatients.SelectedItem as PatientViewModel;

            // RULE 1: Must select patient
            if (selected == null)
            {
                MessageBox.Show("Please select a patient first.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // RULE 2: Confirmation required
            var confirm = MessageBox.Show(
                $"Are you sure you want to delete {selected.FirstName} {selected.LastName}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            // RULE 3 & 4 handled in service
            var result = _patientService.Delete(selected.Id);

            if (!result.Ok)
            {
                MessageBox.Show(result.Message,
                    "Delete Blocked",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // SUCCESS
            MessageBox.Show(result.Message,
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            // Refresh grid
            RefreshPatients();

            // Clear dependent views
            // If you have these grids:
            // dgHistory.ItemsSource = null;
            // dgPrescriptions.ItemsSource = null;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            {
                var selected = dgPatients.SelectedItem as PatientViewModel;

                // RULE: Must select patient
                if (selected == null)
                {
                    MessageBox.Show("Please select a patient first.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                // Get full Patient DTO from service
                var patientDto = _patientService.Get(selected.Id);

                if (patientDto == null)
                {
                    MessageBox.Show("Selected patient does not exist.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                // Open Add History window
                var window = new AddHistory(_historyService, patientDto)
                {
                    Owner = this
                };

                var result = window.ShowDialog();

                if (result == true)
                {
                    MessageBox.Show("Medical history added successfully.",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    // OPTIONAL: Refresh history grid here if you have one
                    // dgHistory.ItemsSource = _historyService.GetByPatient(patientDto.Id);
                }
            }
        }
    }
}

