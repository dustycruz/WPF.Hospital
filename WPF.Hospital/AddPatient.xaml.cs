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
    /// <summary>
    /// Interaction logic for AddPatient.xaml
    /// </summary>
    public partial class AddPatient : Window
    {
        private readonly IPatientService _patientService;
        public AddPatient(IPatientService patientService)
        {
            InitializeComponent();
            _patientService = patientService;
            DataContext = new PatientViewModel { Birthdate = DateTime.Now };

        }

        private void btnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            var vm = (PatientViewModel)DataContext;

            var dto = new DTO.Patient
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Age = vm.Age,
                BirthDate = vm.Birthdate
            };

            var result = _patientService.Create(dto);

            if (result.Ok)
            {
                MessageBox.Show(result.Message, "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // Clear fields
                vm.FirstName = string.Empty;
                vm.LastName = string.Empty;
                vm.Age = 0;
                vm.Birthdate = DateTime.Now;

                // Optional: Close window after success
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(result.Message, "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
