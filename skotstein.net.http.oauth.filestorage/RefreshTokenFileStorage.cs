using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.filestorage
{
    public class RefreshTokenFileStorage : IRefreshTokenStorage
    {

        private string _homeDirectory;

        private IList<IRefreshToken> _refreshTokens;

        /// <summary>
        /// Gets the home directory where all <see cref="IRefreshToken"/> entities are stored persistently. Use <see cref="Initialize(string)"/> to specify the home directory.
        /// </summary>
        public string HomeDirectory
        {
            get
            {
                return _homeDirectory;
            }
        }

        /// <summary>
        /// Initializes this <see cref="RefreshTokenFileStorage"/> and reads all <see cref="RefreshTokenJson"/> from the home directory.
        /// </summary>
        /// <param name="homeDirectory">home directory path</param>
        public void Initialize(string homeDirectory)
        {
            _homeDirectory = homeDirectory;

            if (!Directory.Exists(homeDirectory))
            {
                Directory.CreateDirectory(homeDirectory);
                File.WriteAllText(_homeDirectory + "ID.config", 0 + "");
            }

            _refreshTokens = new List<IRefreshToken>();

            //load all client accounts:
            foreach (string file in Directory.EnumerateFiles(_homeDirectory, "*.json"))
            {
                string json = File.ReadAllText(file);
                RefreshTokenJson rtj = JsonSerializer.DeserializeJson<RefreshTokenJson>(json);
                if (rtj != null)
                {
                    _refreshTokens.Add(rtj);
                }
            }
        }

        /// <summary>
        /// Saves the passed <see cref="IRefreshToken"/> to disk.
        /// </summary>
        /// <param name="data">data to be saved</param>
        private void SaveToDisk(IRefreshToken data)
        {
            File.WriteAllText(_homeDirectory + data.RefreshToken + ".json", JsonSerializer.SerializeJson((RefreshTokenJson)data));
        }

        /// <summary>
        /// Deletes the <see cref="IRefreshToken"/> having the passed ID from disk.
        /// </summary>
        /// <param name="refreshToken">refresh token of the <see cref="IRefreshToken"/> which should be deleted</param>
        private void RemoveFromDisk(string refreshToken)
        {
            File.Delete(_homeDirectory + refreshToken + "*.json");
        }

        public IList<IRefreshToken> GetAllRefreshTokens()
        {
            IList<IRefreshToken> copy = new List<IRefreshToken>();
            foreach(IRefreshToken rt in _refreshTokens)
            {
                copy.Add(rt);
            }
            return copy;
        }

        public IRefreshToken GetRefreshToken(string refreshToken)
        {
            foreach(IRefreshToken rt in _refreshTokens)
            {
                if (rt.RefreshToken.CompareTo(refreshToken) == 0)
                {
                    return rt;
                }        
            }
            return null;
        }

        public bool HasRefreshToken(string refreshToken)
        {
            return GetRefreshToken(refreshToken) != null;
        }

        public void CreateRefreshToken(string refreshToken, IRefreshToken data)
        {
            if (String.IsNullOrWhiteSpace(refreshToken))
            {
                throw new Exception("refresh token is not set");
            }
            if(data == null)
            {
                throw new Exception("data is not set");
            }
            if (HasRefreshToken(refreshToken))
            {
                throw new Exception("Refresh token is already existing");
            }
            else
            {
                data.RefreshToken = refreshToken;
                _refreshTokens.Add(data);
                SaveToDisk(data);
            }
        }

        public void CreateRefreshToken(string refreshToken, string subject, string clientId, long validUntil, string scope, bool isInvalidated)
        {
            if (String.IsNullOrWhiteSpace(refreshToken))
            {
                throw new Exception("refresh token is not set");
            }
            if (HasRefreshToken(refreshToken))
            {
                throw new Exception("Refresh token is already existing");
            }
            else
            {
                IRefreshToken data = new RefreshTokenJson();
                data.RefreshToken = refreshToken;
                data.Subject = subject;
                data.ClientId = clientId;
                data.ValidUntil = validUntil;
                data.Scope = scope;
                data.IsInvalidated = isInvalidated;

                _refreshTokens.Add(data);
                SaveToDisk(data);
            }
        }

        public void UpdateRefreshToken(string refreshToken, IRefreshToken data)
        {
            if (String.IsNullOrWhiteSpace(refreshToken))
            {
                throw new Exception("refresh token is not set");
            }
            if (data == null)
            {
                throw new Exception("data is not set");
            }
            IRefreshToken rt = GetRefreshToken(refreshToken);
            if(rt == null)
            {
                throw new Exception("refresh token does not exist");
            }
            else
            {
                rt.Subject = data.Subject;
                rt.ClientId = data.ClientId;
                rt.ValidUntil = data.ValidUntil;
                rt.Scope = data.Scope;
                rt.IsInvalidated = data.IsInvalidated;
                data.RefreshToken = refreshToken;

                SaveToDisk(data);
            }
        }

        public void DeleteRefreshToken(string refreshToken)
        {
            if (String.IsNullOrWhiteSpace(refreshToken))
            {
                throw new Exception("refresh token is not set");
            }
            IRefreshToken rt = GetRefreshToken(refreshToken);
            if(rt != null)
            {
                _refreshTokens.Remove(rt);
                RemoveFromDisk(refreshToken);
            }
        }

   

    }
}
