using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ShareBill.Domain.Entities
{
    [Table("users")]
    public class Profile : BaseModel
    {
        [PrimaryKey("id",false)]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("username")]
        public string? UserName { get; set; } = default;
    }
}
