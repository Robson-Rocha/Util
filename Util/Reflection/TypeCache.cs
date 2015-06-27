using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using RobsonROX.Util.Extensions;

namespace RobsonROX.Util.Reflection
{
    /// <summary>
    /// Mantém em cache as informações do tipo especificado
    /// </summary>
    /// <typeparam name="T">Tipo cujas informações devem ser mantidas em cache</typeparam>
    public static class TypeCache<T>
    {
        // ReSharper disable StaticFieldInGenericType
        // ReSharper disable once InconsistentNaming
        private static Type _type;
        private static ReadOnlyCollection<Attribute> _attributes;
        private static ReadOnlyDictionary<string, FieldInfo> _fields;
        private static ReadOnlyDictionary<string, ReadOnlyCollection<Attribute>> _fieldsAttributes;
        private static ReadOnlyDictionary<string, PropertyInfo> _properties;
        private static ReadOnlyDictionary<string, ReadOnlyCollection<Attribute>> _propertiesAttributes;
        private static ReadOnlyCollection<ConstructorInfo> _constructors;
        private static ReadOnlyDictionary<string, ReadOnlyCollection<Attribute>> _constructorsAttributes;



        /// <summary>
        /// Obtém as informações do tipo
        /// </summary>
        public static Type Type
        {
            [DebuggerStepThrough]
            get
            {
                return _type ?? (_type = typeof(T));
            }
        }

        /// <summary>
        /// Obtém os atributos do tipo
        /// </summary>
        public static ReadOnlyCollection<Attribute> Attributes
        {
            [DebuggerStepThrough]
            get
            {
                return _attributes ?? (_attributes = new ReadOnlyCollection<Attribute>(Type.GetCustomAttributes(true).Cast<Attribute>().ToList()));
            }
        }

        /// <summary>
        /// Obtém os campos do tipo
        /// </summary>
        public static ReadOnlyDictionary<string, FieldInfo> Fields
        {
            [DebuggerStepThrough]
            get
            {
                return _fields ?? (_fields = new ReadOnlyDictionary<string, FieldInfo>(Type.GetFields().ToDictionary(fi => fi.Name)));
            }
        }

        /// <summary>
        /// Obtém os atributos dos campos do tipo
        /// </summary>
        public static ReadOnlyDictionary<string, ReadOnlyCollection<Attribute>> FieldsAttributes
        {
            [DebuggerStepThrough]
            get
            {
                return _fieldsAttributes ?? (_fieldsAttributes = new ReadOnlyDictionary<string, ReadOnlyCollection<Attribute>>(Fields.Values.ToDictionary(fi => fi.Name, fv => new ReadOnlyCollection<Attribute>(fv.GetCustomAttributes(true).Cast<Attribute>().ToList()))));
            }
        }

        /// <summary>
        /// Obtém as propriedades do tipo
        /// </summary>
        public static ReadOnlyDictionary<string, PropertyInfo> Properties
        {
            [DebuggerStepThrough]
            get
            {
                return _properties ?? (_properties = new ReadOnlyDictionary<string, PropertyInfo>(Type.GetProperties().ToDictionary(pi => pi.Name)));
            }
        }

        /// <summary>
        /// Obtém os atributos das propriedades do tipo
        /// </summary>
        public static ReadOnlyDictionary<string, ReadOnlyCollection<Attribute>> PropertiesAttributes
        {
            [DebuggerStepThrough]
            get
            {
                return _propertiesAttributes ?? (_propertiesAttributes = new ReadOnlyDictionary<string, ReadOnlyCollection<Attribute>>(Properties.Values.ToDictionary(pi => pi.Name, pv => new ReadOnlyCollection<Attribute>(pv.GetCustomAttributes(true).Cast<Attribute>().ToList()))));
            }
        }

        /// <summary>
        /// Retorna todos os construtores - públicos ou não públicos - do tipo
        /// </summary>
        public static ReadOnlyCollection<ConstructorInfo> Constructors
        {
            [DebuggerStepThrough]
            get
            {
                return _constructors ?? (_constructors = new ReadOnlyCollection<ConstructorInfo>(Type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)));
            }
        }

        /// <summary>
        /// Obtém os atributos dos construtores
        /// </summary>
        public static ReadOnlyDictionary<string, ReadOnlyCollection<Attribute>> ConstructorsAttributes
        {
            [DebuggerStepThrough]
            get
            {
                return _constructorsAttributes ?? (_constructorsAttributes = new ReadOnlyDictionary<string, ReadOnlyCollection<Attribute>>(Constructors.ToDictionary(ci => ci.Name, cv => new ReadOnlyCollection<Attribute>(cv.GetCustomAttributes(true).Cast<Attribute>().ToList()))));
            }
        }
    }

    /// <summary>
    /// Mantém em cache informações de tipos
    /// </summary>
    public static class TypeCache
    {
        // ReSharper disable once StaticFieldInGenericType
        private static IDictionary<string, Type> _types;

        /// <summary>
        /// Retorna as informações do tipo, as obtendo caso ainda não disponíveis
        /// </summary>
        /// <param name="typeName">Chave que identifica o nome do tipo.</param>
        /// <param name="typeFunc">Expressão que retorna as informações do tipo para serem armazenadas em cache</param>
        /// <returns>Informações do tipo identificado pela chave fornecida</returns>
        [DebuggerStepThrough]
        public static Type Type(string typeName, Func<Type> typeFunc)
        {
            return (_types ?? (_types = new Dictionary<string, Type>())).GetOrAdd(typeName, typeFunc);
        }
    }

}
