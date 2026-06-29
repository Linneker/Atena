namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Comprovantes.Query.ObterComprovantePdf;

public sealed record ObterComprovantePdfQueryResult(
    byte[] PdfBytes,
    string FileName,
    string ContentType = "application/pdf");
