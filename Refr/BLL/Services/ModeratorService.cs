using BLL.Contracts;
using DAL.Contracts;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ModeratorService : IModeratorService
    {
        private readonly IUnitOfWork _uow;
        public ModeratorService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task BlockRefractory(int refId, string userName)
        {
            if (refId > 0 && userName != null && userName.Length > 3)
            {
                Refractory refractory = await _uow.Refractory.SelectById(refId);
                refractory.IsBlocked = true;
                await _uow.Refractory.Update(refractory);
            }
            else throw new ArgumentException("Wrong data");
        }

        public async Task UnblockRefractory(int refId)
        {
            if (refId > 0)
            {
                Refractory refractory = await _uow.Refractory.SelectById(refId);
                refractory.IsBlocked = false;
                await _uow.Refractory.Update(refractory);
            }
            else throw new ArgumentException("Wrong data");
        }

        public void Dispose()
        {
            _uow?.Dispose();
        }
    }
}
