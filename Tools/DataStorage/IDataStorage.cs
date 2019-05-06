using System.Collections.Generic;
using BD_oneLove.Models;

namespace BD_oneLove.Tools.DataStorage
{
    internal interface IDataStorage
    {
        bool TeacherExists(string tabNum);
        bool UserExists(string login, string password);
        bool UserExistsUseless(string login);
        User GetUser(string login, string password);


        Student SaveStudent(Student s);
        Student UpdateStudent(Student s);
        bool AssignStudentToClass(Student s, Class c);

        Parent GetFather(Student s);
        Parent GetMother(Student s);

        Class GetClass(string classId);
        List<Student> GetStudents(Class c);

        List<string> GetYears();
        List<Teacher> GetTeachers(string year);
        List<Teacher> GetTeachers();
        Teacher UpdateTeacher(Teacher t, Teacher oldT);
        Teacher AddTeacher(Teacher t);
        bool DeleteTeacher(Teacher t);

        List<User> GetUsers();
        void AddUser(User t);

        bool ExpelStudent(Student s, Class c);

    }
}