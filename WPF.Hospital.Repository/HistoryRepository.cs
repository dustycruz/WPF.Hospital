using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WPF.Hospital.Model;

namespace WPF.Hospital.Repository
{
    public class HistoryRepository : IHistoryRepository
    {
        public readonly HospitalDbContext _context;
        public HistoryRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public History Get(int id) => _context.History
            .Include(h => h.Patient)
            .Include(h => h.Doctor)
            .FirstOrDefault(h => h.Id == id);

        public IEnumerable<History> GetAll() => _context.History
            .Include(h => h.Patient)
            .Include(h => h.Doctor)
            .ToList();

        public void Add(History entity)
        {
            _context.History.Add(entity);
        }

        public void Delete(int id)
        {
            var history = _context.History.Find(id);
            if (history != null)
            {
                _context.History.Remove(history);
            }
        }

        public void Update(History entity)
        {
            _context.History.Update(entity);
        }

        public int Save() => _context.SaveChanges();

        // ✅ UPDATED - Include Patient and Doctor
        public IEnumerable<History> GetByPatient(int patientId)
        {
            return _context.History
                .Include(h => h.Patient)
                .Include(h => h.Doctor)
                .Where(h => h.PatientId == patientId)
                .ToList();
        }
    }
}