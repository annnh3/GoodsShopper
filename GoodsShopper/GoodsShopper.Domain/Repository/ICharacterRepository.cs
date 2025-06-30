using GoodsShopper.Domain.Model;
using System;
using System.Collections.Generic;

namespace GoodsShopper.Domain.Repository
{
    public interface ICharacterRepository
    {
        (Exception exception, Character character) Insert(Character character);

        (Exception ex, Character character) Query(int workId);

        (Exception ex, IEnumerable<Character> characters) GetAll();
    }
}
