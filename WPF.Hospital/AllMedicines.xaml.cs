using System;
using System.Linq;
using System.Windows;
using WPF.Hospital.Service.Interface;
using WPF.Hospital.ViewModel;

namespace WPF.Hospital
{
    public partial class AllMedicines : Window
    {
        private readonly IMedicineService _medicineService;

        public AllMedicines(IMedicineService medicineService)
        {
            InitializeComponent();
            _medicineService = medicineService;
            RefreshMedicines();
        }

        private void RefreshMedicines()
        {
            dgMedicines.ItemsSource = _medicineService.GetAll()
                .Select(m => new MedicineViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Brand = m.Brand
                })
                .ToList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddMedicine(_medicineService)
            {
                Owner = this
            };

            var result = addWindow.ShowDialog();

            if (result == true)
            {
                RefreshMedicines();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgMedicines.SelectedItem as MedicineViewModel;

            if (selected == null)
            {
                MessageBox.Show("Please select a medicine first.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var updateWindow = new UpdateMedicine(_medicineService, selected)
            {
                Owner = this
            };

            var result = updateWindow.ShowDialog();

            if (result == true)
            {
                RefreshMedicines();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgMedicines.SelectedItem as MedicineViewModel;

            if (selected == null)
            {
                MessageBox.Show("Please select a medicine first.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete {selected.Name} ({selected.Brand})?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            var result = _medicineService.Delete(selected.Id);

            if (!result.Ok)
            {
                MessageBox.Show(result.Message,
                    "Delete Blocked",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show(result.Message,
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            RefreshMedicines();
        }
    }
}