using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Athenas.JwtDomains
{
	public class JwtSecurityKey
	{
			public static SymmetricSecurityKey Create(string secret) =>
				new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
		}

}
