using System.Collections.Generic;
using TeamRoster.Models;

namespace TeamRoster.Services
{
    public interface IService<T> where T : class
    {
        T Add(T objectToAdd);
        List<T> Delete(T objectToRemove);
        List<T> GetAll();
        int GetNextId(List<T> listOfT);
        void Save(List<T> listOfT);
        int TryGetPropertyValue(T obj, string propertyName);
    }
}