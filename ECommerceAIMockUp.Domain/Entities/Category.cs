using ECommerceAIMockUp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAIMockUp.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }
}
