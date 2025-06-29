using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAIMockUp.Application.DTOs
{
    public class PromptValidationResult
    {
        public bool IsValid { get; set; }
        public string Error { get; set; }
    }
}
