using System.Collections.Generic;
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

            DataContext = $"{patient.FirstName} {patient.LastName}";
        }

        private void btnAddHistory_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProcedure.Text))
            {
                MessageBox.Show("Procedure description must not be empty.");
                return;
            }

            var history = new History
            {
                Patient = _patient,
                Procedure = txtProcedure.Text
            };

            var result = _historyService.Create(history);

            if (!result.Ok)
            {
                MessageBox.Show(result.Message);
                return;
            }

            _patient.History.Add(history);

            DialogResult = true;
            Close();
        }
    }
}