using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;
using Tedu.Server.Status.DataAccess.Entities;

namespace Tedu.Server.Status.Web.BackgroundServices
{
    public sealed class ServerHttpProbeService : IDisposable
    {
        private readonly DataAccess.Entities.Server _server;
        private readonly HttpClient _httpClient;

        private Probe _currentProbe;
        private DateTime _lastProbeDateTimeUtc;
        private SslPolicyErrors _sslPolicyErrors;

        public ServerHttpProbeService(DataAccess.Entities.Server server)
        {
            _server = server ?? throw new ArgumentNullException(nameof(server));
            _httpClient = InitializeHttpClient();
        }

        public async Task<Probe> CheckHostIsReachableAsync(DateTime checkedDateTimeUtc)
        {
            _currentProbe = new Probe
            {
                ServerId = _server.Id,
                CheckedDateTimeUtc = checkedDateTimeUtc,
                Type = ProbeType.IsHostReachable
            };
            bool isReachable = await IsHostReachableAsync(_server.Host);
            _currentProbe.Result = isReachable ? ProbeResult.Success : ProbeResult.Failure;
            return _currentProbe;
        }

        public async Task<Probe> PerformProbeAsync(
            ProbeType probeType,
            string uriTemplate,
            DateTime checkedDateTimeUtc)
        {
            _currentProbe = new Probe
            {
                ServerId = _server.Id,
                CheckedDateTimeUtc = checkedDateTimeUtc,
                Type = probeType
            };
            if (_lastProbeDateTimeUtc == checkedDateTimeUtc && probeType == ProbeType.IsSslCertificateValid)
            {
                _currentProbe.Result = _sslPolicyErrors == SslPolicyErrors.None
                    ? ProbeResult.Success
                    : ProbeResult.Failure;
                return _currentProbe;
            }

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"https://{_server.Host}/{uriTemplate}");
                _lastProbeDateTimeUtc = checkedDateTimeUtc;
                Console.WriteLine(response);
                _currentProbe.Result = probeType == ProbeType.IsSslCertificateValid
                    ? _sslPolicyErrors == SslPolicyErrors.None
                        ? ProbeResult.Success
                        : ProbeResult.Failure
                    : response.StatusCode == HttpStatusCode.OK
                        ? ProbeResult.Success
                        : ProbeResult.Failure;
            }
            catch (Exception ex)
                when (ex is HttpRequestException || ex is OperationCanceledException)
            {
                Console.WriteLine(ex);
                _currentProbe.Result = ProbeResult.Failure;
            }
            
            return _currentProbe;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        private static async Task<bool> IsHostReachableAsync(string host)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    await client.ConnectAsync(host, 443);
                    return true;
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private HttpClient InitializeHttpClient()
        {
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (
                    httpRequestMessage,
                    x509Certificate2,
                    x509Chain,
                    sslPolicyErrors) =>
                {
                    Console.WriteLine(sslPolicyErrors);
                    _sslPolicyErrors = sslPolicyErrors;
                    
                    return true;
                }
            };
            return new HttpClient(httpClientHandler);
        }
    }
}
