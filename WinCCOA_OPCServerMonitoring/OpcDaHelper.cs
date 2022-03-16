using OPCWrapper.DataAccess;
using OPCWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace WinCCOA_OPCServerMonitoring
{
    class OpcDaHelper : IOpcHelper
    {
        private readonly ILogger _logger;
        private Settings _settings = Settings.GetSettings();
        private OpcDaClient _opcDaClient;
        public OpcDaHelper(ILogger logger)
        {
            _logger = logger;
        }

        public bool Initialize()
        {
            try
            {
                _logger.Info($"Creating OPC DA client instance");
                _opcDaClient = new OpcDaClient(new ConnectionSettings(_settings.OpcServerAddress, _settings.OpcServerName));
                _opcDaClient.RegisterLogger(_logger);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error creating instance:\n{ex}");
                return false;
            }
        }

        public bool Check()
        {
            bool checkResult = false;
            if (_opcDaClient.Connect())
            {
                if (_opcDaClient.IsConnected)
                {
                    try
                    {
                        _logger.Info($"Reading OPC DA: [{_settings.OpcSignalName}]");
                        var opcReadResult = _opcDaClient.ReadData(_settings.OpcSignalName);
                        _logger.Info($"Readed value: ItemName = {opcReadResult.ItemName}, Value = {opcReadResult.Value}, Quality = {opcReadResult.Quality}, Timestamp = {opcReadResult.Timestamp}");
                        checkResult = CheckReadResult(opcReadResult);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Error check:\n{ex}");
                    }
                }
            }
            return checkResult;
        }
        private bool CheckReadResult(OpcDaReadResult readResult)
        {
            return readResult.Quality >= 192;
        }
    }
}
