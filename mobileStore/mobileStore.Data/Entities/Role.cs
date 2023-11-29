using Microsoft.AspNetCore.Identity;

namespace mobileStore.Data.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
