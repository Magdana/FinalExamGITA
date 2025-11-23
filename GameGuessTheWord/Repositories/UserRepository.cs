using GameGuessTheWord.Entities;
using System.Xml;

namespace GameGuessTheWord.Repositories;

internal class UserRepository
{
    private List<User> _users;
    private readonly string _filePath;

    public bool Any { get; internal set; }

    public UserRepository(string filePath)
    {
        _filePath = filePath;
        _users = LoadUsers();
    }

    private List<User> LoadUsers()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                return new List<User>();
            }

            var users = new List<User>();
            var doc = new XmlDocument();
            doc.Load(_filePath);

            var userNodes = doc.SelectNodes("//Users/user");
            if (userNodes != null)
            {
                foreach (XmlNode userNode in userNodes)
                {
                    var user = new User
                    {
                        Name = userNode.Attributes?["name"]?.Value ?? string.Empty,
                        UserName = userNode.SelectSingleNode("userName")?.InnerText ?? string.Empty,
                        Password = userNode.SelectSingleNode("password")?.InnerText ?? string.Empty,
                    };

                    if (int.TryParse(userNode.SelectSingleNode("higestScore")?.InnerText, out int score))
                    {
                        user.HigestScore = score;
                    }
                    else
                    {
                        user.HigestScore = null;
                    }
                    users.Add(user);
                }
            }
            return users;
        }
        catch (XmlException ex)
        {
            Console.WriteLine($"Error loading users from XML: {ex.Message}");
            return new List<User>();
        }
        catch (IOException ex)
        {
            Console.WriteLine($"File error while loading users: {ex.Message}");
            return new List<User>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred while loading users: {ex.Message}");
            return new List<User>();
        }
    }

    private void SaveUsers()
    {
        try
        {
            var doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", string.Empty);
            doc.AppendChild(xmlDeclaration);
            XmlElement root = doc.CreateElement("Users");
            doc.AppendChild(root);

            foreach (var user in _users)
            {
                XmlElement userEl = doc.CreateElement("user");
                root.AppendChild(userEl);

                XmlAttribute nameAttr = doc.CreateAttribute("name");
                nameAttr.Value = user.Name;
                userEl.Attributes.Append(nameAttr);

                XmlElement userNameEl = doc.CreateElement("userName");
                userNameEl.InnerText = user.UserName;
                userEl.AppendChild(userNameEl);

                XmlElement passwordEl = doc.CreateElement("password");
                passwordEl.InnerText = user.Password;
                userEl.AppendChild(passwordEl);

                XmlElement scoreEl = doc.CreateElement("higestScore");
                scoreEl.InnerText = user.HigestScore.HasValue ? user.HigestScore.Value.ToString() : string.Empty;
                userEl.AppendChild(scoreEl);
            }
            doc.Save(_filePath);
        }
        catch (XmlException ex)
        {
            Console.WriteLine($"Error saving users to XML: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"File error while saving users: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred while saving users: {ex.Message}");
        }
    }

    public User? GetUserByUsername(string username)
    {
        return _users.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    public List<User> GetTopTenUsers()
    {
        return _users.Where(u => u.HigestScore.HasValue && u.HigestScore > 0).OrderBy(u => u.HigestScore).Take(10).ToList();
    }

    public User AddUser(User user)
    {
        try
        {
            if (GetUserByUsername(user.UserName) != null)
            {
                throw new Exception("User with the same username already exists.");
            }
            if (string.IsNullOrWhiteSpace(user.Name) ||
                string.IsNullOrWhiteSpace(user.UserName) ||
                string.IsNullOrWhiteSpace(user.Password))
            {
                throw new Exception("User properties cannot be null or empty.");
            }
            _users.Add(user);
            SaveUsers();
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding user: {ex.Message}");
            throw;
        }
    }

    public void UpdateUserScore(User user, int newScore)
    {
        var userToUpdate = _users.FirstOrDefault(u => u.UserName.Equals(user.UserName, StringComparison.OrdinalIgnoreCase));
        if (userToUpdate != null)
        {
            if (!userToUpdate.HigestScore.HasValue || newScore < userToUpdate.HigestScore.Value)
            {
                userToUpdate.HigestScore = newScore;
                SaveUsers();
            }
        }
    }

    
}
