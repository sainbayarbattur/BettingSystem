﻿namespace BettingSystem.Infrastructure.Persistence.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Features.Matches;
    using Application.Features.Matches.Queries.Details;
    using Application.Features.Matches.Queries.Stadiums;
    using AutoMapper;
    using Domain.Models.Matches;
    using Domain.Models.Teams;
    using Microsoft.EntityFrameworkCore;

    internal class MatchRepository : DataRepository<Match>, IMatchRepository
    {
        private readonly IMapper mapper;

        public MatchRepository(BettingDbContext db, IMapper mapper)
            : base(db)
            => this.mapper = mapper;

        public async Task<bool> Delete(
            int id,
            CancellationToken cancellationToken = default)
        {
            var match = await this.FindById(id, cancellationToken);

            if (match == null)
            {
                return false;
            }

            this.Data.Remove(match);

            await this.Data.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<Match> Find(
            int id,
            CancellationToken cancellationToken = default)
            => await this.FindById(id, cancellationToken);

        public async Task<MatchDetailsResponseModel> GetDetails(
            int id,
            CancellationToken cancellationToken = default)
            => await this.mapper
                .ProjectTo<MatchDetailsResponseModel>(this
                    .AllAsNoTracking()
                    .Where(c => c.Id == id))
                .FirstOrDefaultAsync(cancellationToken);

        public async Task<Team> GetHomeTeam(
            string homeTeam,
            CancellationToken cancellationToken = default)
            => await this
                .AllTeams()
                .FirstOrDefaultAsync(c => c.Name == homeTeam, cancellationToken);

        public async Task<Team> GetAwayTeam(
            string awayTeam,
            CancellationToken cancellationToken = default)
            => await this
                .AllTeams()
                .FirstOrDefaultAsync(m => m.Name == awayTeam, cancellationToken);

        public async Task<Stadium> GetStadium(
            string stadium,
            CancellationToken cancellationToken = default)
            => await this
                .AllStadiums()
                .FirstOrDefaultAsync(m => m.Name == stadium, cancellationToken);

        public async Task<IEnumerable<GetMatchStadiumResponseModel>> GetStadiums(
            CancellationToken cancellationToken = default)
            => await this.mapper
                .ProjectTo<GetMatchStadiumResponseModel>(this
                    .AllStadiums())
                .ToListAsync(cancellationToken);

        private async Task<Match> FindById(
            int id,
            CancellationToken cancellationToken = default)
            => await this
                .All()
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        private IQueryable<Team> AllTeams()
            => this.Data.Teams;

        private IQueryable<Stadium> AllStadiums()
            => this.Data.Stadiums;
    }
}