﻿using System;
using Unity.Services.Lobbies;

namespace TTLobbyLogic
{
    public class AsyncRequestLobby : AsyncRequest
    {
        private static AsyncRequestLobby s_instance;
        
        public static AsyncRequestLobby Instance
        {
            get
            {   if (s_instance == null)
                    s_instance = new AsyncRequestLobby();
                return s_instance;
            }
        }
        
        /// The Lobby service will wrap HTTP errors in LobbyServiceExceptions. We can filter on LobbyServiceException.Reason for custom behavior.
        protected override void ParseServiceException(Exception e)
        {
            if (!(e is LobbyServiceException))
            {
                return;
            }
            var lobbyEx = e as LobbyServiceException;
            // We have other ways of preventing players from hitting the rate limit, so the developer-facing 429 error is sufficient here.
            if (lobbyEx.Reason == LobbyExceptionReason.RateLimited) 
            {
                return;
            }
            
            // Lobby error type, then HTTP error type.
            Locator.Get.Messenger.OnReceiveMessage(MessageType.DisplayErrorPopup, $"Lobby Error: {lobbyEx.Message} ({lobbyEx.InnerException.Message})");
        }
    }
}