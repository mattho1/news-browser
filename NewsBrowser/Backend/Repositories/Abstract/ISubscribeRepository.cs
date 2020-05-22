using Backend.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Repositories.Abstract
{
    public interface ISubscribeRepository
    {
        Subscriber Get(string email);
        List<Subscriber> GetAllSubscribers();
        void Add(Subscriber subscriber);
        void Remove(string id);
    }
}
