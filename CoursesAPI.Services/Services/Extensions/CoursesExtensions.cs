using System;
using System.Linq;
using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Models.Entities;

namespace CoursesAPI.Services.Services.Extensions
{
    public static class CoursesExtensions
    {
        public static Course GetCourseByID(this IRepository<Course> repo, int id)
        {
            // 0: operations on courses must use valid course IDs
            var course = repo.All().SingleOrDefault(c => c.ID == id);
            if (course == null)
            {
                throw new ArgumentException("No course with this ID exists");
            }
            return course;
        }
    }
}
