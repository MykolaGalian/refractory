using System;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IModeratorService : IDisposable
    {
        Task BlockRefractory(int refId, string userName);
        Task UnblockRefractory(int refId);
    }
}
