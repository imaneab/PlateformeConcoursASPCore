﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetConcoursAspCore.Models.Helper
{
	interface ISoftDelete
	{
		bool IsDeleted { get; set; }
	}
}

