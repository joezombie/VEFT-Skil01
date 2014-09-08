using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Services.Models.Entities
{
    public class CourseWaitingList
    {
        /// <summary>
        /// database-generated id for the given record
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The Students SSN
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// database-generated id of the course instance
        /// </summary>
        public int CourseInstanceID { get; set; }
    }
}
