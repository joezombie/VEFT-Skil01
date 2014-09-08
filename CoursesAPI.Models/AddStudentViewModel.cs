using System.ComponentModel.DataAnnotations;

namespace CoursesAPI.Models
{
    /// <summary>
    /// This class represents the data which are needed as input when
    /// a student is added to a course
    /// </summary>
    public class AddStudentViewModel
    {
        [Required]
        public string SSN { get; set; }
    }
}
