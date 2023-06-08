using System;
using System.Collections.Generic;
using UnityEngine;

namespace CommonDS
{
    public static class ColorHelper 
    {
        static readonly Dictionary<Color, string> colorNameDict = new Dictionary<Color, string>()
        {
        { Color.black, "Black" },
        { Color.blue, "Blue" },
        { Color.cyan, "Cyan" },
        { Color.gray, "Gray" },
        { Color.green, "Green" },
        { Color.magenta, "Magenta" },
        { Color.red, "Red" },
        { Color.white, "White" },
        { Color.yellow, "Yellow" },
        { Color.clear, "Clear" }
        };

        public static string GetColorName(Color color) 
        {
            return colorNameDict[color];
        }
    }



    public class PersistantVariable<T>
    {
        private ISerializationService serializationService = new UnityJsonSerializationService();

        private readonly string _key = string.Empty;

        private Type intType = typeof(int);

        private Type floatType = typeof(float);

        private Type stringType = typeof(string);

        private T defaultValue;

        public T Value
        {
            get
            {
                Type typeFromHandle = typeof(T);
                if (IsInt(typeFromHandle))
                {
                    return (T)(object)PlayerPrefs.GetInt(_key, (int)(object)defaultValue);
                }

                if (IsFloat(typeFromHandle))
                {
                    return (T)(object)PlayerPrefs.GetFloat(_key, (float)(object)defaultValue);
                }

                if (IsString(typeFromHandle))
                {
                    return (T)(object)PlayerPrefs.GetString(_key, (string)(object)defaultValue);
                }

                if (!HasKey(_key) && defaultValue != null)
                {
                    return defaultValue;
                }

                return serializationService.Deserialize<T>(PlayerPrefs.GetString(_key));
            }
            set
            {
                Type typeFromHandle = typeof(T);
                if (IsInt(typeFromHandle))
                {
                    PlayerPrefs.SetInt(_key, (int)(object)value);
                }
                else if (IsFloat(typeFromHandle))
                {
                    PlayerPrefs.SetFloat(_key, (float)(object)value);
                }
                else if (IsString(typeFromHandle))
                {
                    PlayerPrefs.SetString(_key, (string)(object)value);
                }
                else
                {
                    PlayerPrefs.SetString(_key, serializationService.Serialize(value));
                }
            }
        }

        public PersistantVariable(string key)
        {
            _key = key;
        }

        public PersistantVariable(string key, T defaultValue = default(T), ISerializationService service = null)
        {
            _key = key;
            if (service != null)
            {
                serializationService = service;
            }

            this.defaultValue = defaultValue;
        }

        public bool IsInt(Type t)
        {
            return t == intType;
        }

        public bool IsFloat(Type t)
        {
            return t == floatType;
        }

        public bool IsString(Type t)
        {
            return t == stringType;
        }

        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
    }

}
