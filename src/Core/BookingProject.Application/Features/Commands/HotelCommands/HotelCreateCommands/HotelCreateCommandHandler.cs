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
using Microsoft.IdentityModel.Tokens;

namespace BookingProject.Application.Features.Commands.HotelCommands.HotelCreateCommands
{
	public class HotelCreateCommandHandler : IRequestHandler<HotelCreateCommandRequest, HotelCreateCommandResponse>
    {
        private readonly IHotelRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAdvantageRepository _advantageRepository;
        private readonly IStaffLanguageRepository _staffLanguageRepository;
        private readonly IHotelStaffLanguageRepository _hotelStaffLanguageRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IHotelServiceRepository _hotelServiceRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly IHotelPaymentMethodRepository _hotelPaymentMethodRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IHotelActivityRepository _hotelActivityRepository;
        private readonly IHotelImageRepository _hotelImageRepository;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
		private readonly IRoomService _roomService;
		private readonly ITypeRepository _typeRepository;
		private readonly ICountryRepository _countryRepository;

		public HotelCreateCommandHandler(IHotelRepository repository,
            IMapper mapper,
            IAdvantageRepository advantageRepository,
            IStaffLanguageRepository staffLanguageRepository,
            IHotelStaffLanguageRepository hotelStaffLanguageRepository,
            IServiceRepository serviceRepository,
            IHotelImageRepository hotelImageRepository,
            IHotelServiceRepository hotelServiceRepository,
            IPaymentMethodRepository paymentMethodRepository,
            IHotelPaymentMethodRepository hotelPaymentMethodRepository,
            IActivityRepository activityRepository,
            IHotelActivityRepository hotelActivityRepository,
            IConfiguration configuration,
            UserManager<AppUser> userManager,
            IRoomService roomService,
            ICountryRepository countryRepository,
            ITypeRepository typeRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _advantageRepository = advantageRepository;
            _countryRepository = countryRepository;
            _staffLanguageRepository = staffLanguageRepository;
            _hotelStaffLanguageRepository = hotelStaffLanguageRepository;
            _serviceRepository = serviceRepository;
            _hotelServiceRepository = hotelServiceRepository;
            _hotelImageRepository = hotelImageRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _hotelPaymentMethodRepository = hotelPaymentMethodRepository;
            _activityRepository = activityRepository;
            _hotelActivityRepository = hotelActivityRepository;
            _configuration = configuration;
            _userManager = userManager;
			_roomService = roomService;
			_typeRepository = typeRepository;
		}

        public async Task<HotelCreateCommandResponse> Handle(HotelCreateCommandRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new NotFoundException("Request not found");

            if (request.Name.IsNullOrEmpty())
                throw new BadRequestException("Name cannot be null");
            if (!await _typeRepository.Table.AnyAsync(x => x.Id == request.TypeId && x.IsDeactive==false))
                throw new NotFoundException("Type not found");
			if (!await _countryRepository.Table.AnyAsync(x => x.Id == request.CountryId && x.IsDeactive==false))
				throw new NotFoundException("Country not found");
			if (await _repository.Table.AnyAsync(x => x.Name.ToLower() == request.Name.ToLower()))
                throw new BadRequestException("Hotel Name is already exist");
            if (await _userManager.FindByIdAsync(request.AppUserId) is null)
                throw new NotFoundException("User not found");
            var hotel = _mapper.Map<Hotel>(request);
            hotel.IsDeactive = true;
            hotel.IsApproved = false;
            hotel.IsRefused = false;
            SaveFileExtension.Initialize(_configuration);
            foreach (var image in request.ImageFiles)
            {
                if (image is null)
                    throw new NotFoundException($"Image not found");
                string url= await SaveFileExtension.SaveFile(image, "hotels");
                HotelImage hotelImg = new()
                {
                    Hotel = hotel,
                    Url = url,
                    IsDeactive=true
                };
                await _hotelImageRepository.CreateAsync(hotelImg);
            }

            foreach (var languageId in request.StaffLanguageIds)
            {
                var language = await _staffLanguageRepository.Table.Where(x=>x.IsDeactive==false).FirstOrDefaultAsync(x=>x.Id==languageId);
                if (language == null)
                    throw new NotFoundException($"Staff Language with id {languageId} not found");
                if (language.IsDeactive == true) 
                    throw new NotFoundException("Language not found");

                HotelStaffLanguage newHotelLang = new HotelStaffLanguage()
                {
                    Hotel = hotel,
                    StaffLanguage = language,
                    IsDeactive=true
                };
                await _hotelStaffLanguageRepository.CreateAsync(newHotelLang);
            }

            foreach (var serviceId in request.ServiceIds)
            {
                var service = await _serviceRepository.Table.Where(x => x.IsDeactive == false).FirstOrDefaultAsync(x => x.Id == serviceId);

                if (service == null)
                    throw new NotFoundException($"Service with id {serviceId} not found");
                if (service.IsDeactive == true)
                    throw new NotFoundException("Service not found");

                HotelService hotelServ=new HotelService()
                {
                    Hotel=hotel,
                    ServiceId = serviceId,
                    IsDeactive=true 
                };
                await _hotelServiceRepository.CreateAsync(hotelServ);
            }
            
            foreach (var paymentMethodId in request.PaymentMethodIds)
            {
                var paymentMethod = await _paymentMethodRepository.Table.Where(x => x.IsDeactive == false).FirstOrDefaultAsync(x => x.Id == paymentMethodId);

                if (paymentMethod == null)
                    throw new NotFoundException($"Payment Method with id {paymentMethodId} not found");
                if (paymentMethod.IsDeactive == true)
                    throw new NotFoundException("Payment Method not found");
                HotelPaymentMethod paymMethod=new HotelPaymentMethod()
                {
                    Hotel = hotel,
                    PaymentMethodId = paymentMethodId,
                    IsDeactive=true
                };
                await _hotelPaymentMethodRepository.CreateAsync(paymMethod);
            }

            foreach (var activityId in request.ActivityIds)
            {
                var activity = await _activityRepository.Table.Where(x => x.IsDeactive == false).FirstOrDefaultAsync(x => x.Id == activityId);
                if (activity == null)
                    throw new NotFoundException($"Activity with id {activityId} not found");
                if (activity.IsDeactive == true)
                    throw new NotFoundException("Activity not found");
                HotelActivity hotelAct=new HotelActivity()
                {
                    Hotel = hotel,
                    ActivityId = activityId,
                    IsDeactive=true 
                };
                await _hotelActivityRepository.CreateAsync(hotelAct);
            }

            foreach (var advName in request.HotelAdvantageNames)
            {
                if (advName.IsNullOrEmpty())
                    throw new BadRequestException("Advantage Name cannot be null");

                if (await _advantageRepository.Table.AnyAsync(x => x.AdvantageName.ToLower() == advName.ToLower() && x.Hotel == hotel))
                    throw new BadRequestException("Advantage Name is already exist");

                var advantage = new HotelAdvantage
                {
                    Hotel=hotel,
                    AdvantageName = advName,
                    IsDeactive=true
                };

               await _advantageRepository.CreateAsync(advantage);
            }
            foreach (var room in request.RoomCreateDtos)
            {
            await _roomService.CreateAsync(room, hotel);
            }
			await _repository.CreateAsync(hotel);
            await _repository.CommitAsync();

            return new HotelCreateCommandResponse();
        }
    }
}
