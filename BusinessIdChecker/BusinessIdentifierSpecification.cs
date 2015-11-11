using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessIdentificationChecker
{
    /// <summary>
    /// Class to check the business id validity and possible reasons for invalidity. Implements interface ISpecification.
    /// </summary>
    class BusinessIdentifierSpecification<TEntity> : ISpecification<TEntity>
    {
        #region Fields

        private List<string> reasons = new List<string>();

        private enum Reason
        {
            [Description("Checknum does not match identifier base")]
            CheckNumMismatch,
            [Description("Error in identifier base - results checknum 1")]
            CheckNumBaseInvalid,
            [Description("Invalid character ")]
            InvalidCharacter,
            [Description("Invalid format - not string")]
            InvalidFormat,
            [Description("Invalid separator character")]
            InvalidSeparator,
            [Description("Too short identifier string")]
            TooShort,
            [Description("Too long identifier string")]
            TooLong
        }

        #endregion

        #region Properties

        /// <summary>
        /// Read reasons for business id dissatisfactions.
        /// </summary>
        /// <returns>
        /// Returns IEnumerable string list
        /// </returns>
        public IEnumerable<string> ReasonsForDissatisfaction
        {
            get
            {
                return reasons;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Method to check the satisfaction conditions
        /// </summary>
        /// <param name="entity">
        /// Parameter entity requires a TEntity argument. Expected type string.
        /// </param>
        /// <returns>
        /// The method returns a boolean.
        /// </returns>
        public bool IsSatisfiedBy(TEntity entity)
        {
            string identifier = null;
            
            reasons.Clear();

            if (checkType(entity))
            {
                // input type string
                identifier = entity.ToString();

                // basic string content checks
                checkMinLength(identifier);
                checkMaxLength(identifier);
                checkSeparator(identifier);
                checkCharacters(identifier);

                // no reason to count checknum values if other dissatisfaction reasons found
                if (reasons.Count == 0)
                {
                    checkCheckNum(identifier);
                }
            }

            // if no dissatisfaction reasons, identification string is valid
            return (reasons.Count == 0);
        }

        #endregion

        #region Private methods

        /// <summary>Check entity is string type</summary>
        private bool checkType(TEntity entity)
        {
            if (entity.GetType() != typeof(string))
            {
                reasons.Add(Enum.GetName(typeof(Reason), Reason.InvalidFormat));
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>Check string length is mimimun 9 characters</summary>
        private bool checkMinLength(string bi)
        {
            bool retVal = true;

            if (bi.Length < 9)
            {
                reasons.Add(Enum.GetName(typeof(Reason), Reason.TooShort));
                retVal = false;
            }

            return retVal;
        }

        /// <summary>Check string length is maximum 9 characters</summary>
        private bool checkMaxLength(string bi)
        {
            bool retVal = true;

            if (bi.Length > 9)
            {
                reasons.Add(Enum.GetName(typeof(Reason), Reason.TooLong));
                retVal = false;
            }

            return retVal;
        }

        /// <summary>Check that character 7 is separator line '-'</summary>
        private bool checkSeparator(string bi)
        {
            bool retVal = true;

            if (bi.Length < 8 || bi[7] != '-')
            {
                reasons.Add(Enum.GetName(typeof(Reason), Reason.InvalidSeparator));
                retVal = false;
            }

            return retVal;
        }

        /// <summary>Check that characters 0-6 and 8 are numeral</summary>
        private bool checkCharacters(string bi)
        {
            bool retVal = true;

            for (int i = 0; i < bi.Length; i++)
            {
                if (i != 7)
                {
                    try
                    {
                        int.Parse(bi.Substring(i, 1));
                    }
                    catch (FormatException)
                    {
                        reasons.Add(Enum.GetName(typeof(Reason), Reason.InvalidCharacter) + " " + bi.Substring(i, 1));
                        retVal = false;
                    }
                }
            }

            return retVal;
        }
        /*
        /// <summary>Check that character 8 is not 1</summary>
        private bool checkCheckNumOne(string bi)
        {
            bool retVal = true;

            try
            {
                if (int.Parse(bi.Substring(8, 1)) == 1)
                {
                    reasons.Add(Enum.GetName(typeof(Reason), Reason.CheckNumOne));
                    retVal = false;
                }
            }
            catch (Exception)
            {
                // Either fewer characters or not integer. Not 1.
            }

            return retVal;
        }*/

        /// <summary>Check that character 8 (check num) matches rest of identification</summary>
        private bool checkCheckNum(string bi)
        {
            bool retVal = true;
            int checkNumTotal = 0;

            for (int i = 0; i < 7; i++)
            {
                int num = int.Parse(bi.Substring(i, 1));

                switch (i)
                {
                    case 6:
                        checkNumTotal += 2 * num;
                        break;
                    case 5:
                        checkNumTotal += 4 * num;
                        break;
                    case 4:
                        checkNumTotal += 8 * num;
                        break;
                    case 3:
                        checkNumTotal += 5 * num;
                        break;
                    case 2:
                        checkNumTotal += 10 * num;
                        break;
                    case 1:
                        checkNumTotal += 9 * num;
                        break;
                    case 0:
                        checkNumTotal += 7 * num;
                        break;
                }
            }

            int checkNumReal = checkNumTotal % 11;

            if (checkNumReal == 1)
            {
                reasons.Add(Enum.GetName(typeof(Reason), Reason.CheckNumBaseInvalid));
                retVal = false;
            }
            else if (checkNumReal > 1)
            {
                checkNumReal = 11 - checkNumReal;
            }

            if (checkNumReal != int.Parse(bi.Substring(8, 1)))
            {
                reasons.Add(Enum.GetName(typeof(Reason), Reason.CheckNumMismatch));
                retVal = false;
            }

            return retVal;
        }

        #endregion
    }
}
