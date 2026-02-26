using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.DTO;

namespace WPF.Hospital.Service.Interface
{
    public interface IHistoryService : IService<History>
    {
        IEnumerable<History> GetByPatient(int patientId);
    }
}
