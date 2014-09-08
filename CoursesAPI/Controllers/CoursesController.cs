using System.Collections.Generic;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using CoursesAPI.Models;
using CoursesAPI.Services.Services;
using CoursesAPI.Services.DataAccess;

namespace CoursesAPI.Controllers
{
    [RoutePrefix("api/v1/courses")]
    public class CoursesController : ApiController
    {
        private readonly CoursesServiceProvider _service;

        /// <summary>
        /// Constructor
        /// </summary>
        public CoursesController()
        {
            _service = new CoursesServiceProvider( new UnitOfWork<AppDataContext>() );
        }

        [HttpGet]
        [Route("{id:int}/students")]
        public List<StudentDTO> GetStudentsInCourse(int id)
        {
            return _service.GetStudentInCourse(id);
        }

        [HttpPost]
        [Route("{id:int}/students")]
        public HttpResponseMessage AddStudentToCourse(int id, AddStudentViewModel model)
        {
            var responseData = _service.AddStudentToCourse(id, model);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, responseData);

            return response;
            //return _service.AddStudentToCourse(id, model);
        }

        [HttpDelete]
        [Route("{id:int}/students")]
        public HttpResponseMessage RemoveStudentFromCourse(int id, AddStudentViewModel model)
        {
            var responseData = _service.RemoveStudentFromCourse(id, model);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, responseData);

            return response;
        }
        
        [HttpPost]
        [Route("{id:int}/waitinglist")]
        public HttpResponseMessage AddStudentToWaitingList(int id, AddStudentViewModel model)
        {
            var responseData = _service.AddStudentToWaitingList(id, model);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, responseData);

            return response;
        }
    }
}
