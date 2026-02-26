using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.Model;
using WPF.Hospital.Repository.Interface;

namespace WPF.Hospital.Repository
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        public readonly HospitalDbContext _context;
        public PrescriptionRepository(HospitalDbContext context) 
        {
            _context = context;
        }
        public Prescription Get(int id) => _context.Prescription.Find(id);
        public IEnumerable<Prescription> GetAll() => _context.Prescription.ToList();

        public void Add(Prescription entity)
        {
            _context.Prescription.Add(entity);
        }

        public void Delete(int id)
        {
            var prescription = _context.Prescription.Find(id);
            if (prescription != null)
            {
                _context.Prescription.Remove(prescription);
            }
        }

        public void Update(Prescription entity)
        {
            _context.Prescription.Update(entity);
        }
        public int Save() => _context.SaveChanges();

        public IEnumerable<Prescription> GetByHistory(int historyId)
        {
            return _context.Prescription.Where(h => h.HistoryId == historyId).ToList();
        }
    }
}
