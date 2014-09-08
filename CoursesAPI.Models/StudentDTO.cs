using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Models
{   
    /// <summary>
    /// A DTO class for the Student entity class
    /// </summary>
    public class StudentDTO
    {
        /// <summary>
        /// Database-generated ID of the student
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The students Social Security Number, without any delimiters
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// The full name of the student
        /// </summary>
        public string Name { get; set; }
    }
}
