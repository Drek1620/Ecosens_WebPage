using Azure.Storage.Blobs;
using Azure.Storage;
namespace Ecosens_WebPage.Helpers
{
    public static class StorageHelpers
    {
        public static async Task<string> SubirArchivo(Stream contenido, string nombre, AzureStorageConfig config)
        {
            var url = $"https://{config.Cuenta}.blob.core.windows.net/{config.Contenedor}/{nombre}";
            var uri = new Uri(url);
            var credenciales = new StorageSharedKeyCredential(config.Cuenta, config.Llave);
            var cliente = new BlobClient(uri, credenciales);
            await cliente.UploadAsync(contenido);
            return url;
        }

    }
}
