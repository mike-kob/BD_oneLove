using System.Collections.Generic;
using BD_oneLove.Models;

namespace BD_oneLove.Tools.DataStorage
{
    internal interface IDataStorage
    {
        bool UserExists(string login, string password);
        User GetUser(string login, string password);

        Class GetClass(string classId);
        List<Student> GetStudents(Class c);
    }
}