using System;
using Android.App;
using Android.Runtime;
using Ninject;
using FoodSpecs.PCL;

namespace FoodSpecs.Droid
{
	[Application]
	public class DroidApp : Application
	{
		public static IKernel Container { get; set; }

		public DroidApp(IntPtr h, JniHandleOwnership jho) : base(h, jho)
		{
		}

		public override void OnCreate()
		{
			Startup.BuildContainer();
		}
	}
}

