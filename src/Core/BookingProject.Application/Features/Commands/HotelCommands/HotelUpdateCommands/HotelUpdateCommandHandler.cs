using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Helpers.Extensions;
using BookingProject.Application.Repositories;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BookingProject.Application.Features.Commands.HotelCommands.HotelUpdateCommands
{
    public class HotelUpdateCommandHandler : IRequestHandler<HotelUpdateCommandRequest, HotelUpdateCommandResponse>
    {
        private readonly IHotelRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHotelImageRepository _hotelImageRepository;
        private readonly IStaffLanguageRepository _staffLanguageRepository;
        private readonly IHotelStaffLanguageRepository _hotelStaffLanguageRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IHotelServiceRepository _hotelServiceRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly IHotelPaymentMethodRepository _hotelPaymentMethodRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IHotelActivityRepository _hotelActivityRepository;
        private readonly IConfiguration _configuration;
        private readonly IRoomService _roomService;
		private readonly ITypeRepository _typeRepository;
		private readonly ICountryRepository _countryRepository;

		public HotelUpdateCommandHandler(IHotelRepository repository,
            IMapper mapper,
            IHotelImageRepository hotelImageRepository,
            IStaffLanguageRepository staffLanguageRepository,
            IHotelStaffLanguageRepository hotelStaffLanguageRepository,
            IServiceRepository serviceRepository,
            IHotelServiceRepository hotelServiceRepository,
            IPaymentMethodRepository paymentMethodRepository,
            IHotelPaymentMethodRepository hotelPaymentMethodRepository,
            IActivityRepository activityRepository,
			ICountryRepository countryRepository,
            IHotelActivityRepository hotelActivityRepository,
            IConfiguration configuration,
            IRoomService roomService,
			ITypeRepository typeRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _hotelImageRepository = hotelImageRepository;
            _staffLanguageRepository = staffLanguageRepository;
            _hotelStaffLanguageRepository = hotelStaffLanguageRepository;
            _serviceRepository = serviceRepository;
            _hotelServiceRepository = hotelServiceRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _hotelPaymentMethodRepository = hotelPaymentMethodRepository;
            _activityRepository = activityRepository;
            _hotelActivityRepository = hotelActivityRepository;
            _configuration = configuration;
            _roomService = roomService;
			_typeRepository = typeRepository;
			_countryRepository = countryRepository;
		}

        public async Task<HotelUpdateCommandResponse> Handle(HotelUpdateCommandRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new NotFoundException("Request not found");

			Hotel hotel = await _repository.Table
			 .Include(x => x.HotelImages)
			 .Include(x => x.HotelAdvantages)
			 .Include(x => x.HotelActivities)
			 .ThenInclude(x => x.Activity)
			 .Include(x => x.HotelPaymentMethods)
			 .ThenInclude(x => x.PaymentMethod)
			 .Include(x => x.HotelServices)
			 .ThenInclude(x => x.Service)
			 .Include(x => x.HotelStaffLanguages)
			 .ThenInclude(x => x.StaffLanguage)
			 .Include(x => x.Type)
			 .Include(x => x.Country)
			 .FirstOrDefaultAsync(x => x.Id == request.Id);
			if (hotel is null)
                throw new NotFoundException($"Hotel with ID {request.Id} not found");
			if (!await _typeRepository.Table.AnyAsync(x => x.Id == request.TypeId && x.IsDeactive==false))
				throw new NotFoundException("Type not found");
			if (!await _countryRepository.Table.AnyAsync(x => x.Id == request.CountryId && x.IsDeactive==false))
				throw new NotFoundException("Country not found");
			List<int> newLanguageIds = request.StaffLanguageIds?.Except(hotel.HotelStaffLanguages?.Select(hotelLanguage => hotelLanguage.StaffLanguage.Id) ?? Enumerable.Empty<int>()).ToList() ?? new List<int>();

			List<int> deletedLanguageIds = hotel.HotelStaffLanguages?.Select(hotelLanguage => hotelLanguage.StaffLanguage.Id)?.Except(request.StaffLanguageIds ?? Enumerable.Empty<int>())?.ToList() ?? new List<int>();

			List<int> newActivityIds = request.ActivityIds?.Except(hotel.HotelActivities?.Select(hotelActivity => hotelActivity.Activity.Id) ?? Enumerable.Empty<int>()).ToList() ?? new List<int>();

			List<int> deletedActivityIds = hotel.HotelActivities?.Select(hotelActivity => hotelActivity.Activity.Id)?.Except(request.ActivityIds ?? Enumerable.Empty<int>())?.ToList() ?? new List<int>();

			List<int> newPaymentMethodIds = request.PaymentMethodIds?.Except(hotel.HotelPaymentMethods?.Select(hotelPaymentMethod => hotelPaymentMethod.PaymentMethod.Id) ?? Enumerable.Empty<int>()).ToList() ?? new List<int>();

			List<int> deletedPaymentMethodIds = hotel.HotelPaymentMethods?.Select(hotelPaymentMethod => hotelPaymentMethod.PaymentMethod.Id)?.Except(request.PaymentMethodIds ?? Enumerable.Empty<int>())?.ToList() ?? new List<int>();

			List<int> newServiceIds = request.ServiceIds?.Except(hotel.HotelServices?.Select(hotelService => hotelService.Service.Id) ?? Enumerable.Empty<int>()).ToList() ?? new List<int>();

			List<int> deletedServiceIds = hotel.HotelServices?.Select(hotelService => hotelService.Service.Id)?.Except(request.ServiceIds ?? Enumerable.Empty<int>())?.ToList() ?? new List<int>();

			foreach (var id in deletedActivityIds)
			{
				HotelActivity activity= await _hotelActivityRepository.Table.FirstOrDefaultAsync(x=>x.HotelId==hotel.Id && x.ActivityId==id);
				if (activity is null) throw new NotFoundException("Activity not found in hotel");
				_hotelActivityRepository.Delete(activity);
			}
			foreach (var id in deletedLanguageIds)
			{
				HotelStaffLanguage lang = await _hotelStaffLanguageRepository.Table.FirstOrDefaultAsync(x => x.HotelId == hotel.Id && x.StaffLanguageId == id);
				if (lang is null) throw new NotFoundException("Language not found in hotel");
				_hotelStaffLanguageRepository.Delete(lang);
			}
			foreach (var id in deletedPaymentMethodIds)
			{
				HotelPaymentMethod payment = await _hotelPaymentMethodRepository.Table.FirstOrDefaultAsync(x => x.HotelId == hotel.Id && x.PaymentMethodId == id);
				if (payment is null) throw new NotFoundException("Payment method not found in hotel");
				_hotelPaymentMethodRepository.Delete(payment);
			}
			foreach (var id in deletedServiceIds)
			{
				HotelService service = await _hotelServiceRepository.Table.FirstOrDefaultAsync(x => x.HotelId == hotel.Id && x.ServiceId == id);
				if (service is null) throw new NotFoundException("Service not found in hotel");
				_hotelServiceRepository.Delete(service);
			}
			foreach (var activityId in newActivityIds)
			{
				var activity = await _activityRepository.Table.Where(x => x.IsDeactive == false).FirstOrDefaultAsync(x => x.Id == activityId);
				if (activity == null)
					throw new NotFoundException($"Activity with id {activityId} not found");
				if (activity.IsDeactive == true)
					throw new NotFoundException("Activity not found");
				HotelActivity hotelAct = new HotelActivity()
				{
					Hotel = hotel,
					ActivityId = activityId,
					IsDeactive = request.IsDeactive
				};
				await _hotelActivityRepository.CreateAsync(hotelAct);
			}
			foreach (var paymentMethodId in newPaymentMethodIds)
			{
				var paymentMethod = await _paymentMethodRepository.Table.Where(x => x.IsDeactive == false).FirstOrDefaultAsync(x => x.Id == paymentMethodId);

				if (paymentMethod == null)
					throw new NotFoundException($"Payment Method with id {paymentMethodId} not found");
				if (paymentMethod.IsDeactive == true)
					throw new NotFoundException("Payment Method not found");
				HotelPaymentMethod paymMethod = new HotelPaymentMethod()
				{
					Hotel = hotel,
					PaymentMethodId = paymentMethodId,
					IsDeactive = request.IsDeactive
				};
				await _hotelPaymentMethodRepository.CreateAsync(paymMethod);
			}
			foreach (var serviceId in newServiceIds)
			{
				var service = await _serviceRepository.Table.Where(x => x.IsDeactive == false).FirstOrDefaultAsync(x => x.Id == serviceId);

				if (service == null)
					throw new NotFoundException($"Service with id {serviceId} not found");
				if (service.IsDeactive == true)
					throw new NotFoundException("Service not found");

				HotelService hotelServ = new HotelService()
				{
					Hotel = hotel,
					ServiceId = serviceId,
					IsDeactive = request.IsDeactive
				};
				await _hotelServiceRepository.CreateAsync(hotelServ);
			}
			SaveFileExtension.Initialize(_configuration);
			foreach (var id in request.DeletedImageFileIds)
			{
				HotelImage img = await _hotelImageRepository.Table.FirstOrDefaultAsync(x => x.HotelId == hotel.Id && x.Id == id);
				if (img is null) throw new NotFoundException("Image not found in hotel");
				await SaveFileExtension.DeleteFileAsync(img.Url);
				_hotelImageRepository.Delete(img);
			}
			foreach (var languageId in newLanguageIds)
			{
				var language = await _staffLanguageRepository.Table.Where(x => x.IsDeactive == false).FirstOrDefaultAsync(x => x.Id == languageId);
				if (language == null)
					throw new NotFoundException($"Staff Language with id {languageId} not found");
				if (language.IsDeactive == true)
					throw new NotFoundException("Language not found");

				HotelStaffLanguage newHotelLang = new HotelStaffLanguage()
				{
					Hotel = hotel,
					StaffLanguage = language,
					IsDeactive = request.IsDeactive
				};
				await _hotelStaffLanguageRepository.CreateAsync(newHotelLang);
			}
			foreach (var image in request.ImageFiles)
			{
				if (image is null)
					throw new NotFoundException($"Image not found");
				string url = await SaveFileExtension.SaveFile(image, "hotels");
				HotelImage hotelImg = new()
				{
					Hotel = hotel,
					Url = url,
					IsDeactive = request.IsDeactive
				};
				await _hotelImageRepository.CreateAsync(hotelImg);
			}
			hotel.ModifiedDate=DateTime.Now;
		    hotel=_mapper.Map(request, hotel);
			await _repository.CommitAsync();
			return new HotelUpdateCommandResponse();
        }
    }
}
