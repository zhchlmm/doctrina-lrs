using Doctrina.Web.Areas.xAPI.Mvc.ModelBinders;
using Doctrina.xAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Doctrina.Web.Areas.xAPI.Models
{
    [ModelBinder(BinderType = typeof(StatementsPostContentModelBinder))]
    public class StatementsPostContent
    {
        [Required]
        public Statement[] Statements { get; set; }
    }
}
