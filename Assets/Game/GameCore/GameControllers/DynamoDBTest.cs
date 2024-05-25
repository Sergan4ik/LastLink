using System;
using System.Collections.Generic;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using ZeroLag.MultiplayerTools.Modules.Authentication;
using ZeroLag.MultiplayerTools.Modules.Database;

namespace Game.GameCore.GameControllers
{
    public class DynamoDBTest : ConnectableMonoBehaviour
    {
        AWSCognitoAuthentication awsAuth;
        BaseAccessToken accessToken;
        
        DynamoDBPlayerDatabase<RTSPlayerData, RTSPlayerCustomData> playerDatabase;

        private void Start()
        {
            playerDatabase = new DynamoDBPlayerDatabase<RTSPlayerData, RTSPlayerCustomData>(
                AWS_CONFIG.AWS_ACCESS_KEY_ID, 
                AWS_CONFIG.AWS_SECRET_KEY, 
                AWS_CONFIG.AWS_REGION,
                AWS_CONFIG.PLAYER_DATA_TABLE_NAME);
            
            awsAuth = new AWSCognitoAuthentication(
                AWS_CONFIG.AWS_ACCESS_KEY_ID,
                AWS_CONFIG.AWS_SECRET_KEY,
                AWS_CONFIG.AWS_REGION,
                AWS_CONFIG.PLAYER_DATA_TABLE_NAME);

            connections += awsAuth.onRegisteredPlayer.Subscribe(async p =>
            {
                Debug.Log($"Registered player: {p.userId}");
                await playerDatabase.AddPlayer(p.playerId, new RTSPlayerData()
                {
                    playerId = p.playerId,
                    username = p.userId,
                    customData = new RTSPlayerCustomData()
                    {
                        level = 1,
                        playerAvatar = "avatar1"
                    }
                });
            });
        }

        [Button]
        public async void Register(string username, string password, long id)
        {
            BaseSignUpCredentials credentials = new BaseSignUpCredentials()
            {
                userId = username,
                password = password,
                playerId = id
            };
            var res = await awsAuth.Register(credentials);
            Debug.Log($"Register result: {res}");
        }

        [Button]
        public async void Login(string username, string password)
        {
            BaseCredentials credentials = new BaseCredentials()
            {
                userId = username,
                password = password
            };
            accessToken = await awsAuth.Authenticate(credentials);
            Debug.Log($"Login result: {accessToken.token}");
            GetUser(accessToken);
        }

        public async void GetUser(IAccessToken token)
        {
            var res = await awsAuth.GetUser<AWSUserData>(token);
            Debug.Log($"User: {res.userName}, {res.playerId}");
        }
        
        [Button]
        public async void Logout()
        {
            var res = await awsAuth.Logout(accessToken);
            Debug.Log($"Logout result: {res}");
        }
        
        [Button]
        private async void Test(int id, string username)
        {
            var player = new RTSPlayerData()
            {
                playerId = id,
                username = username,
                customData = new RTSPlayerCustomData()
                {
                    level = 1,
                    playerAvatar = "avatar1"
                }
            };
            await playerDatabase.AddPlayer(id, player);
            var res = await playerDatabase.GetPlayer(id);
            Debug.Log($"Player: {res.playerId}, {res.username}, {res.customData.level}, {res.customData.playerAvatar}");
        }
    }
}