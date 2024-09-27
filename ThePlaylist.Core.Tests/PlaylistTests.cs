using FluentAssertions;
using ThePlaylist.Core.Entitites;

namespace ThePlaylist.Core.Tests;

public class PlaylistTests
{
    [Test]
    public void CreateRevision()
    {
        var playlist = new Playlist();
        playlist.Name = "Init";
        playlist.CreateRevision(current => current.Name = "updated");
        playlist.Revisions.Should().HaveCount(1);
        playlist.Revision.Should().Be(1);
        playlist.Revisions.First().Revision.Should().Be(0);
        playlist.Name.Should().Be("updated");
        playlist.Revisions.First().Name.Should().Be("Init");
    }
}