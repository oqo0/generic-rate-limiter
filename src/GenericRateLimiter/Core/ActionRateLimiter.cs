namespace GenericRateLimiter.Core
{
    public class ActionRateLimiter(long limit, TimeSpan period)
    {
        public long Limit { get; } = limit;
        public TimeSpan Period { get; } = period;

        private long _currentLimit = limit;
        private DateTime _lastResetTime = DateTime.UtcNow;

        /// <summary>
        /// Triggers the rate limiter.
        /// </summary>
        /// <returns>A boolean indicating whether the action is limited.</returns>
        public bool Trigger()
        {
            ResetLimitIfPeriodElapsed();

            if (_currentLimit <= 0)
                return true;
            
            _currentLimit--;
            return false;
        }

        /// <summary>
        /// Resets the current limit if the period has elapsed.
        /// </summary>
        private void ResetLimitIfPeriodElapsed()
        {
            if (DateTime.UtcNow - _lastResetTime <= Period)
                return;
            
            _currentLimit = Limit;
            _lastResetTime = DateTime.UtcNow;
        }
    }
}