using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService.Models.Helpers
{
	interface ISoftDelete
	{
		bool IsDeleted { get; set; }
	}
}
