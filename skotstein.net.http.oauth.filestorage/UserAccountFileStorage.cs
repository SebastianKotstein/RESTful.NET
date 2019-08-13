using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.oauth.filestorage
{
    public class UserAccountFileStorage : IUserAccountStorage
    {
        private string _homeDirectory;

        private IList<IUserAccount> _userAccounts;
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
        /// Gets the home directory where all <see cref="IUserAccount"/> entities are stored persistently. Use <see cref="Initialize(string)"/> to specify the home directory.
        /// </summary>
        public string HomeDirectory
        {
            get
            {
                return _homeDirectory;
            }
        }

        /// <summary>
        /// Initializes this <see cref="UserAccountFileStorage"/> and reads all <see cref="UserAccountJson"/> from the home directory as well as the current <see cref="IdCounter"/>.
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

            _userAccounts = new List<IUserAccount>();

            //load all client accounts:
            foreach (string file in Directory.EnumerateFiles(_homeDirectory, "*.json"))
            {
                string json = File.ReadAllText(file);
                UserAccountJson uaj = JsonSerializer.DeserializeJson<UserAccountJson>(json);
                if (uaj != null)
                {
                    _userAccounts.Add(uaj);
                }
            }

            //load ID counter
            string id = File.ReadAllText(_homeDirectory + "ID.config");
            _idCounter = Int32.Parse(id);
        }

        /// <summary>
        /// Saves the passed <see cref="IUserAccount"/> to disk.
        /// </summary>
        /// <param name="data">data to be saved</param>
        private void SaveToDisk(IUserAccount data)
        {
            UserAccountJson uaj = UserAccountJson.CreateInstance(data);
            File.WriteAllText(_homeDirectory + data.ClientId + ".json", JsonSerializer.SerializeJson(uaj));
        }

        /// <summary>
        /// Deletes the <see cref="IUserAccount"/> having the passed ID from disk.
        /// </summary>
        /// <param name="userId">user ID of the <see cref="IUserAccount"/> which should be deleted</param>
        private void RemoveFromDisk(string userId)
        {
            File.Delete(_homeDirectory + userId + ".json");
        }


        public IList<IUserAccount> GetAllUsers()
        {
            IList<IUserAccount> copy = new List<IUserAccount>();
            foreach(IUserAccount ua in _userAccounts)
            {
                copy.Add(ua);
            }
            return copy;
        }


        public IUserAccount GetUserById(string userId)
        {
            foreach(IUserAccount ua in _userAccounts)
            {
                if (ua.UserId.CompareTo(userId) == 0)
                {
                    return ua;
                }
            }
            return null;
        }

        public IUserAccount GetUserByName(string username)
        {
            foreach(IUserAccount ua in _userAccounts)
            {
                if (ua.Username.CompareTo(username) == 0)
                {
                    return ua;
                }
            }
            return null;
        }


        public bool HasUser(string userId)
        {
            return GetUserById(userId) != null;
        }



        public string CreateUser(IUserAccount data)
        {
            if(data == null)
            {
                throw new Exception("data is not set");
            }
            if (String.IsNullOrWhiteSpace(data.Username))
            {
                throw new Exception("username is not set");
            }
            if (GetUserByName(data.Username)!=null)
            {
                throw new Exception("User is already existing");
            }
            else
            {

                data.UserId = "" + IdCounter;
                _userAccounts.Add(data);
                SaveToDisk(data);
                IdCounter++;
                return data.UserId;
            }
        }


        public void UpdateUser(string userId, IUserAccount data)
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                throw new Exception("user ID is not set");
            }
            if(data == null)
            {
                throw new Exception("data is not set");
            }
            IUserAccount ua = GetUserById(userId);
            if(ua == null)
            {
                throw new Exception("User is not exisiting");
            }
            else
            {
                ua.ClientId = data.ClientId;
                ua.IsBlocked = data.IsBlocked;
                ua.Password = data.Password;
                ua.Scope = data.Scope;
                ua.Name = data.Name;
                ua.Description = data.Description;

                SaveToDisk(ua);
            }
        }

        public void DeleteUser(string userId)
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                throw new Exception("user ID is not set");
            }
            IUserAccount ua = GetUserById(userId);
            if(ua != null)
            {
                _userAccounts.Remove(ua);
                RemoveFromDisk(userId);
            }
        }

     

    }
}
