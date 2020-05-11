using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDD_in_Unity.Infrastructure
{
	public static class An
	{
		public static ImageBuilder Image() {
			return new ImageBuilder();
		}
	}

	public static class A
	{
		public static HeartBuilder Heart() {
			return new HeartBuilder();
		}

		public static HeartContainerBuilder HeartContainer() {
			return new HeartContainerBuilder();
		}
	}
}