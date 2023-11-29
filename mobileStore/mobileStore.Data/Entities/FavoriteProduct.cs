using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.Data.Entities
{

    public class FavoriteProduct
    {
        public int Id { set; get; }
        public int ProductId { set; get; }
        public Guid UserId { get; set; }
        public Product Product { get; set; }
        public DateTime DateCreated { get; set; }
        public User User { get; set; }

    }
}
