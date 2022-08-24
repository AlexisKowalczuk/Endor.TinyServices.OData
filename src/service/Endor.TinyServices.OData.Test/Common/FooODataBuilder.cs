using Endor.TinyServices.OData.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endor.TinyServices.OData.Test.Common
{
	internal class FooODataBuilder : ODataBuilder
	{
		public override string GetColumnsString()
		{
			return string.Empty;
		}

		public override string GetNewEntityIndex()
		{
			return string.Empty;
		}
	}
}
