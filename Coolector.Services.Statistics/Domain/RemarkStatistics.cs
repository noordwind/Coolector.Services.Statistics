using System;
using System.Collections.Generic;
using Coolector.Common.Domain;

namespace Coolector.Services.Statistics.Domain
{
    public class RemarkStatistics : IdentifiableEntity
    {
        private ISet<string> _tags = new HashSet<string>();
        public Guid RemarkId { get; protected set; }
        public string Category { get; protected set; }
        public RemarkUser Author { get; protected set; }
        public RemarkLocation Location { get; protected set; }
        public string Description { get; protected set; }
        public RemarkUser Resolver { get; protected set; }
        public string State { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? ResolvedAt { get; protected set; }
        public DateTime? DeletedAt { get; protected set; }
        public IList<VoteStatistics> Votes { get; protected set; }

        public IEnumerable<string> Tags
        {
            get { return _tags; }
            protected set { _tags = new HashSet<string>(value); }
        }

        protected RemarkStatistics()
        {
        }

        public RemarkStatistics(Guid remarkId, string category,
            string authorId, string authorName, DateTime createdAt,
            double latitude, double longitude, string address = null,
            string description = null, IEnumerable<string> tags = null)
        {
            RemarkId = remarkId;
            Category = category;
            Author = new RemarkUser(authorId, authorName);
            CreatedAt = createdAt;
            Location = new RemarkLocation(latitude, longitude, address);
            Description = description;
            Tags = tags ?? new HashSet<string>();
            Votes = new List<VoteStatistics>();
            State = RemarkState.Created;
        }

        public void SetResolved(string resolverId, string resolverName,
            DateTime resolvedAt)
        {
            Resolver = new RemarkUser(resolverId, resolverName);
            ResolvedAt = resolvedAt;
            State = RemarkState.Resolved;
        }

        public void SetDeleted()
        {
            DeletedAt = DateTime.UtcNow;
            State = RemarkState.Deleted;
        }

        public void AddVote(VoteStatistics vote)
        {
            if (Votes == null)
                Votes = new List<VoteStatistics>();

            Votes.Add(vote);
        }

        public class RemarkUser 
        {
            public string Id { get; protected set;}
            public string Name { get; protected set;}

            protected RemarkUser()
            {
            }

            public RemarkUser(string id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        public class RemarkLocation
        {
            public double Longitude => Coordinates[0];
            public double Latitude => Coordinates[1];
            public double[] Coordinates { get; protected set; }
            public string Type { get; protected set; }
            public string Address { get; protected set; }

            protected RemarkLocation()
            {
            }

            public RemarkLocation(double latitude, double longitude, string address = null)
            {
                if (latitude > 90 || latitude < -90)
                {
                    throw new ArgumentException($"Invalid latitude {latitude}", nameof(latitude));
                }
                if (longitude > 180 || longitude < -180)
                {
                    throw new ArgumentException($"Invalid longitude {longitude}", nameof(longitude));
                }

                Type = "Point";
                Coordinates = new[] { longitude, latitude };
                Address = address;
            }
        }

        private static class RemarkState
        {
            public static string Created => "created";
            public static string Resolved => "resolved";
            public static string Deleted => "deleted";
        }
    }
}