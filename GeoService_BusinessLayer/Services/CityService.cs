using DomeinLaag.Exceptions;
using DomeinLaag.Interfaces;
using GeoService_BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomeinLaag.Services
{
    public class CityService
    {
        #region Properties
        private readonly ICityRepository _repository;
        #endregion

        #region Constructors
        public CityService(ICityRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods
        public List<City> GeefStedenLand(int countryId)
        {
            try
            {
                return _repository.GeefStedenLand(countryId);
            }
            catch (Exception ex)
            {
                throw new CityServiceException("GeefLandenContinent - error", ex);
            }
        }

        public bool HeeftSteden(int countryId)
        {
            try
            {
                return _repository.HeeftSteden(countryId);
            }
            catch (Exception ex)
            {
                throw new CityServiceException("HeeftSteden - error", ex);
            }
        }

        public City StadToevoegen(City city)
        {
            try
            {
                if (_repository.BestaatStad(city.Id))
                {
                    throw new CityServiceException("Stad bestaat al.");
                }
                return _repository.StadToevoegen(city);
            }
            catch (Exception ex)
            {
                throw new CityServiceException("StadToevoegen - error", ex);
            }
        }

        public City StadWeergeven(int cityId)
        {
            try
            {
                if (!_repository.BestaatStad(cityId))
                {
                    throw new CityServiceException("Stad bestaat niet.");
                }
                return _repository.StadWeergeven(cityId);
            }
            catch (Exception ex)
            {
                throw new CityServiceException("StadWeergeven - error", ex);
            }
        }

        public bool BestaatStad(int cityId)
        {
            try
            {
                return _repository.BestaatStad(cityId);
            }
            catch (Exception ex)
            {
                throw new CityServiceException("BestaatStad - error", ex);
            }
        }

        public void StadVerwijderen(int cityId)
        {
            try
            {
                if (!_repository.BestaatStad(cityId))
                {
                    throw new CityServiceException("Stad bestaat niet.");
                }
                _repository.StadVerwijderen(cityId);
            }
            catch (Exception ex)
            {
                throw new CityServiceException("StadVerwijderen - error", ex);
            }
        }

        public City StadUpdaten(City city)
        {
            try
            {
                if (!_repository.BestaatStad(city.Id))
                {
                    throw new CityServiceException("Stad bestaat niet.");
                }
                return _repository.StadUpdaten(city);
            }
            catch (Exception ex)
            {
                throw new CityServiceException("StadUpdaten - error", ex);
            }
        }

        public bool ControleerBevolkingsaantal(int countryId, int population)
        {
            try
            {
                return _repository.ControleerBevolkingsaantal(countryId, population);
            }
            catch (Exception ex)
            {
                throw new CityServiceException("ControleerBevolkingsaantal - error", ex);
            }
        }

        public bool ZitStadInLandInContinent(int continentId, int countryId, int cityId)
        {
            try
            {
                return _repository.ZitStadInLandInContinent(continentId, countryId, cityId);
            }
            catch (Exception ex)
            {
                throw new CityServiceException("ZitStadInLandInContinent - error", ex);
            }
        }
        #endregion
    }
}
