using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ShareBill.Domain.Entities
{
    [Table("profiles")]
    public class Profile : BaseModel
    {
        [PrimaryKey("id",false)]
        public Guid Id { get; set; }
        [Column("username")]
        public string? UserName { get; set; } = default;
    }
}
