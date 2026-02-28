using System;
using System.Windows;
using WPF.Hospital.DTO;
using WPF.Hospital.Service.Interface;
using WPF.Hospital.ViewModel;

namespace WPF.Hospital
{
    public partial class UpdateDoctor : Window
    {
        private readonly IDoctorService _doctorService;
        private readonly DoctorViewModel _doctor;

        public UpdateDoctor(IDoctorService doctorService, DoctorViewModel doctor)
        {
            InitializeComponent();
            _doctorService = doctorService;
            _doctor = doctor;

            DataContext = doctor;
        }

        private void btnUpdateDoctor_Click(object sender, RoutedEventArgs e)
        {
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;

            if (string.IsNullOrWhiteSpace(firstName))
            {
                MessageBox.Show("First name is required.",
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("Last name is required.",
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var doctorDto = new Doctor
            {
                Id = _doctor.Id,
                FirstName = firstName,
                LastName = lastName
            };

            var result = _doctorService.Update(doctorDto);

            if (!result.Ok)
            {
                MessageBox.Show(result.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show(result.Message,
                "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            this.DialogResult = true;
            this.Close();
        }
    }
}