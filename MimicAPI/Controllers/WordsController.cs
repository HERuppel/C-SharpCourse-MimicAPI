using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Helpers;
using MimicAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Controllers
{
    [Route("api/words")]
    public class WordsController : ControllerBase
    {
        private readonly MimicContext _database;

        public WordsController(MimicContext database)
        {
            _database = database;
        }

        [Route("")]
        [HttpGet]
        public ActionResult GetAll([FromQuery]WordUrlQuery query)
        {
            var item = _database.Words.AsQueryable();

            if (query.Date.HasValue)
            {
                item = item.Where(a => a.CreatedAt > query.Date.Value || a.UpdatedAt > query.Date.Value);
            }

            if (query.PageNumber.HasValue)
            {
                int totalRegisterCount = item.Count();
                item = item.Skip((query.PageNumber.Value - 1) * query.PageLimit.Value).Take(query.PageLimit.Value);

                Pagination pagination = new Pagination();
                pagination.pageNumber = query.PageNumber.Value;
                pagination.pageLimit = query.PageLimit.Value;
                pagination.totalRegisters = totalRegisterCount;
                pagination.totalPages = (int) Math.Ceiling((double) totalRegisterCount / query.PageLimit.Value);

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagination));

                if (query.PageNumber > pagination.totalPages)
                {
                    return NotFound();
                }
            }

            return new JsonResult(item);
        }

        // URL/api/words/1
        [Route("{id}")]
        [HttpGet]
        public ActionResult GetById(int id)
        {
            try
            {
                Word word = _database.Words.Find(id);

                if (word == null)
                    return StatusCode(404);

                return new JsonResult(word);
            }
            catch (Exception)
            {
                return StatusCode(404);
            }
            
        }

        [Route("")]
        [HttpPost]
        public ActionResult Create([FromBody]Word word)
        {
            _database.Words.Add(word);
            _database.SaveChanges();

            return new JsonResult("Palavra criada!");
        }

        [Route("{id}")]
        [HttpPut]
        public ActionResult Update(int id, [FromBody]Word word)
        {
            var obj = _database.Words.AsNoTracking().FirstOrDefault(a => a.Id == id);

            if (obj == null)
                return StatusCode(404);

            word.Id = id;
            _database.Words.Update(word);
            _database.SaveChanges();

            return new JsonResult("Palavra atualizada!");
        }

        [Route("{id}")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var word = _database.Words.Find(id);
            word.Active = false;
            _database.Words.Update(word);
            _database.SaveChanges();

            return new JsonResult("Palavra deletada!");
        }
    }
}
