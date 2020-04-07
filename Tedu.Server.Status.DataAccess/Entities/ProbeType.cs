namespace Tedu.Server.Status.DataAccess.Entities
{
    public enum ProbeType
    {
        IsHostReachable = 1,
        IsSslCertificateValid = 2,
        IsTutoringApiAvailable = 3,
        IsAdminApiAvailable = 4,
        IsTutoringSwaggerAvailable = 5,
        IsAdminSwaggerAvailable = 6
    }
}
