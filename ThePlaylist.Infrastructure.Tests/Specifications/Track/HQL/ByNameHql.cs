﻿using FluentAssertions;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;
using ThePlaylist.Specifications;
using ThePlaylist.Specifications.Entitites.Track.HQL;

namespace ThePlaylist.Infrastructure.Tests.Specifications.Track.HQL;

[TestFixture]
public class ByNameHql
{
    [TestCaseSource(typeof(RepositorySources), nameof(RepositorySources.NHibernateOnlyRepositoryProviders))]
    public void FindTrackByName(IRepositorySource repositoryProvider)
    {
        var track = new Core.Entitites.Track
        {
            Name = "Test"
        };
        
        var trackB = new Core.Entitites.Track
        {
            Name = "Test B"
        };

        using var repository = repositoryProvider.CreateRepository();
        repository.Add(track);
        repository.Add(trackB);
        var fetchedTracks = repository.List(Specs.Track.TrackByNameHql(track.Name));
        
        fetchedTracks.Should().Contain(x => x.Id == track.Id);
        fetchedTracks.Should().NotContain(x => x.Id == trackB.Id);
    }
}