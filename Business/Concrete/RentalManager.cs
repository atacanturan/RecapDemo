﻿using Business.Abstract;
using Business.Constants;
using Core.Utilities.DataResults;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using Entities.DTO_s;

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {
        IRentalDal _rentalDal;

        public RentalManager(IRentalDal rentalDal)
        {
            _rentalDal = rentalDal;
        }
        public IResult Add(Rental rental)
        {
            var result = _rentalDal.GetAll(r => r.CarId == rental.CarId && r.ReturnDate == null);
            if (result.Count > 0)
            {
                return new ErrorResult(Messages.RentalAddedException);
            }
            _rentalDal.Add(rental);
            return new SuccessResult(Messages.Rented);
        }

        public IResult Delete(Rental rental)
        {
            if (DateTime.Now.Hour == 13)
            {
                _rentalDal.Delete(rental);
                return new SuccessResult(Messages.Deleted);
            }
            return new ErrorResult(Messages.MaintenenceTime);
        }

        public IDataResult<Rental> GetById(int id)
        {
            return new SuccessDataResult<Rental>(_rentalDal.Get(r => r.Id == id));
        }

        public IDataResult<List<RentalDetailDto>> GetRentalDetails()
        {
            return new SuccessDataResult<List<RentalDetailDto>>(_rentalDal.GetRentalInfos());
        }

        public IDataResult<List<Rental>> GetAll()
        {
            return new SuccessDataResult<List<Rental>>(_rentalDal.GetAll(), Messages.Listed);
        }

        public IResult Update(Rental rental)
        {
            if (DateTime.Now.Hour == 13)
            {
                _rentalDal.Update(rental);
                return new SuccessResult(Messages.Updated);
            }
            return new ErrorResult(Messages.MaintenenceTime);
        }
    }
}
