using System;
using System.Linq;
using System.Windows;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital
{
    public partial class AllPrescriptions : Window
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IHistoryService _historyService;
        private readonly IMedicineService _medicineService;

        public AllPrescriptions(IPrescriptionService prescriptionService, IHistoryService historyService, IMedicineService medicineService)
        {
            InitializeComponent();
            _prescriptionService = prescriptionService;
            _historyService = historyService;
            _medicineService = medicineService;

            RefreshPrescriptions();
        }

        private void RefreshPrescriptions()
        {
            dgPrescriptions.ItemsSource = _prescriptionService.GetAll()
                .Select(p => new
                {
                    p.Id,
                    History = $"{p.History.Patient.FirstName} {p.History.Patient.LastName} - {p.History.Procedure}",
                    Medicine = $"{p.Medicine.Name} ({p.Medicine.Brand})",
                    p.Quantity,
                    p.Frequency
                })
                .ToList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddPrescription(_historyService, _medicineService, _prescriptionService)
            {
                Owner = this
            };

            var result = addWindow.ShowDialog();

            if (result == true)
            {
                RefreshPrescriptions();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            dynamic selected = dgPrescriptions.SelectedItem;

            if (selected == null)
            {
                MessageBox.Show("Please select a prescription first.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            int prescriptionId = selected.Id;

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete this prescription?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            var result = _prescriptionService.Delete(prescriptionId);

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

            RefreshPrescriptions();
        }
    }
}