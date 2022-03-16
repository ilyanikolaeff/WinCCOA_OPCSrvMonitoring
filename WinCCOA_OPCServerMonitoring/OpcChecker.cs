using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WinCCOA_OPCServerMonitoring
{
    class OpcChecker
    {
        private readonly WCCOAHelper _wccoaHelper;
        private readonly OpcDaHelper _opcDaHelper;
        private readonly ILogger _logger;
        private readonly Settings _settings = Settings.GetSettings();
        public OpcChecker(ILogger logger)
        {
            _logger = logger;
            _wccoaHelper = new WCCOAHelper(_logger);
            _opcDaHelper = new OpcDaHelper(_logger);
        }

        private readonly Timer _delayTimer = new Timer(Settings.GetSettings().Delay * 1000);

        public void Start()
        {
            // init helpers
            if (!_wccoaHelper.Initialize())
            {
                return;
            }


            // First time check
            PerformCheck();
            // Start timer 
            StartTimer();
        }

        public void Stop()
        {
            StopTimer();
        }

        private void StartTimer()
        {
            _delayTimer.AutoReset = true;
            _delayTimer.Elapsed += OnDelayTimerElapsed;
            _delayTimer.Start();
        }

        private void StopTimer()
        {
            _delayTimer.Stop();
        }

        private void OnDelayTimerElapsed(object sender, ElapsedEventArgs e)
        {
            PerformCheck();
        }

        private void PerformCheck()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            bool opcCheckResult = false;
            if (_opcDaHelper.Initialize())
                opcCheckResult = _opcDaHelper.Check();

            _logger.Info($"Check result = [{opcCheckResult}]");
            _wccoaHelper.DpSet(_settings.DataPointName, opcCheckResult ? _settings.DataPointValueWhenSuccess : _settings.DataPointValueWhenFailed);
            
            stopWatch.Stop();
            _logger.Info($"Perform check elapsed time: {stopWatch.Elapsed}");
        }
    }
}
