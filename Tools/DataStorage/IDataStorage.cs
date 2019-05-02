using System.Collections.Generic;
using BD_oneLove.Models;

namespace BD_oneLove.Tools.DataStorage
{
    internal interface IDataStorage
    {
        bool UserExists(string login, string password);
        User GetUser(string login, string password);

        Student SaveStudent(Student s);
        Student UpdateStudent(Student s);
        bool AssignStudentToClass(Student s, Class c);

        Parent GetFather(Student s);
        Parent GetMother(Student s);

        Class GetClass(string classId);
        List<Student> GetStudents(Class c);

        bool ExpelStudent(Student s, Class c);

    }
}