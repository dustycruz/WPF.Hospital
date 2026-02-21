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
    /// Interaction logic for AddMedicine.xaml
    /// </summary>
    public partial class AddMedicine : Window
    {
        private readonly IPatientService _patientService;

        public AddMedicine(IPatientService patientService)
        {
            InitializeComponent();
            _patientService = patientService;
            DataContext = new MedicineViewModel();
        }

        private void btnAddMedicine_Click(object sender, RoutedEventArgs e)
        {
            _patientService.Add(new DTO.Medicine()
            {
                Name = ((MedicineViewModel)DataContext).Name,
                Brand = ((MedicineViewModel)DataContext).Brand,
            });

            MessageBox.Show("Medicine Addded Succesfully!");
        }
    }
}
