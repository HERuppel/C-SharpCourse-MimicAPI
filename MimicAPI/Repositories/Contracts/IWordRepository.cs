using MimicAPI.Helpers;
using MimicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Repositories.Contracts
{
    public interface IWordRepository
    {
        PaginationList<Word> GetAll(WordUrlQuery query);

        Word Get(int id);

        void Create(Word word);

        void Update(Word word);

        void Delete(int id);
    }
}
