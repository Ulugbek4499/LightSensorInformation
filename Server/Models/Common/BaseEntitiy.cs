using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Entities.Common;

public class BaseEntitiy
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
}
