using System;

namespace RobsonROX.Util.Extensions
{
    /// <summary>
    /// Métodos de extensão para o tipo DateTime
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Retorna a quantidade de segundos restantes até a meia noite
        /// </summary>
        /// <param name="dateTime">Data e hora a partir da qual se deseja obter o número de segundos até a meia noite</param>
        /// <returns>Quantidade de segundos restantes até a meia noite</returns>
        public static int SecondsToMidnight(this DateTime dateTime)
        {
            return Convert.ToInt32(86400 - dateTime.TimeOfDay.TotalMinutes);
        }

        /// <summary>
        /// Retorna a quantidade de minutos restantes até a meia noite
        /// </summary>
        /// <param name="dateTime">Data e hora a partir da qual se deseja obter o número de minutos até a meia noite</param>
        /// <returns>Quantidade de minutos restantes até a meia noite</returns>
        public static int MinutesToMidnight(this DateTime dateTime)
        {
            return Convert.ToInt32(1440 - dateTime.TimeOfDay.TotalMinutes);
        }

        /// <summary>
        /// Obtém a idade de um indivíduo em relação à data atual
        /// </summary>
        /// <param name="birthday">Data de nascimento do indivíduo</param>
        /// <returns>Idade do indivíduo, em anos</returns>
        public static int GetAge(this DateTime birthday)
        {
            return GetAge(birthday, DateTime.Today);
        }

        /// <summary>
        /// Obtém a idade de um indivíduo em relação à data especificada
        /// </summary>
        /// <param name="birthday">Data de nascimento do indivíduo</param>
        /// <param name="today">Data relativa para cálculo da idade do indivíduo</param>
        /// <returns>Idade do indivíduo, em anos</returns>
        public static int GetAge(this DateTime birthday, DateTime today)
        {
            int age = today.Year - birthday.Year;
            if (birthday > today.AddYears(-age)) age--;
            return age;
        }
    }
}
