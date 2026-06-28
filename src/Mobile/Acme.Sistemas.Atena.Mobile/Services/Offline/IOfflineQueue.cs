using Acme.Sistemas.Atena.Mobile.Shared.Dtos;

namespace Acme.Sistemas.Atena.Mobile.Services.Offline;

public interface IOfflineQueue
{
    Task EnfileirarBatidaAsync(BaterPontoMobileForm form, byte[]? fotoBytes);
    Task<int> SyncPendentesAsync();
    Task<int> ContagemPendentesAsync();
    Task LimparAntigasAsync(int diasMin = 30);
}
