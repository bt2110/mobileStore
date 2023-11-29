using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.Data.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public Guid UserId { set; get; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
        public DateTime PublishedDate { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
    }
}
