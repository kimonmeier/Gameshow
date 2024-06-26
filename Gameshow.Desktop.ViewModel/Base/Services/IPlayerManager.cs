﻿namespace Gameshow.Desktop.ViewModel.Base.Services;

public interface IPlayerManager
{
    Guid PlayerId { get; set; }
    
    IEnumerable<Guid> Players { get; }

    void RegisterPlayer(Guid playerGuid, string name, string link);

    void RemovePlayer(Guid playerId);
}