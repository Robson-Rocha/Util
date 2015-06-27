using System;

namespace RobsonROX.Util.Required
{
    /// <summary>
    /// Assegura que o tipo de referência T não seja nulo
    /// </summary>
    /// <typeparam name="T">Tipo de referência que se quer assegurar uma instância</typeparam>
    public struct Required<T> : IEquatable<T> where T : class
    {
        private readonly T _value;

        private T Value
        {
            get
            {
                if (_value == null)
                    throw new RequiredNotInitializedException<T>();
                return _value;
            }
        }

        private Required(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            _value = value;
        }

        /// <summary>
        /// Converte de Required{T} para T
        /// </summary>
        /// <param name="value">Instância de Required{T}</param>
        /// <returns>Valor do tipo T</returns>
        public static implicit operator T(Required<T> value)
        {
            return value.Value;
        }

        /// <summary>
        /// Converte de T para Required{T}
        /// </summary>
        /// <param name="value">Valor do tipo T</param>
        /// <returns>Instância de Required{T}</returns>
        public static implicit operator Required<T>(T value)
        {
            return new Required<T>(value);
        }

        /// <summary>
        /// Compara se dois valores do tipo Required{T} são iguais
        /// </summary>
        /// <param name="left">Valor do tipo Required{T}</param>
        /// <param name="right">Valor do tipo Required{T}</param>
        /// <returns>true caso ambos sejam iguais, false senão</returns>
        public static bool operator ==(Required<T> left, Required<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compara se dois valores do tipo Required{T} são diferentes
        /// </summary>
        /// <param name="left">Valor do tipo Required{T}</param>
        /// <param name="right">Valor do tipo Required{T}</param>
        /// <returns>true caso ambos sejam diferentes, false senão</returns>
        public static bool operator !=(Required<T> left, Required<T> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compara se um valor do tipo Required{T} e uma instância de T são iguais
        /// </summary>
        /// <param name="left">Valor do tipo Required{T}</param>
        /// <param name="right">Instância de T</param>
        /// <returns>true caso ambos sejam iguais, false senão</returns>
        public static bool operator ==(Required<T> left, T right)
        {
            if (right == null)
                return false;
            return left.Equals(right);
        }

        /// <summary>
        /// Compara se um valor do tipo Required{T} e uma instância de T são diferentes
        /// </summary>
        /// <param name="left">Valor do tipo Required{T}</param>
        /// <param name="right">Instância de T</param>
        /// <returns>true caso ambos sejam diferentes, false senão</returns>
        public static bool operator !=(Required<T> left, T right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compara se uma instância de T e um valor do tipo Required{T} são iguais
        /// </summary>
        /// <param name="left">Instância de T</param>
        /// <param name="right">Valor do tipo Required{T}</param>
        /// <returns>true caso ambos sejam iguais, false senão</returns>
        public static bool operator ==(T left, Required<T> right)
        {
            if (left == null)
                return false;
            return right.Equals(left);
        }

        /// <summary>
        /// Compara se uma instância de T e um valor do tipo Required{T} são diferentes
        /// </summary>
        /// <param name="left">Instância de T</param>
        /// <param name="right">Valor do tipo Required{T}</param>
        /// <returns>true caso ambos sejam diferentes, false senão</returns>
        public static bool operator !=(T left, Required<T> right)
        {
            return !(left == right);
        }


        private bool ReferenceEquals(object obj)
        {
            if (obj == null)
                return true;

            if (ReferenceEquals(obj, _value))
                return true;
            return false;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false. 
        /// </returns>
        /// <param name="obj">The object to compare with the current instance. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj)) 
                return true;

            if (obj is T)
                return obj.Equals(_value);

            if (obj is Required<T>)
                return ((Required<T>) obj).Value.Equals(_value);

            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(T other)
        {
            if (ReferenceEquals(other))
                return true;

            return other.Equals(_value);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
