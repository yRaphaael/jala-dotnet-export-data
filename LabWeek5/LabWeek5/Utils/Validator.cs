using System.Text.RegularExpressions;

namespace LabWeek5.Utils;

public class Validator
{
    public static bool ValidatorName(string name)
    {
        string pattern = @"^[a-zA-Z]{3,16}$";
        Regex regex = new(pattern);

        return regex.IsMatch(name);
    }
    
    public static bool ValidateEmail(string email) {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new(pattern);
        
        return regex.IsMatch(email);
    }
}