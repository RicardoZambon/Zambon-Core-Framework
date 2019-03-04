using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Zambon.Core.Module.Interfaces
{
    /// <summary>
    /// Interfaced used to upload files.
    /// </summary>
    public interface IFormFile
    {
        /// <summary>
        /// The content type.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// 
        /// </summary>
        string ContentDisposition { get; }

        /// <summary>
        /// 
        /// </summary>
        IHeaderDictionary Headers { get; }
        
        /// <summary>
        /// Length of the tile.
        /// </summary>
        long Length { get; }

        /// <summary>
        /// Name of the file.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Stream OpenReadStream();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        void CopyTo(Stream target);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CopyToAsync(Stream target, CancellationToken cancellationToken);

    }
}