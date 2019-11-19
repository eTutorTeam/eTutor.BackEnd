using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTutor.ServerApi.Controllers
{
    [Route("api/topics")]
    [ApiController]
    public class TopicsController : EtutorBaseController
    {


        public TopicsController()
        {

        }

        // GET: api/Topics
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Topics/5
        [HttpGet("{id}", Name = "Geter")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Topics
        [HttpPost]
        [Authorize(Roles = "admin")]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Topics/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
