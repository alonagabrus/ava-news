using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaTradeNews.Api.Models
{
    public class EntityBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }

        public EntityBase()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
        }
    }
}