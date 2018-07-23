using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Core.Models.InteractionTypes
{
    public class Other : InteractionTypeBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.Other;
    }
}
