using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Services.Models.Entities
{
    public class CourseStudent
    {
        /// <summary>
        /// The database-generate ID for the given record
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The SSN of the student
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// The database-generated ID of the course instance
        /// </summary>
        public int CourseInstanceID { get; set; }

        /// <summary>
        /// The Status of the student:
        /// 1: registered/active
        /// 2: quit
        /// </summary>
        public int Status { get; set; }
    }
}
