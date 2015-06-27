using System;
using System.Diagnostics;

namespace RobsonROX.Util.Dictionaries
{
    /// <summary>
    /// Indexador baseado em delegates
    /// </summary>
    /// <typeparam name="TKey">Tipo da chave</typeparam>
    /// <typeparam name="TValue">Tipo do valor</typeparam>
    public class DelegatedIndexer<TKey, TValue>
    {
        private readonly Func<TKey, TValue> _getter;
        private readonly Action<TKey, TValue> _setter;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="getter">Delegate a ser executado quando for feita a leitura de itens do indexador</param>
        /// <param name="setter">Delegate a ser executado quando for feita a atribuição de itens do indexador</param>
        [DebuggerStepThrough]
        public DelegatedIndexer(Func<TKey, TValue> getter = null, Action<TKey, TValue> setter = null)
        {
            if(getter == null && setter == null) throw new ArgumentException("Somente um dos delegates pode ser nulo.");
            _getter = getter;
            _setter = setter;
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <returns>
        /// The element with the specified key.
        /// </returns>
        /// <param name="key">The key of the element to get or set.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key"/> is not found.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
        public TValue this[TKey key]
        {
            [DebuggerStepThrough]
            get
            {
                if(_getter == null) throw new InvalidOperationException("O getter não foi atribuído.");
                return _getter(key);
            }
            [DebuggerStepThrough]
            set
            {
                if (_setter == null) throw new InvalidOperationException("O setter não foi atribuído.");
                _setter(key, value);
            }
        }
    }
}