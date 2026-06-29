namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.DownloadAfd;

public static class DownloadAfdMap
{
    // Marker — endpoint resolve direto via repos.
    public static string DefaultFileName(Guid id) => $"afd-{id}.txt";
}
