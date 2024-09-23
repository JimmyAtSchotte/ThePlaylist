using FluentAssertions;
using ThePlaylist.Specifications;
using ThePlaylist.Specifications.Entitites.Playlist.Query;
using ThePlaylist.Specifications.Entitites.Track.Criteria;

namespace ThePlaylist.Infrastructure.Tests;


[TestFixture]
public class SpecificationComparisson
{
    [Test]
    public void Equals()
    {
        var spec1 = Specs.Playlist.ByName("Test");
        var spec2 = Specs.Playlist.ByName("Test");

        spec1.Equals(spec2).Should().BeTrue();
    }
    
    [Test]
    public void NotEquals()
    {
        var spec1 = Specs.Playlist.ByName("TestA");
        var spec2 = Specs.Playlist.ByName("TestB");

        spec1.Equals(spec2).Should().BeFalse();
    }
    
    [Test]
    public void Equals2()
    {
        var spec1 = Specs.Track.TrackByNameCriteria("Test");
        var spec2 = Specs.Track.TrackByNameCriteria("Test");

        spec1.Equals(spec2).Should().BeTrue();
    }
}