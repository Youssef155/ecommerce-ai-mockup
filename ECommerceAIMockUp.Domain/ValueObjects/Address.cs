using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAIMockUp.Domain.ValueObjects;

public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string Governorate { get; set; }
    public string Zip { get; set; }
}
