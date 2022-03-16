using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCCOACOMLib;

namespace WinCCOA_OPCServerMonitoring
{
    class WCCOAHelper : IHelper
    {
        private readonly ILogger _logger;
        public WCCOAHelper(ILogger logger)
        {
            _logger = logger;
        }

       private ComManager _comManager;

        public bool Initialize()
        {
            var cmdLine = Settings.GetSettings().CmdLine;
            if (string.IsNullOrEmpty(cmdLine))
            {
                _logger.Info($"CMD_LINE = {cmdLine} incorrect!");
                return false;
            }
            if (_comManager == null)
            {
                _logger.Info($"Initializing WinCC OA ComManager with CMD_LINE = {cmdLine}");
                try
                {
                    _comManager = new ComManager();
                    _comManager.Init(cmdLine);
                    _logger.Info($"Initializing WinCC OA ComManager - OK");
                    return true;
                }
                catch(Exception ex)
                {
                    _logger.Error($"Initializing WinCC OA ComManager finished with error:\n{ex}");
                    return false;
                }
            }
            else
            {
                _logger.Info($"ComManager already initialized");
                return true;
            }
        }

        public object DpGet(string dpName)
        {
            if (_comManager != null)
            {
                try
                {
                    dpName += ":_online.._value";
                    _comManager.dpGet(dpName, out object dpValue);
                    _logger.Info($"dpGet: dpName = {dpName}, dpValue = {dpValue} - OK");
                    return dpValue;
                }
                catch (Exception ex)
                {
                    _logger.Error($"dpGet error:\n{ex}");
                    return null;
                }
            }
            return null;
        }

        public void DpSet(string dpName, object dpValue)
        {
            if (_comManager != null)
            {
                try
                {
                    dpName += ":_original.._value";
                    _comManager.dpSet(dpName, dpValue);
                    _logger.Info($"dpSet: dpName = {dpName}, dpValue = {dpValue} - OK");
                }
                catch (Exception ex)
                {
                    _logger.Error($"dpSet error:\n{ex}");
                }
            }
        }
    }
}
