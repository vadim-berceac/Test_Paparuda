using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator
{
    private static readonly Dictionary<Type, object> Services = new();
    
    public static void Register<T>(T service) where T : class
    {
        var type = typeof(T);
        
        if (Services.ContainsKey(type))
        {
            Debug.LogWarning($"Service {type.Name} уже зарегистрирован! Перезаписываю.");
        }
        
        Services[type] = service;
    }
    
    public static T Get<T>() where T : class
    {
        var type = typeof(T);
        
        if (Services.TryGetValue(type, out var service))
        {
            return service as T;
        }
        
        Debug.LogError($"Service {type.Name} не найден в Service Locator!");
        return null;
    }
    
    public static bool TryGet<T>(out T service) where T : class
    {
        service = Get<T>();
        return service != null;
    }
    
    public static void Unregister<T>() where T : class
    {
        var type = typeof(T);
        if (Services.Remove(type))
        {
            Debug.Log($"Service удален: {type.Name}");
        }
    }
    
    public static void Clear()
    {
        Services.Clear();
        Debug.Log("Все services очищены");
    }
}