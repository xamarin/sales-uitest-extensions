using System;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;
using Xamarin.UITest;
using System.Linq;
using System.Threading;

namespace LinkedIn.Tests.Android
{
	public static class Extensions
	{
		public static void ScrollDownAndTap(this AndroidApp app, Func<AppQuery, AppQuery> lambda = null, string screenshot = null)
		{
			app.ScrollDownEnough(lambda);

			if(screenshot != null)
				app.Screenshot(screenshot);

			app.Tap(lambda);
		}

		/// <summary>
		/// Incrementally scrolls down until the desired elements are found
		/// </summary>
		public static void ScrollDownEnough(this AndroidApp app, Func<AppQuery, AppQuery> lambda)
		{
			AppResult rootView = null;
			int count = 0;
			int maxTries = 20;

			while(app.Query(lambda).Length == 0 && count < maxTries)
			{
				if(rootView == null)
					rootView = app.Query(e => e.All()).First();

				//Will try to scroll +/-100 from the vertical center point
				float gap = 100;

				//Take into account where the syeahcreen is not large and the gap would be too big
				if(rootView.Rect.Height < gap * 2)
					gap = rootView.Rect.Height / 4;

				app.DragCoordinates(rootView.Rect.CenterX, rootView.Rect.CenterY + gap, rootView.Rect.CenterX, rootView.Rect.CenterY - gap);
				count++;
			}

			if(count == maxTries)
			{
				throw new Exception("Unable to scroll down to find element");
			}
		}

		public static void WaitThenEnterText(this IApp app, Func<AppQuery, AppQuery> lambda, string text, string screenshot = null)
		{
			app.WaitForElement(lambda);
			app.EnterText(lambda, text);

			if(screenshot != null)
				app.Screenshot(screenshot);
		}

		public static void EnterText(this IApp app, Func<AppQuery, AppQuery> lambda, string text, string screenshot)
		{
			app.EnterText(lambda, text);
			app.Screenshot(screenshot);
		}

		public static void Tap(this IApp app, string screenshot, Func<AppQuery, AppQuery> lambda)
		{
			app.Screenshot(screenshot);
			app.Tap(lambda);
		}

		public static void Tap(this IApp app, Func<AppQuery, AppQuery> lambda, string screenshot)
		{
			app.Tap(lambda);
			app.Screenshot(screenshot);
		}

		public static void WaitThenTapIfExists(this IApp app, Func<AppQuery, AppQuery> lambda, int timeout = 5)
		{
			int count = 0;

			while(count < timeout && app.Query(lambda).Length == 0)
			{
				Thread.Sleep(1000);
				count++;
			}

			if(app.Query(lambda).Length > 0)
			{
				app.Tap(lambda);
			}
		}

		public static void WaitThenTap(this IApp app, string screenshot, Func<AppQuery, AppQuery> lambda)
		{
			app.WaitForElement(lambda);

			if(screenshot != null)
				app.Screenshot(screenshot);

			app.Tap(lambda);
		}

		public static void WaitThenTap(this IApp app, Func<AppQuery, AppQuery> lambda, string screenshot = null)
		{
			app.WaitForElement(lambda);
			app.Tap(lambda);

			if(screenshot != null)
				app.Screenshot(screenshot);
		}
	}
}

