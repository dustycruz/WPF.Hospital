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
using WPF.Hospital.DTO;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital
{
    public partial class AddDoctor : Window
    {
        private readonly IDoctorService _doctorService;

        public AddDoctor(IDoctorService doctorService)
        {
            InitializeComponent();
            _doctorService = doctorService;
        }

        private void btnAddDoctor_Click(object sender, RoutedEventArgs e)
        {
            // Get values from TextBoxes
            string firstName = ((TextBox)this.FindName("txtFirstName")).Text;
            string lastName = ((TextBox)this.FindName("txtLastName")).Text;

            // VALIDATION: Check if fields are empty
            if (string.IsNullOrWhiteSpace(firstName))
            {
                MessageBox.Show("First name is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("Last name is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // CREATE DTO
            var doctorDto = new Doctor
            {
                FirstName = firstName,
                LastName = lastName
            };

            // CALL SERVICE
            var result = _doctorService.Create(doctorDto);

            if (!result.Ok)
            {
                MessageBox.Show(result.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // SUCCESS
            MessageBox.Show(result.Message,
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            // ✅ FIX: Set DialogResult first, then close

            this.Close();
        }
    }
}