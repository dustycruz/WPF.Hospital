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

using System.Linq;
using System.Windows;
using WPF.Hospital.DTO;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital
{
    public partial class AddHistory : Window
    {
        private readonly IHistoryService _historyService;
        private readonly Patient _patient;

        public AddHistory(IHistoryService historyService, Patient patient)
        {
            InitializeComponent();
            _historyService = historyService;
            _patient = patient;
        }


        private void btnAddHistory_Click(object sender, RoutedEventArgs e)
        {
            // RULE: Procedure must not be empty
            if (string.IsNullOrWhiteSpace(txtProcedure.Text))
            {
                MessageBox.Show("Procedure description must not be empty.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var history = new History
            {
                Patient = _patient,
                Procedure = txtProcedure.Text
            };

            // SAVE USING SERVICE
            var result = _historyService.Create(history);

            if (!result.Ok)
            {
                MessageBox.Show(result.Message,
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // ATTACH HISTORY TO PATIENT (DTO RELATIONSHIP)
            if (_patient.History == null)
            {
                _patient.History = new List<History>();
            }

            ((List<History>)_patient.History).Add(history);

            DialogResult = true;
            Close();
        }
    }
}