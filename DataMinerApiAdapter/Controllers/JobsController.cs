using DataMinerApiAdapter.Services;
using System.Web.Http;

namespace DataMinerApiAdapter.Controllers
{
    [RoutePrefix("api/jobs")]
    public class JobsController : ApiController
    {
        private readonly JobsService _jobsService;

        public JobsController()
        {
            var adapter = DataMinerAdapterService.Instance; // Improvement: Use decent DI
            _jobsService = new JobsService(adapter);
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllJobs()
        {
            try
            {
                var dto = _jobsService.GetAllJobs();
                return Ok(dto);
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}