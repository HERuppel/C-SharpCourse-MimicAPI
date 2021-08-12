using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Models;
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
        public ActionResult GetAll(DateTime? date)
        {
            var item = _database.Words.AsQueryable();

            if (date.HasValue)
            {
                item = item.Where(a => a.CreatedAt > date.Value || a.UpdatedAt > date.Value);
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
