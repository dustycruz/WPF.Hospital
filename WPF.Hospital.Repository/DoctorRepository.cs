using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.Model;
using WPF.Hospital.Repository.Interface;

namespace WPF.Hospital.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        public readonly HospitalDbContext _context;
        public DoctorRepository(HospitalDbContext context)
        {
            _context = context;
        }
        public Doctor Get(int id) => _context.Doctor.Find(id);
        public IEnumerable<Doctor> GetAll() => _context.Doctor.ToList();

        public void Add(Doctor entity)
        {
            _context.Doctor.Add(entity);
        }

        public void Delete(int id)
        {
            var doctor = _context.Doctor.Find(id);
            if (doctor != null)
            {
                _context.Doctor.Remove(doctor);
            }
        }

        

        public void Update(Doctor entity)
        {
            _context.Doctor.Update(entity);
        }
        public int Save() => _context.SaveChanges();
    }
}
