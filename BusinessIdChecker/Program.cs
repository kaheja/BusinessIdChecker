using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessIdentificationChecker
{
    class Program
    {
        #region Main

        /// <summary>Main program console</summary>
        static void Main(string[] args)
        {
            bool running = true;

            BusinessIdChecker<string> businessIdentifierChecker = new BusinessIdChecker<string>();

            while (running)
            {

                Console.WriteLine("\n\nType business id (enter to exit, 'test' to run the test set): ");
                Console.ForegroundColor = ConsoleColor.White;
                string businessIdentifier = Console.ReadLine();
                Console.ResetColor();

                if (businessIdentifier.ToLower() == "test")
                {
                    // run test set
                    runTestSet(businessIdentifierChecker);
                }
                else if (businessIdentifier.Length != 0)
                {
                    // test identifier input
                    testIdentifier(businessIdentifier, businessIdentifierChecker);
                }
                else
                {
                    // exit
                    return;
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>Test identifier validity</summary>
        private static bool testIdentifier(string bi, BusinessIdChecker<string> checker)
        {
            //BusinessIdChecker<string> businessIdentifierSpecification = new BusinessIdChecker<string>();
            if (checker.IsSatisfiedBy(bi))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Id OK");
                Console.ResetColor();
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Id not OK, reasons: ");
                Console.ResetColor();

                foreach (string reason in checker.ReasonsForDissatisfaction)
                {
                    Console.WriteLine(reason.ToString());
                }
                return false;
            }
        }

        /// <summary>Test set</summary>
        private static void runTestSet(BusinessIdChecker<string> checker)
        {
            Dictionary<string, bool> testData = new Dictionary<string, bool>();

            // too short
            testData.Add("1", false);

            // too long
            testData.Add("24713849XX", false);

            // too short, missing separator
            testData.Add("24713849", false);

            // checknum does not match
            testData.Add("2471385-9", false);

            // checknum does not match
            testData.Add("2471384-8", false);

            // wrong character
            testData.Add("2471A84-9", false);

            // more wrong characters
            testData.Add("a4b1C8D-E", false);

            // check num 1
            testData.Add("2471384-1", false);

            // check num count result 1
            testData.Add("1001000-9", false);

            // ok, real business id
            testData.Add("2471384-9", true);

            // ok, checknum zero
            testData.Add("1572860-0", true);

            foreach (var item in testData)
            {

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\n" + item.Key + ": ");

                if (testIdentifier(item.Key, checker) == item.Value)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("PASSED");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("FAILED");
                    Console.ResetColor();
                }
            }
        }

        #endregion
    }
}
