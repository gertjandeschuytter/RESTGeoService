using DomeinLaag.Exceptions;
using DomeinLaag.Interfaces;
using GeoService_BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomeinLaag.Services {
    public class ContinentService {
        #region Properties
        private readonly IContinentRepository _repository;
        #endregion

        #region Constructors
        public ContinentService(IContinentRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods
        public bool BestaatContinent(string name)
        {
            try
            {
                return _repository.BestaatContinent(name);
            }
            catch (Exception ex)
            {
                throw new ContinentServiceException("BestaatContinent - error", ex);
            }
        }

        public bool BestaatContinent(int id)
        {
            try
            {
                return _repository.BestaatContinent(id);
            }
            catch (Exception ex)
            {
                throw new ContinentServiceException("BestaatContinent - error", ex);
            }
        }

        public Continent ContinentToevoegen(Continent continent)
        {
            try
            {
                if (BestaatContinent(continent.Name))
                {
                    throw new ContinentServiceException("Continent bestaat al met deze naam.");
                }
                return _repository.ContinentToevoegen(continent);
            }
            catch (Exception ex)
            {
                throw new ContinentServiceException("ContinentToevoegen - error" + ex.Message);
            }
        }

        public Continent ContinentWeergeven(int continentId)
        {
            try
            {
                if (!_repository.BestaatContinent(continentId))
                {
                    throw new ContinentServiceException("Continent bestaat niet met deze id.");
                }
                return _repository.ContinentWeergevenMetLanden(continentId);
            }
            catch (Exception ex)
            {
                throw new ContinentServiceException("ContinentWeergeven - error - " + ex.Message);
            }
        }

        public void ContinentVerwijderen(int continentId)
        {
            try
            {
                if (_repository.BestaatContinent(continentId))
                {
                    var continentdb = _repository.ContinentWeergevenMetLanden(continentId);
                    if (continentdb.GetCountries().Count != 0)
                    {
                        throw new ContinentServiceException("Continent bevat nog landen kan niet verwijderd worden");
                    }
                    _repository.ContinentVerwijderen(continentId);
                }
                else
                {
                    throw new ContinentServiceException("Continent bestaat niet met deze id.");
                }
            }
            catch (Exception ex)
            {
                throw new ContinentServiceException("ContinentVerwijderen - error - " + ex.Message);
            }
        }

        public Continent ContinentUpdaten(Continent continent)
        {
            try
            {
                if (continent == null)
                {
                    throw new ContinentServiceException("Continent is null.");
                }
                if (!_repository.BestaatContinent(continent.Id))
                {
                    throw new ContinentServiceException("Continent bestaat niet.");
                }
                Continent continentDb = ContinentWeergeven(continent.Id);
                if (continentDb == continent)
                {
                    throw new ContinentServiceException("Er zijn geen verschillen met het origineel.");
                }
                _repository.ContinentUpdaten(continent);
                return continent;
            }
            catch (Exception ex)
            {
                throw new ContinentServiceException("ContinentUpdaten - error - " + ex.Message);
            }
        }
        #endregion
    }
}
