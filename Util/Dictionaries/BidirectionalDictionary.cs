using System.Collections.Generic;

namespace RobsonROX.Util.Dictionaries
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TFirst"></typeparam>
    /// <typeparam name="TSecond"></typeparam>
    public class BidirectionalDictionary<TFirst, TSecond>
    {
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<TFirst, TSecond> FirstToSecondDictionary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<TSecond, TFirst> SecondToFirstDictionary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BidirectionalDictionary()
        {
            this.FirstToSecondDictionary = new Dictionary<TFirst, TSecond>();
            this.SecondToFirstDictionary = new Dictionary<TSecond, TFirst>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstToSecondDictionary"></param>
        public BidirectionalDictionary(Dictionary<TFirst, TSecond> firstToSecondDictionary)
            : this()
        {
            foreach (TFirst firstValue in firstToSecondDictionary.Keys)
                this.AddValue(firstValue, firstToSecondDictionary[firstValue]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool ExistsInFirst(TFirst value)
        {
            return this.FirstToSecondDictionary.ContainsKey(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool ExistsInSecond(TSecond value)
        {
            return this.SecondToFirstDictionary.ContainsKey(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual TSecond GetSecondValue(TFirst value)
        {
            if (this.ExistsInFirst(value))
                return this.FirstToSecondDictionary[value];
            else
                return default(TSecond);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual TFirst GetFirstValue(TSecond value)
        {
            if (this.ExistsInSecond(value))
                return this.SecondToFirstDictionary[value];
            else
                return default(TFirst);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstValue"></param>
        /// <param name="secondValue"></param>
        public void AddValue(TFirst firstValue, TSecond secondValue)
        {
            this.FirstToSecondDictionary.Add(firstValue, secondValue);
            if (this.SecondToFirstDictionary.ContainsKey(secondValue))
                return;
            this.SecondToFirstDictionary.Add(secondValue, firstValue);
        }
    }
}
