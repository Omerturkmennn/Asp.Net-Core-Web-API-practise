using Entities.LinkModels;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Entities1.LinkModels
{
    public class LinkResponse
    {
        public bool HasLinks { get; set; }
        public List<Entity> ShapedEntities { get; set; }
        public LinkCollectionWrapper<Entity> LinkedEntities { get; set; }

        public LinkResponse()
        {
            ShapedEntities=new List<Entity>();
            LinkedEntities=new LinkCollectionWrapper<Entity>();

        }
    }
}
