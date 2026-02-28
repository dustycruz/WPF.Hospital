using System;
using System.Collections.Generic;
using System.Linq;
using WPF.Hospital.Model;
using WPF.Hospital.Repository.Interface;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital.Service
{
    public class MedicineService : IMedicineService
    {
        private readonly IMedicineRepository _repository;
        private readonly IPrescriptionRepository _prescriptionRepository;

        public MedicineService(IMedicineRepository repository, IPrescriptionRepository prescriptionRepository)
        {
            _repository = repository;
            _prescriptionRepository = prescriptionRepository;
        }

        public IEnumerable<Model.Medicine> GetAll()
        {
            return _repository.GetAll();
        }

        public Model.Medicine Get(int id)
        {
            return _repository.Get(id);
        }

        public (bool Ok, string Message) Create(Model.Medicine dto)
        {
            // VALIDATION: Name must not be empty
            if (string.IsNullOrWhiteSpace(dto.Name))
                return (false, "Medicine name is required.");

            // VALIDATION: Brand must not be empty
            if (string.IsNullOrWhiteSpace(dto.Brand))
                return (false, "Brand is required.");

            // VALIDATION: Combination of Name + Brand must be unique
            if (DuplicateExists(dto.Name, dto.Brand))
                return (false, $"A medicine with name '{dto.Name}' and brand '{dto.Brand}' already exists.");

            _repository.Add(dto);
            _repository.Save();

            return (true, "Medicine created successfully.");
        }

        public (bool Ok, string Message) Update(Model.Medicine dto)
        {
            // VALIDATION: Medicine must exist
            var existing = _repository.Get(dto.Id);
            if (existing == null)
                return (false, "Medicine not found.");

            // VALIDATION: Name must not be empty
            if (string.IsNullOrWhiteSpace(dto.Name))
                return (false, "Medicine name is required.");

            // VALIDATION: Brand must not be empty
            if (string.IsNullOrWhiteSpace(dto.Brand))
                return (false, "Brand is required.");

            // VALIDATION: Combination of Name + Brand must be unique (exclude current ID)
            if (DuplicateExists(dto.Name, dto.Brand, dto.Id))
                return (false, $"A medicine with name '{dto.Name}' and brand '{dto.Brand}' already exists.");

            existing.Name = dto.Name;
            existing.Brand = dto.Brand;

            _repository.Update(existing);
            _repository.Save();

            return (true, "Medicine updated successfully.");
        }

        public (bool Ok, string Message) Delete(int id)
        {
            // VALIDATION: Medicine must exist
            var medicine = _repository.Get(id);
            if (medicine == null)
                return (false, "Medicine not found.");

            // VALIDATION: Medicine must not be referenced by any Prescription
            if (IsInUse(id))
                return (false, "This medicine cannot be deleted because it is currently prescribed to patients. Please delete the prescriptions first.");

            _repository.Delete(id);
            _repository.Save();

            return (true, "Medicine deleted successfully.");
        }

        // CHECK IF Name + Brand combination already exists
        public bool DuplicateExists(string name, string brand, int excludeId = 0)
        {
            var query = _repository.GetAll()
                .Where(m => m.Name.ToLower() == name.ToLower() && m.Brand.ToLower() == brand.ToLower());

            if (excludeId > 0)
                query = query.Where(m => m.Id != excludeId);

            return query.Any();
        }

        // CHECK IF medicine is used in any prescription
        public bool IsInUse(int medicineId)
        {
            return _prescriptionRepository.GetAll()
                .Any(p => p.MedicineId == medicineId);
        }
    }
}