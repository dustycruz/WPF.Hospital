using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.DTO;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital.Service
{
    public class HistoryService : IHistoryService
    {
        private static readonly List<History> _histories = new();
        private readonly IPatientService _patientService;

        public HistoryService(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public IEnumerable<History> GetAll()
        {
            return _histories;
        }

        public History? Get(int id)
        {
            return _histories.FirstOrDefault(h => h.Id == id);
        }

        public IEnumerable<History> GetByPatient(int patientId)
        {
            return _histories
                .Where(h => h.Patient != null && h.Patient.Id == patientId);
        }

        public (bool Ok, string Message) Create(History entity)
        {
            // RULE: Patient must be selected
            if (entity.Patient == null)
                return (false, "A patient must be selected.");

            // RULE: Procedure must not be empty
            if (string.IsNullOrWhiteSpace(entity.Procedure))
                return (false, "Procedure description must not be empty.");

            // RULE: Patient must exist
            var existingPatient = _patientService.Get(entity.Patient.Id);
            if (existingPatient == null)
                return (false, "Selected patient does not exist.");

            // CONNECT DTO PROPERLY
            entity.Patient = existingPatient;

            // SAVE
            entity.Id = _histories.Count == 0
                ? 1
                : _histories.Max(h => h.Id) + 1;

            _histories.Add(entity);

            return (true, "Medical history saved successfully.");
        }

        public (bool Ok, string Message) Update(History entity)
        {
            var existing = Get(entity.Id);

            if (existing == null)
                return (false, "History record not found.");

            if (string.IsNullOrWhiteSpace(entity.Procedure))
                return (false, "Procedure description must not be empty.");

            existing.Procedure = entity.Procedure;

            return (true, "Medical history updated successfully.");
        }

        public (bool Ok, string Message) Delete(int id)
        {
            var history = Get(id);

            if (history == null)
                return (false, "History record not found.");

            _histories.Remove(history);

            return (true, "Medical history deleted successfully.");
        }
    }
}
