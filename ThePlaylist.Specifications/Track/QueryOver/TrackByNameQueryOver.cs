namespace ThePlaylist.Specifications.Track.QueryOver;

public class TrackByNameQueryOver(string trackName)
    : QueryOverSpecification<Core.Entitites.Track>(queryOver => queryOver.Where(x => x.Name == trackName));