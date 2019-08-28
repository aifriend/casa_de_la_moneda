using System;
using System.Collections.Generic;
using System.Reflection;

namespace Idpsa.Control.Tool
{
    public static class ReflexionHelper
    {
        public static void FindFields(ref ICollection<FieldInfo> fields, Type t)
        {
            if (fields == null)
            {
                fields = new List<FieldInfo>();
            }
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            foreach (FieldInfo field in t.GetFields(flags))
            {
                // Ignore inherited fields.
                if (field.DeclaringType == t)
                    fields.Add(field);
            }

            Type baseType = t.BaseType;
            if (baseType != null)
                FindFields(ref fields, baseType);
        }
    }
}