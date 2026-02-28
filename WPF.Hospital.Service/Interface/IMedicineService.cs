using System;
using System.Collections.Generic;
using WPF.Hospital.Model;

namespace WPF.Hospital.Service.Interface
{
    public interface IMedicineService : IService<Medicine>
    {
        bool DuplicateExists(string name, string brand, int excludeId = 0);
        bool IsInUse(int medicineId);
    }
}