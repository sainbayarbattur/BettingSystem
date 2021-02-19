﻿namespace BettingSystem.Application.Competitions.Teams.Handlers
{
    using System.Threading.Tasks;
    using Common;
    using Domain.Competitions.Repositories;
    using Domain.Matches.Events;

    public class MatchFinishedEventHandler : IEventHandler<MatchFinishedEvent>
    {
        private readonly ITeamDomainRepository teamRepository;

        public MatchFinishedEventHandler(ITeamDomainRepository teamRepository)
            => this.teamRepository = teamRepository;

        public Task Handle(MatchFinishedEvent domainEvent)
            => this.teamRepository.GiveTeamPoints(
                domainEvent.HomeTeamId,
                domainEvent.AwayTeamId,
                domainEvent.HomeScore,
                domainEvent.AwayScore);
    }
}