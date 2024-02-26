namespace LabWeek5.Models;

public class User
{
    public string Email {set; get;}
    public string LastName {set; get;}
    public string FirstName {set; get;}
    

    public User(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public string ToString()
    {
        return $"{FirstName},{LastName},{Email}";
    }
}