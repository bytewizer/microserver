using System;
using System.Text.RegularExpressions;

namespace MicroServer.Net.Http.Routing
{
    /// <summary>
    /// Implementation of <see cref="MatchResult"/>
    /// </summary>
    public class MatchResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchResult"/> class.
        /// </summary>
        /// <param name="matchStatus"></param>
        /// <param name="mappedRoute"></param>
        public MatchResult(Match matchStatus, MappedRoute mappedRoute)
        {
            MatchStatus = matchStatus;
            MappedRoute = mappedRoute;
        }
        
        /// <summary>
        /// Gets match status resulting from <see cref="RouteCollection.Match"/>.
        /// </summary>
        public Match MatchStatus { get; set; }

        /// <summary>
        /// Gets match status resulting from <see cref="RouteCollection.Match"/>.
        /// </summary>
        public MappedRoute MappedRoute { get; set; }
    }
}
