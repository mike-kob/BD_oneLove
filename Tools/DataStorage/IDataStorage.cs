using System.Collections.Generic;
using System.Security.Policy;
using BD_oneLove.Models;

namespace BD_oneLove.Tools.DataStorage
{
    internal interface IDataStorage
    {

        //--------------Subjects-----------------
        List<ClassSubject> GetSubjectsStatistics(Class c, string type);


        //--------------Plans-----------------

        List<Plan> GetPlans();
        Plan GetCurrentPlan(string year);
        bool AddPlan(Plan p);
        bool UpdatePlan(Plan p, Plan oldP);
        bool DeletePlan(Plan p);
        string GetCurYear();
        bool UpdateCurYear(string year);

        //--------------Users-----------------

        bool DirectorExists();
        bool UserExists(string login, string password);
    //    bool UserExistsUseless(string login);
        User GetUser(string login, string password);
        List<User> GetUsers(string access);
        bool AddUser(User t);
        bool DeleteUser(User t);
        bool UpdateUser(User t, User oldT);

        //--------------Students---------------
        Student SaveStudent(Student s);
        Student UpdateStudent(Student s);
        Student GetStudent(string id);
        List<Student> GetStudents(Class c);
        List<Student> GetStudents();
        List<Student> GetFreeStudents(Class c);
        List<Student> GetFreeStudents();
        bool AssignStudentToClass(Student s, Class c);
        bool ExpelStudent(Student s, Class c);
        List<Student> GetStudentsStatistics(Class c, string type);

        //--------------Parents---------------
        Parent GetFather(Student s);
        Parent GetMother(Student s);
        List<Parent> GetTrustees(Student s);

        List<ParentChild> GetParentChildren(Student s);
        List<ParentChild> GetParentChildren(Parent p);
        Parent ParentExists(Parent p);
        
        List<Parent> GetAllParents();
        List<Parent> GetParentsInClass(Class c);

        Parent SaveParent(Parent p);
        bool SaveParentChild(ParentChild pc);
        bool RemoveParentChild(ParentChild pc);

        //--------------Mobile phones----------
        List<string> GetMobileNumbers(Parent p);
        List<string> GetMobileNumbers(Student s);

        bool AddMobileNumber(Parent p, string num);
        bool AddMobileNumber(Student s, string num);

        bool RemoveMobileNumber(Parent p, string num);
        bool RemoveMobileNumber(Student s, string num);

        //--------------Classes---------------
        Class GetClass(string classId);
        Class GetCurrentClass(User u);
        List<Class> GetClasses(string year);
        List<Class> GetClasses(Student s);
        List<Class> GetClassesStatistics(string year,string type);


        bool AddClass(Class c);
        bool UpdateClass(Class c);
        bool DeleteClass(Class c);


        //--------------Teachers--------------
        List<string> GetYears();
        List<Teacher> GetTeachers(string year);
        List<Teacher> GetTeachers(Class c);
    //    List<Teacher> GetTeachers();
        Teacher UpdateTeacher(User t, User oldT);
        Teacher AddTeacher(User t);
        bool DeleteTeacher(User t);
        bool TeacherExists(string tabNum);
        Teacher GetTeacher(string id);
        bool AddTeacherClass(Teacher t, Class c);
        bool DeleteTeacherClass(Teacher t, Class c);


        //-------------Marks-------------------
        bool AddSubject(Class c, string subject);
        bool RemoveSubject(Class c, string subject);
        List<string> GetSubjects(Class c, string type);
        List<Mark> GetMarks(Class c, string subject, string type);
        List<StudentSubject> GetMarks(Student s, Class c);
        List<Mark> SaveMarks(List<Mark> l);
        List<string> GetSubjects(Class c);
        bool RemoveMarks(List<Mark> l);

        //-------------Comments--------------
        List<Comment> GetComments(Student s);
        List<string> GetAllComments();
        List<Comment> SaveComments(Student s);
        bool RemoveComments(Comment c);
        List<Comment> SaveComments(List<Comment> l);

        //------------Movement---------------
        List<Movement> GetMovements(Class c);
        List<Movement> GetMovements();
        List<Movement> SaveMovements(List<Movement> l);
        bool RemoveMovement(Movement m);

   
    }
}