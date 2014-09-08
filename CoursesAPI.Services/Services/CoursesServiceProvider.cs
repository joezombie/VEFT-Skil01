using System;
using System.Collections.Generic;
using System.Linq;
using CoursesAPI.Models;
using CoursesAPI.Services.Models.Entities;
using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Services.Extensions;


namespace CoursesAPI.Services.Services
{
    public class CoursesServiceProvider
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Course> _courses;
        private readonly IRepository<Student> _students;
        private readonly IRepository<CourseStudent> _courseStudents;
        private readonly IRepository<CourseWaitingList> _courseWaitingLists;

        public CoursesServiceProvider(IUnitOfWork uow)
        {
            _uow = uow;
            _courses = uow.GetRepository<Course>();
            _students = uow.GetRepository<Student>();
            _courseStudents = uow.GetRepository<CourseStudent>();
            _courseWaitingLists = uow.GetRepository<CourseWaitingList>();
        }

        public List<StudentDTO> GetStudentInCourse(int id)
        {
            var result = (from s in _students.All()
                          join cs in _courseStudents.All() on s.SSN equals cs.SSN
                          where cs.CourseInstanceID == id 
                          && cs.Status == 1
                          select new StudentDTO
                          {
                              Name = s.Name,
                              ID = s.ID,
                              SSN = s.SSN
                          }).ToList();
            return result;
        }

        public StudentDTO AddStudentToCourse(int id, AddStudentViewModel model)
        {
            // 0: operations on courses must use valid course IDs
            var course = _courses.GetCourseByID(id);

            // 1: only registered students can be added to a course
            var student = _students.All().SingleOrDefault(s => s.SSN == model.SSN);
            if (student == null)
            {
                throw new ArgumentException("No student with this SSN exists");
            }

            // 2: students cannot be registered more than once into 
            // the same coures (but they can be re-registered)

            var courseStudent = _courseStudents.All().SingleOrDefault(cs => cs.SSN == model.SSN && cs.CourseInstanceID == id);

            if (courseStudent != null)
            {
                if (courseStudent.Status == 1)
                {
                    throw new ArgumentException("A student cannot be registered more than once in a given course");
                }
                else if (courseStudent.Status == 2)
                {
                    courseStudent.Status = 1;
                    _uow.Save();
                }
            }
            else
            {
                CourseStudent cs = new CourseStudent
                {
                    SSN = student.SSN,
                    CourseInstanceID = course.ID,
                    Status = 1
                };

                _courseStudents.Add(cs);
                _uow.Save();
            }

            return new StudentDTO
            {
                SSN = student.SSN,
                Name = student.Name,
                ID = student.ID
            };

        }

        public StudentDTO RemoveStudentFromCourse(int id, AddStudentViewModel model)
        {
            // 0: operations on courses must use valid course IDs
            var course = _courses.GetCourseByID(id);

            // 1: student must be registered
            var student = _students.All().SingleOrDefault(s => s.SSN == model.SSN);
            if (student == null)
            {
                throw new ArgumentException("No student with this SSN exists");
            }

            var courseStudent = _courseStudents.All().SingleOrDefault(cs => cs.SSN == student.SSN && cs.CourseInstanceID == course.ID);

            if(courseStudent == null)
            {
                throw new ArgumentException("No student with this SSN is registered in given class");
            }
            else 
            {
                courseStudent.Status = 2;
                _uow.Save();
            }

            return new StudentDTO
            {
                SSN = student.SSN,
                Name = student.Name,
                ID = student.ID
            };

        }

        public StudentDTO AddStudentToWaitingList(int id, AddStudentViewModel model)
        {
            // 0: operations on courses must use valid course IDs
            var course = _courses.GetCourseByID(id);

            // 1: only registered students can be added to a course waiting list
            var student = _students.All().SingleOrDefault(s => s.SSN == model.SSN);
            if (student == null)
            {
                throw new ArgumentException("No student with this SSN exists");
            }

            // 2: students cannot be registered more than once into the same course waiting list
            // 3: students cannot be A student cannot be registered to a course and on the course waitinglist

            var courseWaitingList = _courseWaitingLists.All().SingleOrDefault(cw => cw.SSN == model.SSN && cw.CourseInstanceID == id);
            var courseStudent = _courseStudents.All().SingleOrDefault(cs => cs.SSN == model.SSN && cs.CourseInstanceID == id);

            if (courseWaitingList != null)
            {
                throw new ArgumentException("A student cannot be registered more than once in a given course waitinglist");
            }
            else if (courseStudent != null)
            {
                if(courseStudent.Status == 1)
                { 
                    throw new ArgumentException("A student cannot be registered to a course and on the course waitinglist"); 
                } 
            }
            else
            {
                CourseWaitingList cw = new CourseWaitingList
                {
                    SSN = student.SSN,
                    CourseInstanceID = course.ID,
                };

                _courseWaitingLists.Add(cw);
                _uow.Save();
            }

            return new StudentDTO
            {
                SSN = student.SSN,
                Name = student.Name,
                ID = student.ID
            };

        }



        public StudentDTO PopWaitingList(int id)
        {


            // 0: operations on courses must use valid course IDs
            var course = _courses.GetCourseByID(id);

            var topEntry = (from s in _courseWaitingLists.All()
                            orderby s.ID
                            select s).FirstOrDefault();

            // Make sure that the waiting list is not empty
            if(topEntry == null)
            {
                throw new ArgumentException("The waiting list for the given course is empty");
            }


            // 1: only registered students can be added to a course
            var student = _students.All().SingleOrDefault(s => s.SSN == topEntry.SSN);
            if (student == null)
            {
                throw new ArgumentException("No student with this SSN exists");
            }

            // 2: students cannot be registered more than once into 
            // the same coures (but they can be re-registered)

            var courseStudent = _courseStudents.All().SingleOrDefault(cs => cs.SSN == student.SSN && cs.CourseInstanceID == id);

            if (courseStudent != null)
            {
                if (courseStudent.Status == 1)
                {
                    throw new ArgumentException("A student cannot be registered more than once in a given course");
                }
                else if (courseStudent.Status == 2)
                {
                    courseStudent.Status = 1;
                    _uow.Save();
                }
            }
            else
            {
                CourseStudent cs = new CourseStudent
                {
                    SSN = student.SSN,
                    CourseInstanceID = course.ID,
                    Status = 1
                };

                _courseStudents.Add(cs);
                _uow.Save();
            }

            _courseWaitingLists.Delete(topEntry);
            _uow.Save();

            return new StudentDTO
            {
                SSN = student.SSN,
                Name = student.Name,
                ID = student.ID
            };
        }
    }
}
