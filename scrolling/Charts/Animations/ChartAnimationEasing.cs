using System;

namespace scrolling
{
	public enum ChartEasingOption {
		Linear , EaseInQuad , EaseOutQuad , EaseInOutQuad , EaseInCubic , EaseOutCubic , EaseInOutCubic , EaseInQuart , EaseOutQuart , EaseInOutQuart , EaseInQuint , EaseOutQuint,
		EaseInOutQuint , EaseInSine , EaseOutSine , EaseInOutSine , EaseInExpo , EaseOutExpo, EaseInOutExpo, EaseInCirc, EaseOutCirc, EaseInOutCirc, EaseInElastic,
		EaseOutElastic, EaseInOutElastic, EaseInBack, EaseOutBack, EaseInOutBack, EaseInBounce, EaseOutBounce, EaseInOutBounce
	}

	public delegate nfloat ChartEasingFunctionBlock(nfloat elapsed, nfloat duration);

	internal class EasingFunctions
	{
		static nfloat EPSILON = 0.0001f;

		internal ChartEasingFunctionBlock easingFunctionFromOption(ChartEasingOption easing) {
			switch (easing) {
				case ChartEasingOption.Linear:
					return EasingFunctions.Linear;
				case ChartEasingOption.EaseInQuad:
					return EasingFunctions.EaseInQuad;
				case ChartEasingOption.EaseOutQuad:
					return EasingFunctions.EaseOutQuad;
				case ChartEasingOption.EaseInOutQuad:
					return EasingFunctions.EaseInOutQuad;
				case ChartEasingOption.EaseInCubic:
					return EasingFunctions.EaseInCubic;
				case ChartEasingOption.EaseOutCubic:
					return EasingFunctions.EaseOutCubic;
				case ChartEasingOption.EaseInOutCubic:
					return EasingFunctions.EaseInOutCubic;
				case ChartEasingOption.EaseInQuart:
					return EasingFunctions.EaseInQuart;
				case ChartEasingOption.EaseOutQuart:
					return EasingFunctions.EaseOutQuart;
				case ChartEasingOption.EaseInOutQuart:
					return EasingFunctions.EaseInOutQuart;
				case ChartEasingOption.EaseInQuint:
					return EasingFunctions.EaseInQuint;
				case ChartEasingOption.EaseOutQuint:
					return EasingFunctions.EaseOutQuint;
				case ChartEasingOption.EaseInOutQuint:
					return EasingFunctions.EaseInOutQuint;
				case ChartEasingOption.EaseInSine:
					return EasingFunctions.EaseInSine;
				case ChartEasingOption.EaseOutSine:
					return EasingFunctions.EaseOutSine;
				case ChartEasingOption.EaseInOutSine:
					return EasingFunctions.EaseInOutSine;
				case ChartEasingOption.EaseInExpo:
					return EasingFunctions.EaseInExpo;
				case ChartEasingOption.EaseOutExpo:
					return EasingFunctions.EaseOutExpo;
				case ChartEasingOption.EaseInOutExpo:
					return EasingFunctions.EaseInOutExpo;
				case ChartEasingOption.EaseInCirc:
					return EasingFunctions.EaseInCirc;
				case ChartEasingOption.EaseOutCirc:
					return EasingFunctions.EaseOutCirc;
				case ChartEasingOption.EaseInOutCirc:
					return EasingFunctions.EaseInOutCirc;
				case ChartEasingOption.EaseInElastic:
					return EasingFunctions.EaseInElastic;
				case ChartEasingOption.EaseOutElastic:
					return EasingFunctions.EaseOutElastic;
				case ChartEasingOption.EaseInOutElastic:
					return EasingFunctions.EaseInOutElastic;
				case ChartEasingOption.EaseInBack:
					return EasingFunctions.EaseInBack;
				case ChartEasingOption.EaseOutBack:
					return EasingFunctions.EaseOutBack;
				case ChartEasingOption.EaseInOutBack:
					return EasingFunctions.EaseInOutBack;
				case ChartEasingOption.EaseInBounce:
					return EasingFunctions.EaseInBounce;
				case ChartEasingOption.EaseOutBounce:
					return EasingFunctions.EaseOutBounce;
				case ChartEasingOption.EaseInOutBounce:
					return EasingFunctions.EaseInOutBounce;
			}
			return EaseInBounce;
		}
			
		internal static ChartEasingFunctionBlock Linear = delegate(nfloat elapsed, nfloat duration) {  
			return elapsed / duration; 
		};



		internal static ChartEasingFunctionBlock EaseInQuad = delegate(nfloat elapsed, nfloat duration) {
			var position = (nfloat)(elapsed / duration);
			return position * position;
		};

		internal static ChartEasingFunctionBlock EaseOutQuad = delegate(nfloat elapsed,nfloat duration) {
			var position = (nfloat)(elapsed / duration);
			return -position * (position - 2.0f);
		};

		internal static ChartEasingFunctionBlock EaseInOutQuad = delegate (nfloat elapsed,nfloat duration) {
			var position = (nfloat)(elapsed / (duration / 2.0f));
			if (position < 1.0f) 
				return 0.5f * position * position;
			return -0.5f * ((--position) * (position - 2.0f) - 1.0f);
		};

		internal static ChartEasingFunctionBlock EaseInCubic = delegate (nfloat elapsed,nfloat duration) {
			var position = elapsed / duration;
			return position * position * position;
		};

		internal static ChartEasingFunctionBlock EaseOutCubic = delegate (nfloat elapsed,nfloat duration) {
			var position = elapsed / duration;
			position--;
			return (position * position * position + 1.0f);
		};

		internal static ChartEasingFunctionBlock EaseInOutCubic = delegate (nfloat elapsed,nfloat duration) {
			var position = elapsed / (duration / 2.0f);
			if (position < 1.0f) 
				return 0.5f * position * position * position;
			position -= 2.0f;
			return 0.5f * (position * position * position + 2.0f);
		};

		internal static ChartEasingFunctionBlock EaseInQuart = delegate (nfloat elapsed,nfloat duration) {
			var position = elapsed / duration;
			return position * position * position * position;
		};

		internal static ChartEasingFunctionBlock EaseOutQuart = delegate (nfloat elapsed,nfloat duration) {
			var position = elapsed / duration;
			position--;
			return -(position * position * position * position - 1.0f);
		};

		internal static ChartEasingFunctionBlock EaseInOutQuart = delegate (nfloat elapsed,nfloat duration) {
			var position = elapsed / (duration / 2.0f);
			if (position < 1.0f)
					return 0.5f * position * position * position * position;
			position -= 2.0f;
			return -0.5f * (position * position * position * position - 2.0f);
		};

		internal static ChartEasingFunctionBlock EaseInQuint = delegate (nfloat elapsed,nfloat duration) {
			var position =elapsed / duration;
			return position * position * position * position * position;
		};

		internal static ChartEasingFunctionBlock EaseOutQuint = delegate (nfloat elapsed,nfloat duration) {
			var position = elapsed / duration;
			position--;
			return (position * position * position * position * position + 1.0f);
		};

		internal static ChartEasingFunctionBlock EaseInOutQuint = delegate (nfloat elapsed,nfloat duration) {
			var position = elapsed / (duration / 2.0f);
			if (position < 1.0f)
				return 0.5f * position * position * position * position * position;
			else
			{
				position -= 2.0f;
				return 0.5f * (position * position * position * position * position + 2.0f);
			}
		};

		internal static ChartEasingFunctionBlock EaseInSine = delegate (nfloat elapsed,nfloat duration) {
			var position = elapsed / duration;
			return (nfloat)( -Math.Cos(position * Math.PI/2) + 1.0);
		};

		internal static ChartEasingFunctionBlock EaseOutSine = delegate (nfloat elapsed,nfloat duration) {
			var position = elapsed / duration;
			return (nfloat)( Math.Sin(position * Math.PI/2) );
		};

		internal static ChartEasingFunctionBlock EaseInOutSine = delegate (nfloat elapsed,nfloat duration) {
			var position = elapsed / duration;
			return (nfloat)( -0.5f * (Math.Cos(Math.PI * position) - 1.0) );
		};

		internal static ChartEasingFunctionBlock EaseInExpo = delegate (nfloat elapsed,nfloat duration) {
			return (Math.Abs (elapsed) < EPSILON) ? 0.0f : (nfloat)(Math.Pow(2.0, 10.0 * (elapsed / duration - 1.0)));
		};

		internal static ChartEasingFunctionBlock EaseOutExpo = delegate (nfloat elapsed,nfloat duration) {
			return (elapsed == duration) ? 1.0f : -(nfloat)((Math.Pow(2.0, -10.0 * elapsed / duration)) + 1.0);
		};
			
		internal static ChartEasingFunctionBlock EaseInOutExpo = delegate (nfloat elapsed,nfloat duration) {
			if (elapsed == 0)
				return 0.0f;
			if (elapsed == duration)
				return 1.0f;

			var position = elapsed / (duration / 2.0f);
			if (position < 1.0)
				return (nfloat)( 0.5 * Math.Pow(2.0, 10.0 * (position - 1.0)) );
			return (nfloat)( 0.5 * (-Math.Pow(2.0, -10.0 * --position) + 2.0) );
		};

		internal static ChartEasingFunctionBlock EaseInCirc = delegate (nfloat elapsed,nfloat duration) {
			var position = elapsed / duration;
			return -(nfloat)((Math.Sqrt(1.0 - position * position)) - 1.0);
		};

		internal static ChartEasingFunctionBlock EaseOutCirc = delegate (nfloat elapsed,nfloat duration) {
			var position = elapsed / duration;
			position--;
			return (nfloat)( Math.Sqrt(1 - position * position) );
		};

		internal static ChartEasingFunctionBlock EaseInOutCirc = delegate (nfloat elapsed,nfloat duration) {
			var position = elapsed / (duration / 2.0);
			if (position < 1.0f)
				return (nfloat)( -0.5 * (Math.Sqrt(1.0 - position * position) - 1.0) );
			position -= 2.0f;
			return (nfloat)( 0.5 * (Math.Sqrt(1.0 - position * position) + 1.0) );
		};

		internal static ChartEasingFunctionBlock EaseInElastic = delegate (nfloat elapsed,nfloat duration) {
			if (Math.Abs (elapsed) < EPSILON)
				return 0.0f;

			var position = elapsed / duration;
			if (Math.Abs (position - 1.0f) < EPSILON)
				return 1.0f;
			var p = duration * 0.3f;
			var s = p / (2.0 * Math.PI) * Math.Asin(1.0);
			position -= 1.0f;
			return (nfloat)( -(Math.Pow(2.0, 10.0 * position) * Math.Sin((position * duration - s) * (2.0 * Math.PI) / p)) );
		};

		internal static ChartEasingFunctionBlock EaseOutElastic = delegate (nfloat elapsed,nfloat duration) {
			if (Math.Abs (elapsed) < EPSILON)
				return 0.0f;

			var position = elapsed / duration;
			if (Math.Abs (position - 1.0f) < EPSILON)
				return 1.0f;

			var p = duration * 0.3f;
			var s = p / (2.0 * Math.PI) * Math.Asin(1.0);
			return (nfloat)( Math.Pow(2.0, -10.0 * position) * Math.Sin((position * duration - s) * (2.0 * Math.PI) / p) + 1.0 );
		};

		internal static ChartEasingFunctionBlock EaseInOutElastic = delegate (nfloat elapsed,nfloat duration) {
			if (Math.Abs (elapsed) < EPSILON)
				return 0.0f;

			var position = elapsed / (duration / 2.0);
			if (Math.Abs (position - 2.0f) < EPSILON)
				return 1.0f;

			var p = duration * (0.3 * 1.5);
			var s = p / (2.0 * Math.PI) * Math.Asin(1.0);
			if (position < 1.0)
			{
				position -= 1.0f;
				return (nfloat)( -0.5 * (Math.Pow(2.0, 10.0 * position) * Math.Sin((position * duration - s) * (2.0 * Math.PI) / p)) );
			}
			position -= 1.0;
			return (nfloat)( Math.Pow(2.0, -10.0 * position) * Math.Sin((position * duration - s) * (2.0 * Math.PI) / p) * 0.5 + 1.0 );
		};

		internal static ChartEasingFunctionBlock EaseInBack = delegate (nfloat elapsed,nfloat duration) {
			var s = 1.70158f;
			var position = elapsed / duration;
			return (nfloat)( position * position * ((s + 1.0) * position - s) );
		};

		internal static ChartEasingFunctionBlock EaseOutBack = delegate (nfloat elapsed,nfloat duration) {
			var s = 1.70158f;
			var position = elapsed / duration;
			position--;
			return (nfloat)( position * position * ((s + 1.0) * position + s) + 1.0 );
		};

		internal static ChartEasingFunctionBlock EaseInOutBack = delegate (nfloat elapsed,nfloat duration) {
			var s = 1.70158f;
			var position = elapsed / (duration / 2.0);
			if (position < 1.0f)
			{
				s *= 1.525f;
				return (nfloat)( 0.5 * (position * position * ((s + 1.0) * position - s)) );
			}
			s *= 1.525f;
			position -= 2.0f;
			return (nfloat)( 0.5 * (position * position * ((s + 1.0) * position + s) + 2.0) );
		};

		internal static ChartEasingFunctionBlock EaseInBounce = delegate (nfloat elapsed,nfloat duration) {
			return 1.0f - EaseOutBounce(duration - elapsed, duration);
		};

		internal static ChartEasingFunctionBlock EaseOutBounce = delegate (nfloat elapsed,nfloat duration) {
			var position = elapsed / duration;
			if (position < (1.0f / 2.75f))
				return (nfloat)( 7.5625f * position * position );
			else if (position < (2.0f / 2.75f))
			{
				position -= (1.5f / 2.75f);
				return (nfloat)( 7.5625f * position * position + 0.75f );
			}
			else if (position < (2.5f / 2.75f))
			{
				position -= (2.25f / 2.75f);
				return (nfloat)( 7.5625f * position * position + 0.9375f );
			}
			else
			{
				position -= (2.625f / 2.75f);
				return (nfloat)( 7.5625f * position * position + 0.984375f );
			}
		};

		internal static ChartEasingFunctionBlock EaseInOutBounce = delegate (nfloat elapsed,nfloat duration) {
			if (elapsed < (duration / 2.0f))
				return EaseInBounce(elapsed * 2.0f, duration) * 0.5f;
			return EaseOutBounce(elapsed * 2.0f - duration, duration) * 0.5f + 0.5f;
		};

	}
}

