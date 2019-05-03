using System.Collections.Generic;
using BD_oneLove.Models;

namespace BD_oneLove.Tools.DataStorage
{
    internal interface IDataStorage
    {
        bool UserExists(string login, string password);
        bool UserExistsUseless(string login);
        User GetUser(string login, string password);

        Class GetClass(string classId);
        List<Student> GetStudents(Class c);
        List<string> GetYears();
        List<Teacher> GetTeachers(string year);
        List<Teacher> GetTeachers();
        List<User> GetUsers();
        void AddTeacher(Teacher t);
        void AddUser(User t);
    }
}