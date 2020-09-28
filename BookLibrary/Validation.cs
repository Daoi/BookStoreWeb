using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BookLibrary


{
    //Pass a string to validate and the type of validation to do.
    //Key = Type of validation, value = Regex to match string against
    //Return result of the test
    
    public class Validation
    {
        private static Dictionary<string, Func<string, bool>> regexDict = new Dictionary<string, Func<string, bool>>()
        {
            {"Name", s => Regex.IsMatch(s, @"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$")}, //Two strings seperated by space, second string can have apostrophes/dashes. can start with Title
            {"Address", s => !string.IsNullOrEmpty(s)}, //Not empty string
            {"PhoneNumber", s => Regex.IsMatch(s, @"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}")},//(###) ###-#### or ###-###-####
            {"ID", s => Regex.IsMatch(s, @"^tu[[a-z]\d{5}$")},//tuX#####
            {"ISBN", s => Regex.IsMatch(s, @"^\d{3}(?:-\d{3})$")}, //ISBN
            {"Price", s => Regex.IsMatch(s, @"^[$]?(0|[1-9]\d*)(\.\d+)?$")}, //Us currency format $###.##
            {"Quantity", s => Regex.IsMatch(s, @"^[1-9]{1}[0-9]{0,1}$")} //#(1-9)(optional 0-9) ##
        };

        public static bool Validate(string text, string type)
        {
            Func<string, bool> test = regexDict[type];
            return test(text);
        }
    }
}
