using System;
using System.Collections.Generic;
using System.Linq;
using WPF.Hospital.DTO;
using WPF.Hospital.Model;
using WPF.Hospital.Repository;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital.Service
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;

        public PatientService(IPatientRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<DTO.Patient> GetAll()
        {
            return _repository.GetAll()
                .Select(p => new DTO.Patient
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Age = p.Age,
                    BirthDate = p.BirthDate,
                    History = new List<DTO.History>() // ✅ ADD
                })
                .ToList();
        }

        public DTO.Patient? Get(int id)
        {
            var p = _repository.Get(id);
            if (p == null) return null;

            return new DTO.Patient
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Age = p.Age,
                BirthDate = p.BirthDate,
                History = new List<DTO.History>() // ✅ ADD
            };
        }

        public (bool Ok, string Message) Create(DTO.Patient dto)
        {
            // ===== VALIDATION =====

            if (string.IsNullOrWhiteSpace(dto.FirstName))
                return (false, "First Name must not be empty.");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                return (false, "Last Name must not be empty.");

            if (dto.Age <= 0)
                return (false, "Age must be greater than 0.");

            if (dto.BirthDate >= DateTime.Today)
                return (false, "Birthdate must be earlier than today.");

            int computedAge = DateTime.Today.Year - dto.BirthDate.Year;
            if (dto.BirthDate.Date > DateTime.Today.AddYears(-computedAge))
                computedAge--;

            if (computedAge != dto.Age)
                return (false, "Age is not consistent with Birthdate.");

            // Duplicate check (use MODEL from repository)
            bool duplicate = _repository.GetAll().Any(p =>
                p.FirstName.ToLower() == dto.FirstName.ToLower() &&
                p.LastName.ToLower() == dto.LastName.ToLower() &&
                p.BirthDate.Date == dto.BirthDate.Date
            );

            if (duplicate)
                return (false, "Duplicate patient entry detected.");

            // ===== MAP DTO → MODEL =====
            var model = new Model.Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Age = dto.Age,
                BirthDate = dto.BirthDate
            };

            _repository.Add(model);
            _repository.Save();

            return (true, "Patient successfully added.");
        }

        public (bool Ok, string Message) Update(DTO.Patient dto)
        {
            var existing = _repository.Get(dto.Id);
            if (existing == null)
                return (false, "Patient not found.");

            existing.FirstName = dto.FirstName;
            existing.LastName = dto.LastName;
            existing.Age = dto.Age;
            existing.BirthDate = dto.BirthDate;

            _repository.Update(existing);
            _repository.Save();

            return (true, "Patient successfully updated.");
        }

        public (bool Ok, string Message) Delete(int id)
        {
            var existing = _repository.Get(id);
            if (existing == null)
                return (false, "Patient not found.");

            _repository.Delete(id);
            _repository.Save();

            return (true, "Patient successfully deleted.");
        }
    }
}