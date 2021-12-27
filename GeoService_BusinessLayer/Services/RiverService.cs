using GeoService_BusinessLayer.Interfaces;
using GeoService_BusinessLayer.Models;
using GeoService_BusinessLayer.ServiceExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoService_BusinessLayer.Services {
    public class RiverService {
        private readonly IRiverRepository _repository;

        public RiverService(IRiverRepository repository)
        {
            _repository = repository;
        }
        public List<Country> geefLandenRivier(int riverId)
        {
            try
            {
                return _repository.GeefLandenRivier(riverId);
            }
            catch (Exception ex)
            {
                throw new RiverServiceException("geefLandenRivier - error - ", ex);
            }
        }
        public River RivierWeergeven(int riverId)
        {
            try
            {
                if (!_repository.BestaatRivier(riverId))
                {
                    throw new RiverServiceException("Rivier bestaat niet met deze id.");
                }
                return _repository.RivierWeergeven(riverId);
            }
            catch (Exception ex)
            {
                throw new RiverServiceException("RivierWeergeven - error", ex);
            }
        }
        public River RivierToevoegen(River river)
        {
            try
            {
                if (_repository.BestaatRivier(river.Name))
                {
                    throw new RiverServiceException("Rivier bestaat al met deze naam.");
                }
                return _repository.RivierToevoegen(river);
            }
            catch (Exception ex)
            {
                throw new RiverServiceException("Rivier toevoegen - error", ex);
            }
        }
        public void RivierVerwijderen(int riverId)
        {
            try
            {
                if (!_repository.BestaatRivier(riverId))
                {
                    throw new RiverServiceException("Rivier bestaat niet en kan niet verwijderd worden");
                }
                _repository.RivierVerwijderen(riverId);
            }
            catch (Exception ex)
            {
                throw new RiverServiceException("RivierVerwijderen - error - " + ex.Message);
            }
        }
    }
}