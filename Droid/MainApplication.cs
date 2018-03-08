using System;

using Android.App;
using Android.OS;
using Android.Runtime;
using FoodSpecs.PCL;
using Ninject;
using Plugin.CurrentActivity;

namespace FoodSpecs.Droid
{
	//You can specify additional application information in this attribute
	#if (!DEBUG)
	[Application(Debuggable=false)]
	#else
	[Application(Debuggable = true)]
	#endif
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
		public static IKernel Container { get; set; }

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          :base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            //A great place to initialize Xamarin.Insights and Dependency Services!

			Startup.BuildContainer();
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
			
        }
    }
}