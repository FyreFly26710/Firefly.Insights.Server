using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Common.Configurations;
public class JwtSettings
{
    public string Key { get; set; } = "ThisIsASuperLongSecretKey1234567890!";
    public string Issuer { get; set; } = "default_issuer";
    public string Audience { get; set; } = "default_audience";
    public double ExpireHours { get; set; } = 24; 
}
