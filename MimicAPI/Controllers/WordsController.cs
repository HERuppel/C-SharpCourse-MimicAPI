using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Helpers;
using MimicAPI.Models;
using MimicAPI.Repositories.Contracts;
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
        private readonly IWordRepository _repository;

        public WordsController(IWordRepository repository)
        {
            _repository = repository;
        }

        [Route("")]
        [HttpGet]
        public ActionResult GetAll([FromQuery]WordUrlQuery query)
        {
            var items = _repository.GetAll(query);
            if (query.PageNumber > items.Pagination.totalPages)
            {
                return NotFound();
            }

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(items.Pagination));

            return new JsonResult(items.ToList());
        }

        // URL/api/words/1
        [Route("{id}")]
        [HttpGet]
        public ActionResult GetById(int id)
        {
            try
            {
                Word word = _repository.Get(id);

                if (word == null)
                    return NotFound();

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
            _repository.Create(word);

            return Created($"/api/words/{word.Id}", word);
        }

        [Route("{id}")]
        [HttpPut]
        public ActionResult Update(int id, [FromBody]Word word)
        {
            var obj = _repository.Get(id);

            if (obj == null)
                return NotFound();

            word.Id = id;
            _repository.Update(word);

            return new JsonResult("Palavra atualizada!");
        }

        [Route("{id}")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Word word = _repository.Get(id);

            if (word == null)
                return NotFound();

            return NoContent();
        }
    }
}
