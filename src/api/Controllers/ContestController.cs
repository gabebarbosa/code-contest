namespace CodeContest.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using CodeContest.Api.Contracts;
    using CodeContest.Core.Domain.Model;
    using CodeContest.Core.Provider;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using MongoDB.Driver;

    /// <summary>
    /// The contest controller.
    /// </summary>
    [Route("api/v1/contests")]
    public class ContestController : ControllerBase
    {
        /// <summary>
        /// The contest provider.
        /// </summary>
        private readonly ContestProvider contestProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="ContestController" />.
        /// </summary>
        public ContestController(IConfiguration configuration)
        {
            this.contestProvider = new ContestProvider(configuration);
        }

        /// <summary>
        /// Adds a new contest.
        /// </summary>
        /// <param name="contestContract">The contest contract.</param>
        /// <returns>The id of new contest.</returns>
        [HttpPost("")]
        public Guid Post([FromBody]ContestContract contestContract)
        {
            var contest = new Contest();
            contest.Id = Guid.NewGuid();
            contest.Title = contestContract.Title;
            contest.Description = contestContract.Description;
            contest.StartupCode = contestContract.StartupCode ?? new List<StartupCode>();
            contest.Active = false;

            this.contestProvider.Entities.InsertOne(contest);

            return contest.Id;
        }

        /// <summary>
        /// Gets all active contests.
        /// </summary>
        /// <returns>All active contests.</returns>

        [HttpGet("actives")]
        public IEnumerable<Contest> GetActives()
        {
            return this.contestProvider.Entities.FindSync(c => c.Active).ToList();
        }

        /// <summary>
        /// Enable or disable the contest.
        /// </summary>
        /// <param name="id">The contest id.</param>
        /// <param name="enable">The value that indicates if contest will be active.</param>
        [HttpPatch("{id}")]
        public void Activate(Guid id, bool enable)
        {
            this.contestProvider.Entities.FindOneAndUpdate(
            c => c.Id.Equals(id),
            Builders<Contest>.Update.Set(c => c.Active, enable));
        }
    }
}
