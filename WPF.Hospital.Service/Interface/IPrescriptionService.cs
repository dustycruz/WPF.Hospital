using System;
using System.Collections.Generic;
using WPF.Hospital.Model;

namespace WPF.Hospital.Service.Interface
{
    public interface IPrescriptionService : IService<Prescription>
    {
        IEnumerable<Prescription> GetByHistory(int historyId);
        bool DuplicateMedicineExists(int historyId, int medicineId);
    }
}