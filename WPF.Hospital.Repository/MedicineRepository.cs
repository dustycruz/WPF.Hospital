using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.Model;
using WPF.Hospital.Repository.Interface;

namespace WPF.Hospital.Repository
{
    public class MedicineRepository : IMedicineRepository
    {
        public readonly HospitalDbContext _context;
        public MedicineRepository(HospitalDbContext context)
        {
            _context = context;
        }
        public Medicine Get(int id) => _context.Medicine.Find(id);
        public IEnumerable<Medicine> GetAll() => _context.Medicine.ToList();

        public void Add(Medicine entity)
        {
            _context.Medicine.Add(entity);
        }

        public void Delete(int id)
        {
            var medicine = _context.Medicine.Find(id);
            if (medicine != null)
            {
                _context.Medicine.Remove(medicine);
            }
        }

        public void Update(Medicine entity)
        {
            _context.Medicine.Update(entity);
        }
        public int Save() => _context.SaveChanges();


    }
}
