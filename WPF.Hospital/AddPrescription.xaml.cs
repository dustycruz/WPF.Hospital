using System;
using System.Linq;
using System.Windows;
using WPF.Hospital.Model;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital
{
    public partial class AddPrescription : Window
    {
        private readonly IHistoryService _historyService;
        private readonly IMedicineService _medicineService;
        private readonly IPrescriptionService _prescriptionService;

        public AddPrescription(IHistoryService historyService, IMedicineService medicineService, IPrescriptionService prescriptionService)
        {
            InitializeComponent();
            _historyService = historyService;
            _medicineService = medicineService;
            _prescriptionService = prescriptionService;

            LoadHistories();
            LoadMedicines();
        }

        private void LoadHistories()
        {
            var histories = _historyService.GetAll()
                .Select(h => new
                {
                    Id = h.Id,
                    HistoryInfo = $"{h.Patient.FirstName} {h.Patient.LastName} - Dr. {h.Doctor.FirstName} {h.Doctor.LastName} - {h.Procedure}"
                })
                .ToList();

            cbxHistory.ItemsSource = histories;
        }

        private void LoadMedicines()
        {
            var medicines = _medicineService.GetAll()
                .Select(m => new
                {
                    Id = m.Id,
                    MedicineName = $"{m.Name} ({m.Brand})"
                })
                .ToList();

            cbxMedicine.ItemsSource = medicines;
        }

        private void btnAddPrescription_Click(object sender, RoutedEventArgs e)
        {
            if (cbxHistory.SelectedValue == null)
            {
                MessageBox.Show("A medical history must be selected.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (cbxMedicine.SelectedValue == null)
            {
                MessageBox.Show("A medicine must be selected.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be a number greater than 0.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFrequency.Text))
            {
                MessageBox.Show("Frequency is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            int historyId = (int)cbxHistory.SelectedValue;
            int medicineId = (int)cbxMedicine.SelectedValue;

            var history = _historyService.Get(historyId);
            if (history == null)
            {
                MessageBox.Show("Selected history does not exist.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var medicine = _medicineService.Get(medicineId);
            if (medicine == null)
            {
                MessageBox.Show("Selected medicine does not exist.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // ✅ SET IDS AND OBJECTS
            var prescriptionDto = new Prescription
            {
                HistoryId = historyId,
                History = history,
                MedicineId = medicineId,
                Medicine = medicine,
                Quantity = quantity,
                Frequency = txtFrequency.Text
            };

            var result = _prescriptionService.Create(prescriptionDto);

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

            cbxHistory.SelectedValue = null;
            cbxMedicine.SelectedValue = null;
            txtQuantity.Clear();
            txtFrequency.Clear();

            this.DialogResult = true;
            this.Close();
        }
    }
}