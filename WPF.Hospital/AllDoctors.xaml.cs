using System;
using System.Linq;
using System.Windows;
using WPF.Hospital.Service.Interface;
using WPF.Hospital.ViewModel;

namespace WPF.Hospital
{
    public partial class AllDoctors : Window
    {
        private readonly IDoctorService _doctorService;

        public AllDoctors(IDoctorService doctorService)
        {
            InitializeComponent();
            _doctorService = doctorService;
            RefreshDoctors();
        }

        private void RefreshDoctors()
        {
            dgDoctors.ItemsSource = _doctorService.GetAll()
                .Select(d => new DoctorViewModel
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName
                })
                .ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgDoctors.SelectedItem as DoctorViewModel;

            if (selected == null)
            {
                MessageBox.Show("Please select a doctor first.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updateWindow = new UpdateDoctor(_doctorService, selected);
            var result = updateWindow.ShowDialog();

            if (result == true)
            {
                RefreshDoctors();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selected = dgDoctors.SelectedItem as DoctorViewModel;

            // RULE: Must select doctor
            if (selected == null)
            {
                MessageBox.Show("Please select a doctor first.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // RULE: Confirmation required
            var confirm = MessageBox.Show(
                $"Are you sure you want to delete {selected.FirstName} {selected.LastName}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            // CALL SERVICE
            var result = _doctorService.Delete(selected.Id);

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
            RefreshDoctors();
        }
    }
}