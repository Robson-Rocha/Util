using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RobsonROX.Util.Extensions
{
    // ReSharper disable once InconsistentNaming

    /// <summary>
    /// Define métodos de extensão para dicionários
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Permite obter um valor do dicionário, ou, caso não exista item com a chave especificada, inserir esse objeto
        /// </summary>
        /// <param name="source">Dicionário de onde se pretende obter ou adicionar o valor</param>
        /// <param name="key">Chave que identifica o item dentro do dicionário</param>
        /// <param name="valueFunc">Método que retorna o valor a ser inserido no dicionário em caso da chave especificada não ter sido encontrada.</param>
        /// <typeparam name="TKey">Tipo da chave</typeparam>
        /// <typeparam name="TValue">Tipo do valor</typeparam>
        /// <returns>Item representado pela chave, ou recém inserido</returns>
        [DebuggerStepThrough]
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TValue> valueFunc)
        {
            if (source.ContainsKey(key)) return source[key];
            TValue value = valueFunc();
            source.Add(key, value);
            return value;
        }

        /// <summary>
        /// Permite obter um valor do dicionário, ou, caso não exista item com a chave especificada, inserir esse objeto
        /// </summary>
        /// <param name="source">Dicionário de onde se pretende obter ou adicionar o valor</param>
        /// <param name="key">Chave que identifica o item dentro do dicionário</param>
        /// <param name="valueFunc">Método que retorna o valor a ser inserido no dicionário em caso da chave especificada não ter sido encontrada.</param>
        /// <returns>Item representado pela chave, ou recém inserido</returns>
        [DebuggerStepThrough]
        public static object GetOrAdd(this IDictionary source, object key, Func<object> valueFunc)
        {
            if (source.Contains(key)) return source[key];
            object value = valueFunc();
            source.Add(key, value);
            return value;
        }

        /// <summary>
        /// Permite atualizar um valor no dicionário, ou, caso não exista item com a chave especificada, inserir esse objeto
        /// </summary>
        /// <param name="source">Dicionário de onde se pretende atualizar ou adicionar o valor</param>
        /// <param name="key">Chave que identifica o item dentro do dicionário</param>
        /// <param name="valueFunc">Método que retorna o valor a ser inserido no dicionário em caso da chave especificada não ter sido encontrada.</param>
        /// <typeparam name="TKey">Tipo da chave</typeparam>
        /// <typeparam name="TValue">Tipo do valor</typeparam>
        [DebuggerStepThrough]
        public static void UpdateOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key,
            Func<TValue> valueFunc)
        {
            if (!source.ContainsKey(key))
                source.Add(key, valueFunc());
            else
                source[key] = valueFunc();
        }

        /// <summary>
        /// Permite atualizar um valor no dicionário, ou, caso não exista item com a chave especificada, inserir esse objeto
        /// </summary>
        /// <param name="source">Dicionário de onde se pretende atualizar ou adicionar o valor</param>
        /// <param name="key">Chave que identifica o item dentro do dicionário</param>
        /// <param name="valueFunc">Método que retorna o valor a ser inserido no dicionário em caso da chave especificada não ter sido encontrada.</param>
        [DebuggerStepThrough]
        public static void UpdateOrAdd(this IDictionary source, object key, Func<object> valueFunc)
        {
            if (!source.Contains(key))
                source.Add(key, valueFunc());
            else
                source[key] = valueFunc();
        }


        /// <summary>
        /// Permite inserir um novo valor no dicionário caso sua chave especificada não exista previamente
        /// </summary>
        /// <param name="source">Dicionário de onde se pretende adicionar condicionalmente o valor</param>
        /// <param name="key">Chave que identifica o item dentro do dicionário</param>
        /// <param name="valueFunc">Método que retorna o valor a ser inserido no dicionário em caso da chave especificada não ter sido encontrada.</param>
        /// <typeparam name="TKey">Tipo da chave</typeparam>
        /// <typeparam name="TValue">Tipo do valor</typeparam>
        [DebuggerStepThrough]
        public static void AddIfNotExists<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TValue> valueFunc)
        {
            if (!source.ContainsKey(key))
                source.Add(key, valueFunc());
        }


        /// <summary>
        /// Permite inserir um novo valor no dicionário caso sua chave especificada não exista previamente
        /// </summary>
        /// <param name="source">Dicionário de onde se pretende adicionar condicionalmente o valor</param>
        /// <param name="key">Chave que identifica o item dentro do dicionário</param>
        /// <param name="valueFunc">Método que retorna o valor a ser inserido no dicionário em caso da chave especificada não ter sido encontrada.</param>
        [DebuggerStepThrough]
        public static void AddIfNotExists(this IDictionary source, object key, Func<object> valueFunc)
        {
            if (!source.Contains(key))
                source.Add(key, valueFunc());
        }
    }
}
