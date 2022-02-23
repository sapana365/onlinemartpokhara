using onlinemartpokhara.Models;
using System.ComponentModel.DataAnnotations;

namespace onlinemartpokhara.ViewModels
{
    public class ArticleCommentViewModel
    {
        [Key]
        public string Title { get; set; }
        public List <ArticlesComment>ListsOfComments { get; set; }
        public string Comment{ get; set; }
        public int ProductsId{ get; set; }
        public int Rating { get; set; }
    }
}
