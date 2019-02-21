using Doctrina.xAPI.LRS.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Doctrina.xAPI.LRS.Models
{
    [ModelBinder(BinderType = typeof(StatementsPostModelBinder))]
    public class StatementsPostContent
    {
        [Required]
        public Statement[] Statements { get; internal set; }
    }
}
