using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Entities
{
    public class BaseEntitiy
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
