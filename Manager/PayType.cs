using System.Runtime.Serialization;

namespace Manager
{
    /// <summary>
    /// 
    /// </summary>
    public enum PayType
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMember(Value = "BalancePayment")] BalancePayment,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember(Value = "BonusPayment")] BonusPayment
    }
}