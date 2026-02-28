using System;
using System.Windows;
using WPF.Hospital.Model;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital
{
    public partial class AddMedicine : Window
    {
        private readonly IMedicineService _medicineService;

        public AddMedicine(IMedicineService medicineService)
        {
            InitializeComponent();
            _medicineService = medicineService;
        }

        private void btnAddMedicine_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            string brand = txtBrand.Text.Trim();

            // VALIDATION: Name must not be empty
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Medicine name is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                txtName.Focus();
                return;
            }

            // VALIDATION: Brand must not be empty
            if (string.IsNullOrWhiteSpace(brand))
            {
                MessageBox.Show("Brand is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                txtBrand.Focus();
                return;
            }

            // ✅ USE MODEL.MEDICINE (NOT DTO)
            var medicineDto = new Medicine
            {
                Name = name,
                Brand = brand
            };

            var result = _medicineService.Create(medicineDto);

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

            txtName.Clear();
            txtBrand.Clear();
            txtName.Focus();


            this.Close();
        }
    }
}