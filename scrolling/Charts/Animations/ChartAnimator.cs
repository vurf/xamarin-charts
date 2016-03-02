using System;
using CoreAnimation;
using Foundation;
using ObjCRuntime;

namespace scrolling
{

    public interface IChartAnimatorDelegate
    {
        /// Called when the Animator has stepped.
        void chartAnimatorUpdated(ChartAnimator chartAnimator);

        /// Called when the Animator has stopped.
        void chartAnimatorStopped(ChartAnimator chartAnimator);
    }

    public class ChartAnimator : NSObject
    {
        public IChartAnimatorDelegate Delegate;
        public event Action updateBlock;
        public event Action stopBlock;

        /// the phase that is animated and influences the drawn values on the x-axis
        public nfloat phaseX = 1.0f;

        /// the phase that is animated and influences the drawn values on the y-axis
        public nfloat phaseY = 1.0f;

        private nfloat _startTimeX = 0.0f;
        private nfloat _startTimeY = 0.0f;
        private CADisplayLink _displayLink;

        private nfloat _durationX = 0.0f;
        private nfloat _durationY = 0.0f;

        private nfloat _endTimeX = 0.0f;
        private nfloat _endTimeY = 0.0f;
        private nfloat _endTime = 0.0f;

        private bool _enabledX = false;
        private bool _enabledY = false;

        private ChartEasingFunctionBlock _easingX;
        private ChartEasingFunctionBlock _easingY;

        public ChartAnimator()
        {

        }

        ~ChartAnimator()
        {
            stop();
        }

        public void stop()
        {
            if (_displayLink != null)
            {
                _displayLink.RemoveFromRunLoop(NSRunLoop.Main, NSRunLoop.NSRunLoopCommonModes);
                _displayLink = null;

                _enabledX = false;
                _enabledY = false;

                // If we stopped an animation in the middle, we do not want to leave it like this
                if (phaseX != 1.0f || phaseY != 1.0f)
                {
                    phaseX = 1.0f;
                    phaseY = 1.0f;

                    if (Delegate != null)
                    {
                        Delegate.chartAnimatorUpdated(this);
                    }
                    if (updateBlock != null)
                    {
                        OnUpdateBlock();
                    }
                }

                if (Delegate != null)
                {
                    Delegate.chartAnimatorStopped(this);
                }
                if (stopBlock != null)
                {
                    OnStopBlock();
                }
            }
        }

        private void updateAnimationPhases(nfloat currentTime)
        {
            if (_enabledX)
            {
                var elapsedTime = currentTime - _startTimeX;
                var duration = _durationX;
                var elapsed = elapsedTime;

                if (elapsed > duration)
                {
                    elapsed = duration;
                }

                if (_easingX != null)
                {
                    phaseX = _easingX(elapsed, duration);
                }
                else
                {
                    phaseX = (nfloat) (elapsed/duration);
                }
            }
            if (_enabledY)
            {
                var elapsedTime = currentTime - _startTimeY;
                var duration = _durationY;
                var elapsed = elapsedTime;
                if (elapsed > duration)
                {
                    elapsed = duration;
                }

                if (_easingY != null)
                {
                    phaseY = _easingY(elapsed, duration);
                }
                else
                {
                    phaseY = (nfloat) (elapsed/duration);
                }
            }
        }

        private void animationLoop()
        {
            //FIXME current media type
            nfloat currentTime = 0f;

            updateAnimationPhases(currentTime);

            if (Delegate != null)
            {
                Delegate.chartAnimatorUpdated(this);
            }
            if (updateBlock != null)
            {
                OnUpdateBlock();
            }

            if (currentTime >= _endTime)
            {
                stop();
            }
        }

        /// Animates the drawing / rendering of the chart on both x- and y-axis with the specified animation time.
        /// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
        /// - parameter xAxisDuration: duration for animating the x axis
        /// - parameter yAxisDuration: duration for animating the y axis
        /// - parameter easingX: an easing function for the animation on the x axis
        /// - parameter easingY: an easing function for the animation on the y axis
        public void animate(nfloat xAxisDuration, nfloat yAxisDuration, ChartEasingFunctionBlock easingX,
            ChartEasingFunctionBlock easingY)
        {
            stop();
            //FIXME current media type
            _startTimeX = 0;
            _startTimeY = _startTimeX;
            _durationX = xAxisDuration;
            _durationY = yAxisDuration;
            _endTimeX = _startTimeX + xAxisDuration;
            _endTimeY = _startTimeY + yAxisDuration;
            _endTime = _endTimeX > _endTimeY ? _endTimeX : _endTimeY;
            _enabledX = xAxisDuration > 0.0;
            _enabledY = yAxisDuration > 0.0;

            _easingX = easingX;
            _easingY = easingY;

            // Take care of the first frame if rendering is already scheduled...
            updateAnimationPhases(_startTimeX);

            if (_enabledX || _enabledY)
            {
                _displayLink = CADisplayLink.Create(this, new Selector("animationLoop"));
                _displayLink.AddToRunLoop(NSRunLoop.Main, NSRunLoopMode.Common);
            }
        }

        /// Animates the drawing / rendering of the chart on both x- and y-axis with the specified animation time.
        /// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
        /// - parameter xAxisDuration: duration for animating the x axis
        /// - parameter yAxisDuration: duration for animating the y axis
        /// - parameter easingOptionX: the easing function for the animation on the x axis
        /// - parameter easingOptionY: the easing function for the animation on the y axis
        public void animate(nfloat xAxisDuration, nfloat yAxisDuration, ChartEasingOption easingOptionX, ChartEasingOption easingOptionY)
        {
			animate(xAxisDuration, yAxisDuration, EasingFunctions.easingFunctionFromOption(easingOptionX), EasingFunctions.easingFunctionFromOption(easingOptionY));
        }

        /// Animates the drawing / rendering of the chart on both x- and y-axis with the specified animation time.
        /// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
        /// - parameter xAxisDuration: duration for animating the x axis
        /// - parameter yAxisDuration: duration for animating the y axis
        /// - parameter easing: an easing function for the animation
        public void animate(nfloat xAxisDuration, nfloat yAxisDuration, ChartEasingFunctionBlock easing)
        {
            animate(xAxisDuration, yAxisDuration, easing, easing);
        }

        /// Animates the drawing / rendering of the chart on both x- and y-axis with the specified animation time.
        /// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
        /// - parameter xAxisDuration: duration for animating the x axis
        /// - parameter yAxisDuration: duration for animating the y axis
        /// - parameter easingOption: the easing function for the animation
        public void animate(nfloat xAxisDuration, nfloat yAxisDuration, ChartEasingOption easingOption)
        {
			animate(xAxisDuration, yAxisDuration, EasingFunctions.easingFunctionFromOption(easingOption));
        }

        /// Animates the drawing / rendering of the chart on both x- and y-axis with the specified animation time.
        /// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
        /// - parameter xAxisDuration: duration for animating the x axis
        /// - parameter yAxisDuration: duration for animating the y axis
        public void animate(nfloat xAxisDuration, nfloat yAxisDuration)
        {
			animate(xAxisDuration, yAxisDuration, ChartEasingOption.EaseInOutSine);
        }

        /// Animates the drawing / rendering of the chart the x-axis with the specified animation time.
        /// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
        /// - parameter xAxisDuration: duration for animating the x axis
        /// - parameter easing: an easing function for the animation
        public void animate(nfloat xAxisDuration, ChartEasingFunctionBlock easing)
        {
            //FIXME current time type
            _startTimeX = 0; //CACurrentMediaTime()
            _durationX = xAxisDuration;
            _endTimeX = _startTimeX + xAxisDuration;
            _endTime = _endTimeX > _endTimeY ? _endTimeX : _endTimeY;
            _enabledX = xAxisDuration > 0.0

            _easingX = easing;

            // Take care of the first frame if rendering is already scheduled...
            updateAnimationPhases(_startTimeX);

            if (_enabledX || _enabledY)
            {
                if (_displayLink == null)
                {
                    _displayLink = CADisplayLink.Create(this, new Selector("animationLoop"));
                    _displayLink.AddToRunLoop(NSRunLoop.Main, NSRunLoopMode.Common);
                }
            }
        }

        /// Animates the drawing / rendering of the chart the x-axis with the specified animation time.
        /// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
        /// - parameter xAxisDuration: duration for animating the x axis
        /// - parameter easingOption: the easing function for the animation
        public void animate(nfloat xAxisDuration, ChartEasingOption easingOption)
        {
			animate(xAxisDuration, EasingFunctions.easingFunctionFromOption(easingOption));
        }

        /// Animates the drawing / rendering of the chart the x-axis with the specified animation time.
        /// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
        /// - parameter xAxisDuration: duration for animating the x axis
        public void animate(nfloat xAxisDuration)
        {
			animate(xAxisDuration, ChartEasingOption.EaseInOutSine)
            ;
        }

        /// Animates the drawing / rendering of the chart the y-axis with the specified animation time.
        /// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
        /// - parameter yAxisDuration: duration for animating the y axis
        /// - parameter easing: an easing function for the animation
        public void animate(nfloat yAxisDuration, ChartEasingFunctionBlock easing)
        {
            //FIXME current media time CACurrentMediaTime()
            _startTimeY = 0;
            _durationY = yAxisDuration;
            _endTimeY = _startTimeY + yAxisDuration;
            _endTime = _endTimeX > _endTimeY ? _endTimeX : _endTimeY;
            _enabledY = yAxisDuration > 0.0;

            _easingY = easing;

            // Take care of the first frame if rendering is already scheduled...
            updateAnimationPhases(_startTimeY);

            if (_enabledX || _enabledY)
            {
                if (_displayLink == null)
                {
                    _displayLink = CADisplayLink.Create(this, new Selector("animationLoop"));
                    _displayLink.AddToRunLoop(NSRunLoop.Main, NSRunLoopMode.Common);
                }
            }
        }

        /// Animates the drawing / rendering of the chart the y-axis with the specified animation time.
        /// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
        /// - parameter yAxisDuration: duration for animating the y axis
        /// - parameter easingOption: the easing function for the animation
        public void animate(nfloat yAxisDuration, ChartEasingOption easingOption)
        {
			animate(yAxisDuration, EasingFunctions.easingFunctionFromOption(easingOption));
        }

        /// Animates the drawing / rendering of the chart the y-axis with the specified animation time.
        /// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
        /// - parameter yAxisDuration: duration for animating the y axis
        public void animate(nfloat yAxisDuration)
        {
			animate(yAxisDuration, ChartEasingOption.EaseInOutSine)
            ;
        }

        protected virtual void OnUpdateBlock()
        {
            var handler = updateBlock;
            if (handler != null) handler();
        }

        protected virtual void OnStopBlock()
        {
            var handler = stopBlock;
            if (handler != null) handler();
        }
    }
}