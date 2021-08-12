using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Helpers;
using MimicAPI.Models;
using MimicAPI.Models.DTO;
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
        private readonly IMapper _mapper;

        public WordsController(IWordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
        //[Route("{id}")]
        [HttpGet("{id}", Name = "GetWord")]
        public ActionResult GetById(int id)
        {
            try
            {
                Word word = _repository.Get(id);

                if (word == null)
                    return NotFound();

                WordDTO wordDTO = _mapper.Map<Word, WordDTO>(word);
                wordDTO.Links = new List<LinkDTO>();
                wordDTO.Links.Add(
                    new LinkDTO("self", Url.Link("GetWord", new { id = wordDTO.Id }), "GET")
                );
                wordDTO.Links.Add(
                    new LinkDTO("update", Url.Link("UpdateWord", new { id = wordDTO.Id }), "PUT")
                );
                wordDTO.Links.Add(
                    new LinkDTO("delete", Url.Link("DeleteWord", new { id = wordDTO.Id }), "DELETE")
                );

                return new JsonResult(wordDTO);
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

        //[Route("{id}")]
        [HttpPut("{id}", Name = "UpdateWord")]
        public ActionResult Update(int id, [FromBody]Word word)
        {
            var obj = _repository.Get(id);

            if (obj == null)
                return NotFound();

            word.Id = id;
            _repository.Update(word);

            return new JsonResult("Palavra atualizada!");
        }

        //[Route("{id}")]
        [HttpDelete("{id}", Name = "DeleteWord")]
        public ActionResult Delete(int id)
        {
            Word word = _repository.Get(id);

            if (word == null)
                return NotFound();

            return NoContent();
        }
    }
}
