using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.DTO;
using WPF.Hospital.Repository;
using WPF.Hospital.Repository.Interface;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;

        public DoctorService(IDoctorRepository repository)
        {
            _repository = repository;
        }

        // GET ALL DOCTORS
        public IEnumerable<DTO.Doctor> GetAll()
        {
            return _repository.GetAll()
                .Select(d => new DTO.Doctor
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName
                });
        }

        // GET DOCTOR BY ID
        public DTO.Doctor? Get(int id)
        {
            var doctor = _repository.Get(id);
            if (doctor == null) return null;

            return new DTO.Doctor
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName
            };
        }

        // CREATE DOCTOR
        public (bool Ok, string Message) Create(DTO.Doctor dto)
        {
            // VALIDATION: FirstName must not be empty
            if (string.IsNullOrWhiteSpace(dto.FirstName))
                return (false, "Doctor first name must not be empty.");

            // VALIDATION: LastName must not be empty
            if (string.IsNullOrWhiteSpace(dto.LastName))
                return (false, "Doctor last name must not be empty.");

            // MAP DTO → EF MODEL
            var entity = new Model.Doctor
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            // SAVE TO DATABASE
            _repository.Add(entity);
            _repository.Save();

            return (true, "Doctor created successfully.");
        }

        // UPDATE DOCTOR
        public (bool Ok, string Message) Update(DTO.Doctor dto)
        {
            var doctor = _repository.Get(dto.Id);
            if (doctor == null)
                return (false, "Doctor not found.");

            if (string.IsNullOrWhiteSpace(dto.FirstName))
                return (false, "Doctor first name must not be empty.");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                return (false, "Doctor last name must not be empty.");

            doctor.FirstName = dto.FirstName;
            doctor.LastName = dto.LastName;

            _repository.Update(doctor);
            _repository.Save();

            return (true, "Doctor updated successfully.");
        }

        // DELETE DOCTOR
        // DELETE DOCTOR
        public (bool Ok, string Message) Delete(int id)
        {
            var doctor = _repository.Get(id);
            if (doctor == null)
                return (false, "Doctor not found.");

            // ✅ CHECK IF DOCTOR HAS ASSOCIATED HISTORIES
            var histories = _repository.GetHistoriesByDoctor(id);  // You need to add this method
            if (histories != null && histories.Any())
                return (false, $"Cannot delete doctor with ID {id}. This doctor has {histories.Count()} associated medical histories. Please delete the histories first.");

            _repository.Delete(id);
            _repository.Save();

            return (true, "Doctor deleted successfully.");
        }
    }
}