using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Content.Models.Components;

namespace Content.Controllers
{
    [Route("api/[controller]")]
    public class SchemaController : Controller
    {
        private readonly ComponentRepository _repo;

        public SchemaController(ComponentRepository repo)
        {
            _repo = repo;            
        }

        [HttpGet]
        public ActionResult Get()
        {
           return Ok(_repo.Components.ToDictionary(v => v.Value.ComponentType, v => v.Value));                
        }

        // GET api/values/5
        [HttpGet("{id:guid}")]
        public ActionResult Get(Guid id)
        {
            if (_repo.Components.ContainsKey(id))
                return Ok(_repo.Components[id]);

            return NotFound();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
