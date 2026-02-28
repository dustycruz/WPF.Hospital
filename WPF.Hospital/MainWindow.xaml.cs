using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.Hospital.Repository;
using WPF.Hospital.Service;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital
{
    public partial class MainWindow : Window
    {
        private readonly IPatientService _patientService;
        private readonly IHistoryService _historyService;
        private readonly IDoctorService _doctorService;

        public MainWindow(IPatientService patientService, IHistoryService historyService, IDoctorService doctorService)
        {
            InitializeComponent();
            _patientService = patientService;
            _historyService = historyService;
            _doctorService = doctorService;  // ✅ ADD THIS LINE
            this.WindowState = WindowState.Maximized;
        }

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        private void btnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            AddPatient addPatient = new AddPatient(_patientService);
            addPatient.ShowDialog();
        }

        private void btnAllPatients_Click(object sender, RoutedEventArgs e)
        {
            AllPatients allPatients = new AllPatients(_patientService, _historyService, _doctorService);
            allPatients.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DeletePatient DeletePatient = new DeletePatient(_patientService);
            DeletePatient.Show();
        }

        private void btnAddMedicine_Click(object sender, RoutedEventArgs e)
        {
            AddMedicine AddMedicine = new AddMedicine(_patientService);
            AddMedicine.Show();
        }

        private void btnAddDoctor_Click(object sender, RoutedEventArgs e)
        {
            AddDoctor AddDoctor = new AddDoctor(_doctorService);
            AddDoctor.Show();
        }

        private void btnDeleteDoctor_Click(object sender, RoutedEventArgs e)
        {
            var deleteWindow = new DeleteDoctor(_doctorService);
            deleteWindow.Owner = this;
            deleteWindow.ShowDialog();
        }

        private void btnAllDoctor_Click(object sender, RoutedEventArgs e)
        {
            var allDoctorsWindow = new AllDoctors(_doctorService);
            allDoctorsWindow.ShowDialog();
        }


    }
}