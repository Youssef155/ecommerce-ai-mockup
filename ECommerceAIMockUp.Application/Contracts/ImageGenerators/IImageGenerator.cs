using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Wrappers;

namespace ECommerceAIMockUp.Application.Contracts.ImageGenerators
{
    public interface IImageGenerator
    {
        Task<Response<string>> ImageGenerator(string prompt);
    }
}
