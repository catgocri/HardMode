using System;
using System.Reflection;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    // Reflection methods by RoboPhred
    public static class Reflection
    {
        public static T GetPrivateField<T>(object instance, string fieldName)
        {
            var prop = instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)prop.GetValue(instance);
        }

        public static void SetPrivateField<T>(object instance, string fieldName, T value)
        {
            var prop = instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            prop.SetValue(instance, value);
        }

        public static TValue GetPrivateStaticField<TType, TValue>(string fieldName)
        {
            var prop = typeof(TType).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            return (TValue)prop.GetValue(null);
        }

        public static void CallPrivateMethod(object instance, string methodName, params object[] args)
        {
            var type = instance.GetType();
            var method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (method == null)
            {
                throw new ArgumentException($"Method {methodName} not found on type {type.FullName}", nameof(methodName));
            }

            method.Invoke(instance, args);
        }
    }
}
