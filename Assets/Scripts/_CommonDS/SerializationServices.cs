using System.Collections.Generic;
using UnityEngine;

namespace CommonDS
{
    
    public interface ISerializationService
    {
        string Serialize<T>(T obj);

        T Deserialize<T>(string json);
    }


    public class UnityJsonSerializationService : ISerializationService
    {
        public T Deserialize<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        public string Serialize<T>(T obj)
        {
            return JsonUtility.ToJson(obj);
        }
    }
}
