using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDD_in_Unity.Infrastructure
{
	public class HeartContainerBuilder : TestDataBuilder<HeartContainer>
	{
		private List<Heart> _list;

		public HeartContainerBuilder(List<Heart> list) {
			_list = list;
		}

		public HeartContainerBuilder() : this(null) { }

		public HeartContainerBuilder With(params Heart[] hearts) {
			_list = new List<Heart>(hearts);
			return this;
		}

		public override HeartContainer Build() {
			return new HeartContainer(_list);
		}
	}
}
