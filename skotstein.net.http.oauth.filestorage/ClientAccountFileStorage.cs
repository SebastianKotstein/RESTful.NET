using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.filestorage
{
    public class ClientAccountFileStorage : IClientAccountStorage
    {
        private string _homeDirectory;

        private IList<IClientAccount> _clientAccounts;
        private int _idCounter;

        /// <summary>
        /// Gets or sets the current ID Counter. By setting the ID counter, its value is stored persistently in the home directory.
        /// </summary>
        public int IdCounter
        {
            get
            {
                return _idCounter;
            }

            set
            {
                _idCounter = value;
                File.WriteAllText(_homeDirectory + "ID.config", _idCounter + "");
            }
        }

        /// <summary>
        /// Gets the home directory where all <see cref="IClientAccount"/> entities are stored persistently. Use <see cref="Initialize(string)"/> to specify the home directory.
        /// </summary>
        public string HomeDirectory
        {
            get
            {
                return _homeDirectory;
            }
        }

        /// <summary>
        /// Initializes this <see cref="ClientAccountFileStorage"/> and reads all <see cref="ClientAccountJson"/> from the home directory as well as the current <see cref="IdCounter"/>.
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

            _clientAccounts = new List<IClientAccount>();

            //load all client accounts:
            foreach(string file in Directory.EnumerateFiles(_homeDirectory, "*.json"))
            {
                string json = File.ReadAllText(file);
                ClientAccountJson caj = JsonSerializer.DeserializeJson<ClientAccountJson>(json);
                if(caj != null)
                {
                    _clientAccounts.Add(caj);
                }
            }

            //load ID counter
            string id = File.ReadAllText(_homeDirectory + "ID.config");
            _idCounter = Int32.Parse(id);
        }

        /// <summary>
        /// Saves the passed <see cref="IClientAccount"/> to disk.
        /// </summary>
        /// <param name="data">data to be saved</param>
        private void SaveToDisk(IClientAccount data)
        {
            ClientAccountJson caj = ClientAccountJson.CreateInstance(data);

            File.WriteAllText(_homeDirectory + data.ClientId + ".json", JsonSerializer.SerializeJson(caj));
        }

        /// <summary>
        /// Deletes the <see cref="IClientAccount"/> having the passed ID from disk.
        /// </summary>
        /// <param name="clientId">client ID of the <see cref="IClientAccount"/> which should be deleted</param>
        private void RemoveFromDisk(string clientId)
        {
            File.Delete(_homeDirectory + clientId + ".json");
        }

        public IList<IClientAccount> GetAllClients()
        {
            IList<IClientAccount> copy = new List<IClientAccount>();
            foreach(IClientAccount ca in _clientAccounts)
            {
                copy.Add(ca);
            }
            return copy;
        }

        public IClientAccount GetClient(string clientId)
        {
            foreach(IClientAccount ca in _clientAccounts)
            {
                if (ca.ClientId.CompareTo(clientId) == 0)
                {
                    return ca;
                }
            }
            return null;
        }

        public bool HasClient(string clientId)
        {
            return GetClient(clientId) != null;
        }

        public string CreateClient(IClientAccount data)
        {
            if(data == null)
            {
                throw new Exception("data is not set");
            }
            else
            {
                data.ClientId = "" + IdCounter;
                _clientAccounts.Add(data);
                SaveToDisk(data);
                IdCounter++;
                return data.ClientId;
            }
        }

        public void UpdateClient(string clientId, IClientAccount data)
        {
            if (String.IsNullOrWhiteSpace(clientId))
            {
                throw new Exception("client ID is not set");
            }
            if (data == null)
            {
                throw new Exception("data is not set");
            }
            IClientAccount ca = GetClient(clientId);
            if(ca == null)
            {
                throw new Exception("Client does not exist");
            }
            else
            {
                ca.AccessTokenExpiryInSeconds = data.AccessTokenExpiryInSeconds;
                ca.ClientSecret = data.ClientSecret;
                ca.IsBlocked = data.IsBlocked;
                ca.RefreshTokenExpiryInSeconds = data.RefreshTokenExpiryInSeconds;
                ca.Scope = data.Scope;
                ca.HasUser = data.HasUser;
                ca.IsClientIdRequiredForRefreshToken = data.IsClientIdRequiredForRefreshToken;
                ca.IsClientSecretRequiredForPasswordGrant = data.IsClientSecretRequiredForPasswordGrant;
                ca.IsClientSecretRequiredForRefreshToken = data.IsClientSecretRequiredForRefreshToken;
                ca.IsUserIdRequiredForRefreshToken = data.IsUserIdRequiredForRefreshToken;
                ca.IsUserPasswordRequiredForRefreshToken = data.IsUserPasswordRequiredForRefreshToken;
                ca.FriendlyName = data.FriendlyName;
                ca.Description = data.Description;

                data.ClientId = clientId;
                SaveToDisk(data);
            }

        }

        public void DeleteClient(string clientId)
        {
            if (String.IsNullOrWhiteSpace(clientId))
            {
                throw new Exception("client ID is not set");
            }
            IClientAccount ca = GetClient(clientId);
            if(ca != null)
            {
                _clientAccounts.Remove(ca);
                RemoveFromDisk(clientId);
            }
        }


        

    }
}
