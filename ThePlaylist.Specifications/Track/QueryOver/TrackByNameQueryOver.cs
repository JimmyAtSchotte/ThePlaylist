namespace ThePlaylist.Specifications.Track.QueryOver;

public class TrackByNameQueryOver : QueryOverSpecification<Core.Entitites.Track>
{
    public TrackByNameQueryOver(string trackName)
    {
        this.UseQueryOver(queryOver => queryOver.Where(x => x.Name == trackName));
    }
}