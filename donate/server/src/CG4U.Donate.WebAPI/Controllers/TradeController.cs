using System.Collections.Generic;
using AutoMapper;
using CG4U.Core.Services.Controllers;
using CG4U.Donate.Domain.Trades.Commands;
using CG4U.Donate.Domain.Trades.Models;
using CG4U.Donate.Domain.Trades.Repository;
using CG4U.Donate.WebAPI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Notifications;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Services.ViewModels;
using CG4U.Core.Services.Interfaces;
using CG4U.Donate.Domain.Donations;
using CG4U.Donate.Domain.Trades;

namespace CG4U.Donate.WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = "Admin,UserDonate")]    
    public class TradeController : BaseController
    {
        private readonly ITradeRepository _tradeRepository;

        public TradeController(IMediatorHandler mediator,
                               ITradeRepository tradeRepository,
                               UserManager<IdentityUser> userManager,
                               IUserAdapter userAdapter,
                               INotificationHandler<DomainNotification> notifications,
                               IStringLocalizer<TradeController> localizer,
                               ILogger<TradeController> logger)
            : base(notifications, userManager, userAdapter, mediator, localizer, logger)
        {
            _tradeRepository = tradeRepository;
        }

        [HttpGet("{idDonationsGivens:int}/{idSystems:int}/{idLanguages:int}/{maxDistanceInMeters}")]
        //[Authorize(Policy = "Trade.ListMatchDesiredsByPositionAsync")]
        public async Task<IEnumerable<DesiredViewModel>> ListMatchDesiredsByPositionAsync(int idDonationsGivens, int idSystems, int idLanguages, double maxDistanceInMeters)
        {
            var desireds = await _tradeRepository.ListMatchDesiredsByPositionAsync(idDonationsGivens, idSystems, idLanguages, maxDistanceInMeters);
            return Mapper.Map<IEnumerable<Desired>, IEnumerable<DesiredViewModel>>(desireds);
        }

        [HttpGet("{idDonationsDesired:int}/{idSystems:int}/{idLanguages:int}/{maxDistanceInMeters}")]
        //[Authorize(Policy = "Trade.ListMatchGivensByPositionAsync")]
        public async Task<IEnumerable<GivenViewModel>> ListMatchGivensByPositionAsync(int idDonationsDesired, int idSystems, int idLanguages, double maxDistanceInMeters)
        {
            var givens = await _tradeRepository.ListMatchGivensByPositionAsync(idDonationsDesired, idSystems, idLanguages, maxDistanceInMeters);
            return Mapper.Map<IEnumerable<Given>, IEnumerable<GivenViewModel>>(givens);
        }

        [HttpPost]
        //[Authorize(Policy = "Trade.Add")]
        public async Task<IActionResult> AddTradeAsync([FromBody]TradeViewModel tradeViewModel)
        {
            if (!IsModelStateValid()) return Response();

            var usersRequest = await GetUsersRequestAsync(HttpContext, tradeViewModel.UserGet.IdUserIdentity, tradeViewModel.UserLet.IdUserIdentity);
            if (usersRequest == null) return Response();

            var domainModel = Mapper.Map<TradeModel>(tradeViewModel);
            domainModel.UserGet = usersRequest.ListUsersViewModel[0];
            domainModel.UserLet = usersRequest.ListUsersViewModel[1];

            var cmd = new AddTradeCommand(usersRequest.UserLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpGet("{id:int}/{idSystems:int}/{idLanguages:int}")]
        //[Authorize(Policy = "Trade.GetById")]
        public async Task<TradeViewModel> GetTradeByIdSystemLanguageAsync(int id, int idSystems, int idLanguages)
        {
            var trade = await _tradeRepository.GetByIdSystemLanguageAsync(id, idSystems, idLanguages);
            return Mapper.Map<Trade, TradeViewModel>(trade);
        }

        [HttpPost]
        //[Authorize(Policy = "Trade.Update")]
        public async Task<IActionResult> UpdateTradeAsync([FromBody]TradeViewModel tradeViewModel)
        {
            if (!IsModelStateValid()) return Response();

            var usersRequest = await GetUsersRequestAsync(HttpContext, tradeViewModel.UserGet.IdUserIdentity, tradeViewModel.UserLet.IdUserIdentity);
            if (usersRequest == null) return Response();

            var domainModel = Mapper.Map<TradeModel>(tradeViewModel);
            domainModel.UserGet = usersRequest.ListUsersViewModel[0];
            domainModel.UserLet = usersRequest.ListUsersViewModel[1];

            var cmd = new UpdateTradeCommand(usersRequest.UserLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpPost]
        //[Authorize(Policy = "Trade.Disable")]
        public async Task<IActionResult> DisableTradeAsync([FromBody] int id)
        {
            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var cmd = new DisableTradeCommand(userLoggedIn, id);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpGet("{id:int}/{idSystems:int}/{idLanguages:int}")]
        //[Authorize(Policy = "Trade.ListByUserGetSystemLanguage")]
        public async Task<IEnumerable<TradeViewModel>> ListByUserGetSystemLanguageAsync(int id, int idSystems, int idLanguages)
        {
            var trades = await _tradeRepository.ListByUserGetSystemLanguageAsync(id, idSystems, idLanguages);
            return Mapper.Map<IEnumerable<TradeViewModel>>(trades);
        }

        [HttpGet("{id:int}/{idSystems:int}/{idLanguages:int}")]
        //[Authorize(Policy = "Trade.ListByUserLetSystemLanguage")]
        public async Task<IEnumerable<TradeViewModel>> ListByUserLetSystemLanguageAsync(int id, int idSystems, int idLanguages)
        {
            var trades = await _tradeRepository.ListByUserLetSystemLanguageAsync(id, idSystems, idLanguages);
            return Mapper.Map<IEnumerable<TradeViewModel>>(trades);
        }

        [HttpPost]
        //[Authorize(Policy = "TradeLocation.AddTradeLocation")]
        public async Task<IActionResult> AddTradeLocationAsync([FromBody]LocationViewModel locationViewModel)
        {
            if (!IsModelStateValid()) return Response();

            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var domainModel = Mapper.Map<TradeLocationModel>(locationViewModel);
                
            var cmd = new AddTradeLocationCommand(userLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpGet]
        //[Authorize(Policy = "TradeLocation.ListTradeLocationsByIdTrades")]
        public async Task<IEnumerable<LocationViewModel>> ListTradeLocationsByIdTradesAsync([FromBody]int idTrades)
        {
            return Mapper.Map<IEnumerable<LocationViewModel>>(await _tradeRepository.ListTradeLocationsByIdTradesAsync(idTrades));
        }

        [HttpPost]
        //[Authorize(Policy = "TradeLocation.UpdateTradeLocation")]
        public async Task<IActionResult> UpdateTradeLocationAsync([FromBody]LocationViewModel locationViewModel)
        {
            if (!IsModelStateValid()) return Response();

            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var domainModel = Mapper.Map<TradeLocationModel>(locationViewModel);
                
            var cmd = new UpdateTradeLocationCommand(userLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpPost]
        //[Authorize(Policy = "TradeLocation.DisableTradeLocation")]
        public async Task<IActionResult> DisableTradeLocationAsync([FromBody]int idTradeLocations)
        {
            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var cmd = new DisableTradeLocationCommand(userLoggedIn, idTradeLocations);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpPost]
        //[Authorize(Policy = "TradeEvaluation.Add")]
        public async Task<IActionResult> AddTradeEvaluationAsync([FromBody]EvaluationViewModel evaluationViewModel)
        {
            if (!IsModelStateValid()) return Response();

            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var domainModel = Mapper.Map<TradeEvaluationModel>(evaluationViewModel);
                
            var cmd = new AddTradeEvaluationCommand(userLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpGet]
        //[Authorize(Policy = "TradeEvaluation.ListTradeEvaluationsByIdTrades")]
        public async Task<IEnumerable<EvaluationViewModel>> ListTradeEvaluationsByIdTradesAsync([FromBody]int idTrades)
        {
            return Mapper.Map<IEnumerable<EvaluationViewModel>>(await _tradeRepository.ListTradeEvaluationsByIdTradesAsync(idTrades));
        }

        [HttpPost]
        //[Authorize(Policy = "TradeEvaluation.UpdateTradeEvaluation")]
        public async Task<IActionResult> UpdateTradeEvaluationAsync([FromBody]EvaluationViewModel evaluationViewModel)
        {
            if (!IsModelStateValid()) return Response();

            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var domainModel = Mapper.Map<TradeEvaluationModel>(evaluationViewModel);

            var cmd = new UpdateTradeEvaluationCommand(userLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }
    }
}
