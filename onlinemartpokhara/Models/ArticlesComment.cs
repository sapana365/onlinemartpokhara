using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace onlinemartpokhara.Models
{
    public class ArticlesComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        public string Comments { get; set; }

        public DateTime? ThisDateTime { get; set; }

        public int  ProductsId { get; set; }
        public Products  Products { get; set; }
       

        public int? Rating { get; set; }
    }
}
