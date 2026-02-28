using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.DTO;
using WPF.Hospital.Repository;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital.Service
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryRepository _repository;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;

        public HistoryService(IHistoryRepository repository, IPatientService patientService, IDoctorService doctorService)
        {
            _repository = repository;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        // GET ALL HISTORIES
        public IEnumerable<DTO.History> GetAll()
        {
            return _repository.GetAll()
                .Select(h => new DTO.History
                {
                    Id = h.Id,
                    Patient = new DTO.Patient { Id = h.PatientId },
                    Doctor = new DTO.Doctor { Id = h.DoctorId },
                    Procedure = h.Procedure
                });
        }

        // GET HISTORY BY ID
        public DTO.History? Get(int id)
        {
            var h = _repository.Get(id);
            if (h == null) return null;

            return new DTO.History
            {
                Id = h.Id,
                Patient = new DTO.Patient { Id = h.PatientId },
                Doctor = new DTO.Doctor { Id = h.DoctorId },
                Procedure = h.Procedure
            };
        }

        // GET BY PATIENT
        public IEnumerable<DTO.History> GetByPatient(int patientId)
        {
            return _repository.GetByPatient(patientId)
                .Select(h => new DTO.History
                {
                    Id = h.Id,
                    Patient = new DTO.Patient
                    {
                        Id = h.Patient.Id,
                        FirstName = h.Patient.FirstName,
                        LastName = h.Patient.LastName
                    },
                    Doctor = new DTO.Doctor
                    {
                        Id = h.Doctor.Id,
                        FirstName = h.Doctor.FirstName,
                        LastName = h.Doctor.LastName
                    },
                    Procedure = h.Procedure
                });
        }

        // CREATE HISTORY
        public (bool Ok, string Message) Create(DTO.History dto)
        {
            if (dto.Patient == null)
                return (false, "A patient must be selected.");

            if (dto.Doctor == null)
                return (false, "A doctor must be selected.");

            if (string.IsNullOrWhiteSpace(dto.Procedure))
                return (false, "Procedure description must not be empty.");

            var patientExists = _patientService.Get(dto.Patient.Id);
            if (patientExists == null)
                return (false, "Selected patient does not exist in the database.");

            var doctorExists = _doctorService.Get(dto.Doctor.Id);
            if (doctorExists == null)
                return (false, "Selected doctor does not exist in the database.");

            var entity = new Model.History
            {
                PatientId = dto.Patient.Id,
                DoctorId = dto.Doctor.Id,
                Procedure = dto.Procedure
            };

            _repository.Add(entity);
            _repository.Save();

            return (true, "Medical history saved successfully.");
        }

        // UPDATE HISTORY
        public (bool Ok, string Message) Update(DTO.History dto)
        {
            var model = _repository.Get(dto.Id);
            if (model == null)
                return (false, "History record not found.");

            if (string.IsNullOrWhiteSpace(dto.Procedure))
                return (false, "Procedure description must not be empty.");

            model.Procedure = dto.Procedure;

            if (dto.Patient != null && dto.Patient.Id > 0)
                model.PatientId = dto.Patient.Id;

            if (dto.Doctor != null && dto.Doctor.Id > 0)
                model.DoctorId = dto.Doctor.Id;

            _repository.Update(model);
            _repository.Save();

            return (true, "Medical history updated successfully.");
        }

        // DELETE HISTORY
        public (bool Ok, string Message) Delete(int id)
        {
            _repository.Delete(id);
            _repository.Save();
            return (true, "Medical history deleted successfully.");
        }

    }
}