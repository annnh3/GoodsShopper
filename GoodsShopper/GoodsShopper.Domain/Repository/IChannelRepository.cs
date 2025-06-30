using GoodsShopper.Domain.Model;
using System;
using System.Collections.Generic;

namespace GoodsShopper.Domain.Repository
{
    public interface IChannelRepository
    {
        (Exception exception, Channel channel) Insert(Channel channel);

        (Exception ex, Channel channel) Query(int id);

        (Exception ex, IEnumerable<Channel> channels) GetAll();
    }
}
