﻿using Business.Abstract;
using Business.Constants;
using Core.Ultities.Business;
using Core.Ultities.Helpers.FileHelper;
using Core.Ultities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CarImageManager : ICarImageService
    {
        ICarImageDal _carImage;
        IFileHelper _fileHelper;
        public CarImageManager(ICarImageDal carImageDal, IFileHelper fileHelper)
        {
            _carImage = carImageDal;
            _fileHelper = fileHelper;
        }



        public IResult Add(IFormFile file, CarImage carImage)
        {
            IResult result = BusinessRules.Run(CheckIfCarImageLimit(carImage.CarId));
            if (result != null)
            {
                return result;
            }
            carImage.ImagePath = _fileHelper.Upload(file, PathConstants.ImagesPath);
            carImage.ImageDate = DateTime.Now;
            _carImage.Add(carImage);
            return new SuccessResult("araç resmi eklendi");
        }

        public IResult Delete(CarImage carImage)
        {
            _fileHelper.Delete(PathConstants.ImagesPath + carImage.ImagePath);
            _carImage.Delete(carImage);
            return new SuccessResult("resim silindi.");
        }

        public IDataResult<List<CarImage>> GetAll()
        {
            return new SuccessDataResult<List<CarImage>>(_carImage.GetAll());
        }

        public IDataResult<List<CarImage>> GetByCarId(int carId)
        {
            var result = BusinessRules.Run(CheckCarImagesExist(carId));
            if (result != null)
            {
                return new ErrorDataResult<List<CarImage>>(GetDefaultImage(carId).Data);
            }
            return new SuccessDataResult<List<CarImage>>(_carImage.GetAll(x => x.CarId == carId));
        }

        public IDataResult<CarImage> GetById(int imageId)
        {
            return new SuccessDataResult<CarImage>(_carImage.Get(x => x.CarImageId == imageId));
        }

        public IResult Update(IFormFile file, CarImage carImage)
        {
            carImage.ImagePath = _fileHelper.Update(file, PathConstants.ImagesPath + carImage,PathConstants.ImagesPath);
            carImage.ImageDate = DateTime.Now;
            _carImage.Update(carImage);
            return new SuccessResult("araç resmi güncellendi.");
        }


        //

        private IResult CheckCarImagesExist(int carID)
        {
            var result = _carImage.GetAll(x=>x.CarId==carID).Count();
            if (result > 0)
            {
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        private IDataResult<List<CarImage>> GetDefaultImage(int carID)
        {
            List<CarImage> carImages = new List<CarImage>();
            carImages.Add(new CarImage { ImageDate = DateTime.Now, CarId = carID, ImagePath = "1.jpg" });
            return new SuccessDataResult<List<CarImage>>(carImages);
        }

        private IResult CheckIfCarImageLimit(int carID)
        {
            var result = _carImage.GetAll(x => x.CarId == carID).Count();
            if (result >= 5)
            {
                return new ErrorResult();
            }
            return new SuccessResult();
        }

    }
}
