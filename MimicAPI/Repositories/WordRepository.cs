using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Helpers;
using MimicAPI.Models;
using MimicAPI.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Repositories
{
    public class WordRepository : IWordRepository
    {
        private readonly MimicContext _database;

        public WordRepository(MimicContext database)
        {
            _database = database;
        }

        public PaginationList<Word> GetAll(WordUrlQuery query)
        {
            PaginationList<Word> list = new PaginationList<Word>();
            var item = _database.Words.AsNoTracking().AsQueryable();

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
                pagination.totalPages = (int)Math.Ceiling((double)totalRegisterCount / query.PageLimit.Value);

                list.Pagination = pagination;
            }
            list.AddRange(item.ToList());

            return list;
        }

        public Word Get(int id)
        {
            return _database.Words.AsNoTracking().FirstOrDefault(a => a.Id == id);
        }

        public void Create(Word word)
        {
            _database.Words.Add(word);
            _database.SaveChanges();
        }

        public void Update(Word word)
        {
            _database.Words.Update(word);
            _database.SaveChanges();
        }

        public void Delete(int id)
        {
            Word word = Get(id);

            word.Active = false;
            _database.Words.Update(word);
            _database.SaveChanges();
        }
    }
}
