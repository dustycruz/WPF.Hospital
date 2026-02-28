using System;
using System.Windows;
using WPF.Hospital.Model;
using WPF.Hospital.Service.Interface;
using WPF.Hospital.ViewModel;

namespace WPF.Hospital
{
    public partial class UpdateMedicine : Window
    {
        private readonly IMedicineService _medicineService;
        private readonly MedicineViewModel _medicine;

        public UpdateMedicine(IMedicineService medicineService, MedicineViewModel medicine)
        {
            InitializeComponent();
            _medicineService = medicineService;
            _medicine = medicine;

            txtName.Text = medicine.Name;
            txtBrand.Text = medicine.Brand;
        }

        private void btnUpdateMedicine_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            string brand = txtBrand.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Medicine name is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(brand))
            {
                MessageBox.Show("Brand is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                txtBrand.Focus();
                return;
            }

            // ✅ USE MODEL.MEDICINE
            var medicineDto = new Medicine
            {
                Id = _medicine.Id,
                Name = name,
                Brand = brand
            };

            var result = _medicineService.Update(medicineDto);

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