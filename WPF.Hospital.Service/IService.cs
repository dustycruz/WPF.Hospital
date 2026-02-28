using System;
using System.Collections.Generic;

namespace WPF.Hospital.Service
{
    public interface IService<T>
    {
        IEnumerable<T> GetAll();
        T? Get(int id);
        (bool Ok, string Message) Create(T entity);
        (bool Ok, string Message) Update(T entity);
        (bool Ok, string Message) Delete(int id);
    }
}