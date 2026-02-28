using System;
using System.Collections.Generic;
using System.Linq;
using WPF.Hospital.Model;
using WPF.Hospital.Repository.Interface;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital.Service
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly IMedicineService _medicineService;

        public PrescriptionService(IPrescriptionRepository repository, IHistoryService historyService, IMedicineService medicineService)
        {
            _repository = repository;
            _historyService = historyService;
            _medicineService = medicineService;
        }

        public (bool Ok, string Message) Create(Prescription entity)
        {
            // VALIDATION: HistoryId must be valid
            if (entity.HistoryId <= 0)
                return (false, "A valid history must be selected.");

            var history = _historyService.Get(entity.HistoryId);
            if (history == null)
                return (false, "Selected history does not exist.");

            // VALIDATION: MedicineId must be valid
            if (entity.MedicineId <= 0)
                return (false, "A valid medicine must be selected.");

            var medicine = _medicineService.Get(entity.MedicineId);
            if (medicine == null)
                return (false, "Selected medicine does not exist.");

            // VALIDATION: Quantity must be greater than 0
            if (entity.Quantity <= 0)
                return (false, "Quantity must be greater than 0.");

            // VALIDATION: Frequency must not be empty
            if (string.IsNullOrWhiteSpace(entity.Frequency))
                return (false, "Frequency is required.");

            // VALIDATION: Prevent duplicate medicine per history
            if (DuplicateMedicineExists(entity.HistoryId, entity.MedicineId))
                return (false, "This medicine is already prescribed for this history.");

            var prescriptionEntity = new Prescription
            {
                HistoryId = entity.HistoryId,
                MedicineId = entity.MedicineId,
                Quantity = entity.Quantity,
                Frequency = entity.Frequency
            };

            _repository.Add(prescriptionEntity);
            _repository.Save();

            return (true, "Prescription created successfully.");
        }

        public (bool Ok, string Message) Delete(int id)
        {
            // VALIDATION: Prescription must exist
            var prescription = _repository.Get(id);
            if (prescription == null)
                return (false, "Prescription not found.");

            _repository.Delete(id);
            _repository.Save();

            return (true, "Prescription deleted successfully.");
        }

        public bool DuplicateMedicineExists(int historyId, int medicineId)
        {
            return _repository.GetByHistory(historyId)
                .Any(p => p.MedicineId == medicineId);
        }

        public Prescription? Get(int id)
        {
            return _repository.Get(id);
        }

        public IEnumerable<Prescription> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<Prescription> GetByHistory(int historyId)
        {
            return _repository.GetByHistory(historyId);
        }

        public (bool Ok, string Message) Update(Prescription entity)
        {
            // VALIDATION: Prescription must exist
            var model = _repository.Get(entity.Id);
            if (model == null)
                return (false, "Prescription not found.");

            // VALIDATION: Quantity must be greater than 0
            if (entity.Quantity <= 0)
                return (false, "Quantity must be greater than 0.");

            // VALIDATION: Frequency must not be empty
            if (string.IsNullOrWhiteSpace(entity.Frequency))
                return (false, "Frequency is required.");

            model.Quantity = entity.Quantity;
            model.Frequency = entity.Frequency;

            _repository.Update(model);
            _repository.Save();

            return (true, "Prescription updated successfully.");
        }
    }
}