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
    public class CountryService
    {
        #region Properties
        private readonly ICountryRepository _repository;
        #endregion

        #region Constructors
        public CountryService(ICountryRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods
        public List<Country> GeefLandenContinent(int ContinentId)
        {
            try
            {
                return _repository.GeefLandenContinent(ContinentId);
            }
            catch (Exception ex)
            {
                throw new CountryServiceException("GeefLandenContinent - error", ex);
            }
        }

        public bool HeeftLanden(int continentId)
        {
            try
            {
                return _repository.HeeftLanden(continentId);
            }
            catch (Exception ex)
            {
                throw new CountryServiceException("HeeftLanden - error", ex);
            }
        }

        public Country LandToevoegen(Country country)
        {
            try
            {
                if (_repository.BestaatLand(country.Name, country.Continent.Id))
                {
                    throw new CountryServiceException("Land bestaat al.");
                }
                return _repository.LandToevoegen(country);
            }
            catch (Exception ex)
            {
                throw new CountryServiceException("LandToevoegen - error", ex);
            }
        }

        public Country LandWeergeven(int countryId)
        {
            try
            {
                if (!_repository.BestaatLand(countryId))
                {
                    throw new CountryServiceException("Land bestaat niet.");
                }
                return _repository.LandWeergeven(countryId);
            }
            catch (Exception ex)
            {
                throw new CountryServiceException("LandWeergeven - error", ex);
            }
        }

        public void LandVerwijderen(int landId)
        {
            try
            {
                if (!_repository.BestaatLand(landId))
                {
                    throw new CountryServiceException("Land bestaat niet.");
                }
                _repository.LandVerwijderen(landId);
            }
            catch (Exception ex)
            {
                throw new CountryServiceException("LandVerwijderen - error", ex);
            }
        }

        public Country LandUpdaten(Country country)
        {
            try
            {
                if (_repository.BestaatLand(country.Name, country.Continent.Id))
                {
                    throw new CountryServiceException("Land bestaat al.");
                }
                if (!_repository.BestaatLand(country.Id))
                {
                    throw new CountryServiceException("Land bestaat niet.");
                }
                return _repository.LandUpdaten(country);
            }
            catch (Exception ex)
            {
                throw new CountryServiceException("LandUpdaten - error", ex);
            }
        }

        public bool BestaatLand(int countryId)
        {
            try
            {
                return _repository.BestaatLand(countryId);
            }
            catch (Exception ex)
            {
                throw new CountryServiceException("BestaatLand - error", ex);
            }
        }

        public bool ZitLandInContinent(int continentId, int countryId)
        {
            try
            {
                return _repository.ZitLandInContinent(continentId, countryId);
            }
            catch (Exception ex)
            {
                throw new CountryServiceException("ZitLandInContinent - error", ex);
            }
        }
        #endregion
    }
}
