using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAIMockUp.Application.Contracts.ImageGenerators
{
    public interface IImageGenerator
    {
        Task<string> ImageGenerator(string prompt);
    }
}
