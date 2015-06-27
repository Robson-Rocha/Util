using System.Collections.Generic;

namespace RobsonROX.Util.Dictionaries
{
    /// <summary>
    /// 
    /// </summary>
    public class StringBidirectionalDictionary : BidirectionalDictionary<string, string>
    {
        /// <summary>
        /// 
        /// </summary>
        public StringBidirectionalDictionary()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstToSecondDictionary"></param>
        public StringBidirectionalDictionary(Dictionary<string, string> firstToSecondDictionary)
            : base(firstToSecondDictionary)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool ExistsInFirst(string value)
        {
            return base.ExistsInFirst(value.ToLowerInvariant());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool ExistsInSecond(string value)
        {
            return base.ExistsInSecond(value.ToLowerInvariant());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string GetFirstValue(string value)
        {
            return base.GetFirstValue(value.ToLowerInvariant());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string GetSecondValue(string value)
        {
            return base.GetSecondValue(value.ToLowerInvariant());
        }
    }
}
